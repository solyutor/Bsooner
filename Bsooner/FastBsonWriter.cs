using System;
using System.IO;
using System.Text;

namespace Bsooner
{
    public static class FastBsonWriter
    {
        public static BinaryWriter WriteProperty(this BinaryWriter writer, string name, int value)
        {
            writer
                .WriteType(BsonType.Int32)
                .WritePropertyName(name)
                .Write(value);
            return writer;
        }

        public static BinaryWriter WriteProperty(this BinaryWriter writer, string name, int? value)
        {
            if (value.HasValue)
            {
                return writer.WriteProperty(name, value.Value);
            }

            return WriteNullProperty(writer, name);
        }

        public static BinaryWriter WriteProperty(this BinaryWriter writer, string name, double value)
        {
            writer
                .WriteType(BsonType.Double)
                .WritePropertyName(name)
                .Write(value);
            return writer;
        }

        public static BinaryWriter WriteProperty(this BinaryWriter writer, string name, double? value)
        {
            if (value.HasValue)
            {
                return writer.WriteProperty(name, value.Value);
            }

            return WriteNullProperty(writer, name);
        }

        public static BinaryWriter WriteProperty(this BinaryWriter writer, string name, bool value)
        {
            writer
                .WriteType(BsonType.Boolean)
                .WritePropertyName(name)
                .Write(value);
            return writer;
        }

        public static BinaryWriter WriteProperty(this BinaryWriter writer, string name, bool? value)
        {
            if (value.HasValue)
            {
                return writer.WriteProperty(name, value.Value);
            }

            return WriteNullProperty(writer, name);
        }

        public static BinaryWriter WriteProperty(this BinaryWriter writer, string name, byte[] value)
        {
            if (value == null)
            {
                return WriteNullProperty(writer, name);
            }

            writer
                .WriteType(BsonType.Binary)
                .WritePropertyName(name);

            writer.Write(value.Length);
            writer.Write((byte) BinaryType.Generic);

            writer.Write(value);

            return writer;
        }


        public static BinaryWriter WriteStruct<TDocument>(this BinaryWriter writer, string name, TDocument value)
            where TDocument : struct 
        {
            writer.WriteType(BsonType.Document);
            writer.WritePropertyName(name);

            BsonSerializer<TDocument>.Instance.Serialize(writer, value);

            return writer;
        }

        public static BinaryWriter WriteNullableStruct<TDocument>(this BinaryWriter writer, string name, TDocument? value)
            where TDocument : struct 
        {
            if (value.HasValue)
            {
                return writer.WriteStruct(name, value.Value);
            }

            return writer.WriteNullProperty(name);
        }

        public static BinaryWriter WriteClass<TDocument>(this BinaryWriter writer, string name, TDocument value)
            where TDocument : class 
        {
            if (value != null)
            {
                writer.WriteType(BsonType.Document);
                writer.WritePropertyName(name);

                BsonSerializer<TDocument>.Instance.Serialize(writer, value);
                return writer;
            }

            return writer.WriteNullProperty(name);
        }

        public static BinaryWriter WriteProperty(this BinaryWriter writer, string name, string value)
        {
            if (value == null)
            {
                return WriteNullProperty(writer, name);
            }

            return writer
                .WriteType(BsonType.String)
                .WritePropertyName(name)
                .WriteValue(value);

        }

        private static BinaryWriter WriteType(this BinaryWriter writer, BsonType bsonType)
        {
            writer.Write((byte)bsonType);
            return writer;
        }

        private static BinaryWriter WritePropertyName(this BinaryWriter writer, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("property name could not be null", "name");
            }

            var stringAsBytes = Encoding.UTF8.GetBytes(name);

            writer.Write(stringAsBytes);
            writer.Write(new byte()); // string terminator
            return writer;
        }

        private static BinaryWriter WriteValue(this BinaryWriter writer, string value)
        {
            if (value == null)
            {
                throw new ArgumentException("Null values should be written as BsonType.Null");
            }

            writer.Write(value);
            return writer;
        }

        private static BinaryWriter WriteNullProperty(this BinaryWriter writer, string name)
        {
            return writer
                .WriteType(BsonType.Null)
                .WritePropertyName(name);
        }
    }
}