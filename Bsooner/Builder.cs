using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bsooner
{
    public delegate void CustomSerializer<T>(FastBsonWriter writer, T instance);

    public class Builder
    {
        public static CustomSerializer<T> BuildSerialiazer<T>(IDictionary<MemberInfo, CustomSerializer<T>> serializers = null)
        {
            serializers = serializers ?? new Dictionary<MemberInfo, CustomSerializer<T>>();

            var members = typeof(T)
                .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.GetProperty)
                .Where(x => x.MemberType == MemberTypes.Field || x.MemberType == MemberTypes.Property);

            var fastBson = typeof(FastBsonWriter);

            var writerParameter = Expression.Parameter(fastBson);

            var instanceParameter = Expression.Parameter(typeof(T));

            var writeExpressions = new List<Expression>();

            foreach (var member in members)
            {
                CustomSerializer<T> serializer;
                if (serializers.TryGetValue(member, out serializer))
                {
                    var customExpression = Expression.Constant(serializer);
                    var call = Expression.Call(customExpression, "Invoke", Type.EmptyTypes, writerParameter, instanceParameter);
                    writeExpressions.Add(call);
                    continue;
                }

                var propertyName = Expression.Constant(member.Name);

                var value = Expression.PropertyOrField(instanceParameter, member.Name);

                var memberType = member.GetPropertyOrFieldType();

                var writeType = member.GetWriteMethod();

                MethodInfo writeMethod;

                switch (writeType)
                {
                    
                    case WriteMethod.Binary:
                        writeMethod = fastBson.GetMethod("WriteBinary", new[] { typeof(string), memberType });
                        break;
                    case WriteMethod.Class:
                        writeMethod = fastBson.GetMethod("WriteClass").MakeGenericMethod(memberType);
                        break;
                    case WriteMethod.NullableStruct:
                        writeMethod = fastBson.GetMethod("WriteNullableStruct").MakeGenericMethod(memberType);
                        break;
                    case WriteMethod.ObjectId:
                        writeMethod = fastBson.GetMethod("WriteBsonId", new[] {typeof(string), memberType });
                        break;
                    case WriteMethod.SimpleType:
                        writeMethod = fastBson.GetMethod("WriteProperty", new[] {typeof(string), memberType });
                        break;
                    case WriteMethod.Struct:
                        writeMethod = fastBson.GetMethod("WriteStruct").MakeGenericMethod(memberType);
                        break;
                    case WriteMethod.Array:
                        throw new NotSupportedException();
                    default:
                        throw new NotImplementedException();
                }

                var writerExpression = Expression.Call(writerParameter, writeMethod, propertyName, value);
                writeExpressions.Add(writerExpression);

            }


            var body = Expression.Block(writeExpressions);

            var result = Expression.Lambda<CustomSerializer<T>>(body, writerParameter, instanceParameter);
            return result.Compile();

        }
    }
}