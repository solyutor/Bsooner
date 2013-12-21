using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Bsooner
{
    public static class TypeExtenstions
    {
        public static readonly HashSet<Type> SimpleTypes = new HashSet<Type>
                                                           {
                                                               typeof(double), typeof(double?),
                                                               typeof(int), typeof(int?),
                                                               typeof(long), typeof(long?),
                                                               typeof(bool), typeof(bool?),
                                                               typeof(DateTime), typeof(DateTime?),
                                                               typeof(string)
                                                           };
        
        public static MemberInfo Member<T, TTProperty>(this Expression<Func<T, TTProperty>> property)
        {
            return ((MemberExpression) property.Body).Member;
        }

        public static Type GetPropertyOrFieldType(this MemberInfo member)
        {
            var property = member as PropertyInfo;
            if (property != null)
            {
                return property.PropertyType;
            }

            var field = (FieldInfo)member;

            return field.FieldType;
        }


        public static WriteMethod GetWriteMethod(this MemberInfo member)
        {
            var type = member.GetPropertyOrFieldType();
            if ((type == typeof (string) && member.GetCustomAttributes(typeof (ObjectIdAttribute), true).Length > 0))
            {
                return WriteMethod.ObjectId;
            }

            return type.GetWriteMethod();
        }


        public static WriteMethod GetWriteMethod(this Type type)
        {
            if (SimpleTypes.Contains(type))
            {
                return WriteMethod.SimpleType;
            }

            var isBsonIdMember = type == typeof(ObjectId) || type == typeof(ObjectId?);
            if (isBsonIdMember)
            {
                return WriteMethod.ObjectId;
            }

            var isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

            var isNullableStruct = isNullable && !type.GetGenericArguments()[0].IsPrimitive;
            if (isNullableStruct)
            {
                return WriteMethod.NullableStruct;
            }

            var isStruct = type.IsValueType
                && !type.IsPrimitive
                && !(isNullable && type.GetGenericArguments()[0].IsPrimitive);

            if (isStruct)
            {
                return WriteMethod.Struct;
            }

            if (type == typeof (byte[]))
            {
                return WriteMethod.Binary;
            }

            if (type.IsArray)
            {
                return WriteMethod.Array;
            }

            return WriteMethod.Class;
        }

        public static BsonType GetBsonType(this Type type)
        {
            if (type == typeof (double) || type == typeof (double?))
            {
                return BsonType.Double;
            }

            if (type == typeof (int) || type == typeof (int?))
            {
                return BsonType.Int32;
            }

            if (type == typeof (long) || type == typeof (long?))
            {
                return BsonType.Int64;
            }

            if (type == typeof (bool) || type == typeof (bool?))
            {
                return BsonType.Boolean;
            }

            if (type == typeof (DateTime) || type == typeof (DateTime?))
            {
                return BsonType.UtcDateTime;
            }

            if (type == typeof (string))
            {
                return BsonType.String;
            }

            if (type == typeof (byte[]))
            {
                return BsonType.Binary;
            }

            if (type == typeof (ObjectId))
            {
                return BsonType.ObjectId;
            }

            if (type.IsArray)
            {
                return BsonType.Array;
            }

            return BsonType.Document;
        }
    }
}