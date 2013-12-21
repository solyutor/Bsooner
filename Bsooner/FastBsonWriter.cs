using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Bsooner
{
    public static class FastBsonWriter
    {
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1,1, 0,0,0, DateTimeKind.Utc);

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

        public static BinaryWriter WriteProperty(this BinaryWriter writer, string name, long value)
        {
            writer
                .WriteType(BsonType.Int64)
                .WritePropertyName(name)
                .Write(value);
            return writer;
        }

        public static BinaryWriter WriteProperty(this BinaryWriter writer, string name, long? value)
        {
            if (value.HasValue)
            {
                return writer.WriteProperty(name, value.Value);
            }

            return WriteNullProperty(writer, name);
        }

        public static BinaryWriter WriteBinary(this BinaryWriter writer, string name, byte[] value)
        {
            if (value == null)
            {
                return WriteNullProperty(writer, name);
            }

            writer
                .WriteType(BsonType.Binary)
                .WritePropertyName(name);

            writer.Write(value.Length);
            writer.Write((byte)BinaryType.Generic);

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

            var stringAsBytes = Encoding.UTF8.GetBytes(value);

            writer
                .WriteType(BsonType.String)
                .WritePropertyName(name)
                .Write(stringAsBytes.Length + 1); //size placeholder
            writer.Write(stringAsBytes);
            writer.Write(new byte()); // string terminator

            return writer;
        }

        public static BinaryWriter WriteProperty(this BinaryWriter writer, string name, DateTime value)
        {
            var utcValue = value.ToUniversalTime();

            var milliSeconds = (long)(utcValue - UnixEpoch).TotalMilliseconds;

            writer
                .WriteType(BsonType.UtcDateTime)
                .WritePropertyName(name)
                .Write(milliSeconds);
            return writer;
        }

        public static BinaryWriter WriteProperty(this BinaryWriter writer, string name, DateTime? value)
        {
            if (value.HasValue)
            {
                return writer.WriteProperty(name, value.Value);
            }

            return WriteNullProperty(writer, name);
        }

        public static BinaryWriter WriteBsonId(this BinaryWriter writer, string name, string value)
        {
            writer
                .WriteType(BsonType.ObjectId)
                .WritePropertyName(name);
            //empty id
            if (string.IsNullOrWhiteSpace(value))
            {
                writer.Write(new byte());
                writer.Write(new byte());
                writer.Write(new byte());
                writer.Write(new byte());
                writer.Write(new byte());
                writer.Write(new byte());
                writer.Write(new byte());
                writer.Write(new byte());
                writer.Write(new byte());
                writer.Write(new byte());
                writer.Write(new byte());
                writer.Write(new byte());
            }
            else
            {
                value.EnsureValidObjectIdString();

                writer.Write(value.ToByteFromHex(0));
                writer.Write(value.ToByteFromHex(2));
                writer.Write(value.ToByteFromHex(4));
                writer.Write(value.ToByteFromHex(6));
                writer.Write(value.ToByteFromHex(8));
                writer.Write(value.ToByteFromHex(10));
                writer.Write(value.ToByteFromHex(12));
                writer.Write(value.ToByteFromHex(14));
                writer.Write(value.ToByteFromHex(16));
                writer.Write(value.ToByteFromHex(18));
                writer.Write(value.ToByteFromHex(20));
                writer.Write(value.ToByteFromHex(22));
            }

            return writer;
        }

        public static BinaryWriter WriteBsonId(this BinaryWriter writer, string name, ObjectId? value)
        {
            if (value.HasValue)
            {
                return writer.WriteBsonId(name, value.Value);
            }

            return writer.WriteNullProperty(name);
        }

        public static BinaryWriter WriteBsonId(this BinaryWriter writer, string name, ObjectId value)
        {
            writer
                .WriteType(BsonType.ObjectId)
                .WritePropertyName(name);

            writer.Write(value.Byte01);
            writer.Write(value.Byte02);
            writer.Write(value.Byte03);
            writer.Write(value.Byte04);
            writer.Write(value.Byte05);
            writer.Write(value.Byte06);
            writer.Write(value.Byte07);
            writer.Write(value.Byte08);
            writer.Write(value.Byte09);
            writer.Write(value.Byte10);
            writer.Write(value.Byte11);
            writer.Write(value.Byte12);

            return writer;
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