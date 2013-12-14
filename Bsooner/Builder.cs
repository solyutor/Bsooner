using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bsooner
{
    public class Builder
    {
        public static Action<BinaryWriter, T> BuildSerialiazer<T>()
        {
            var members = typeof(T)
                .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.GetProperty)
                .Where(x => x.MemberType == MemberTypes.Field || x.MemberType == MemberTypes.Property);

            var writerParameter = Expression.Parameter(typeof(BinaryWriter));
            var instanceParameter = Expression.Parameter(typeof(T));

            var writeExpressions = new List<Expression>();

            foreach (var member in members)
            {
                var propertyName = Expression.Constant(member.Name);

                var value = Expression.PropertyOrField(instanceParameter, member.Name);

                var memberType = GetPropertyOrFieldType(member);

                var isNullable = memberType.IsGenericType && memberType.GetGenericTypeDefinition() == typeof(Nullable<>);

                var isNullableStruct = isNullable && !memberType.GetGenericArguments()[0].IsPrimitive;

                var isStruct = memberType.IsValueType 
                    && !memberType.IsPrimitive
                    && !(isNullable && memberType.GetGenericArguments()[0].IsPrimitive);

                var isBsonIdMember = 
                    (memberType == typeof(string) && member.GetCustomAttributes(typeof (ObjectIdAttribute), true).Length > 0) 
                    || memberType == typeof (ObjectId) 
                    || memberType == typeof (ObjectId?);    

                MethodInfo writeMethod;

                var fastBson = typeof(FastBsonWriter);


                //note: order does matter
                if (isBsonIdMember)
                {
                    writeMethod = fastBson.GetMethod("WriteBsonId", new[] { typeof(BinaryWriter), typeof(string), memberType });
                }
                else if(memberType == typeof(DateTime) || memberType ==  typeof(DateTime?))
                {
                    writeMethod = fastBson.GetMethod("WriteProperty", new[] { typeof(BinaryWriter), typeof(string), memberType });
                }
                else if (isStruct)
                {
                    writeMethod = fastBson.GetMethod("WriteStruct").MakeGenericMethod(memberType);
                }
                else if (isNullableStruct)
                {
                    writeMethod = fastBson.GetMethod("WriteNullableStruct").MakeGenericMethod(memberType);
                }
                else if (memberType == typeof(string))
                {
                    writeMethod = fastBson.GetMethod("WriteProperty", new[] { typeof(BinaryWriter), typeof(string), memberType });
                }
                else if(memberType == typeof(byte).MakeArrayType())
                {
                    writeMethod = fastBson.GetMethod("WriteProperty", new[] { typeof(BinaryWriter), typeof(string), memberType });
                }
                else if (memberType.IsClass)
                {
                    writeMethod = fastBson.GetMethod("WriteClass").MakeGenericMethod(memberType);
                }
                else
                {
                    writeMethod = fastBson.GetMethod("WriteProperty", new[] { typeof(BinaryWriter), typeof(string), memberType });
                }

                var writerExpression = Expression.Call(writeMethod, writerParameter, propertyName, value);
                writeExpressions.Add(writerExpression);

            }


            var body = Expression.Block(writeExpressions);

            var result = Expression.Lambda<Action<BinaryWriter, T>>(body, writerParameter, instanceParameter);
            return result.Compile();

        }

        private static BsonType GetBsonType(MemberInfo member)
        {
            var type = GetPropertyOrFieldType(member);

            if (type == typeof(int) || type == typeof(int?))
            {
                return BsonType.Int32;
            }

            return BsonType.Document;
        }

        private static Type GetPropertyOrFieldType(MemberInfo member)
        {
            var property = member as PropertyInfo;
            if (property != null)
            {
                return property.PropertyType;
            }

            var field = (FieldInfo)member;

            return field.FieldType;
        }
    }
}