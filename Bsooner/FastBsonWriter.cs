using System;
using System.IO;

namespace Bsooner
{
    public class FastBsonWriter : IDisposable
    {
        private Stream _stream;
        private byte[] _buffer;
        private readonly int _maxCharsInBuffer;

        public const int DefaultBufferSize = 64;

        public FastBsonWriter(Stream stream, int bufferSize = DefaultBufferSize)
        {
            if (!stream.CanSeek)
            {
                throw new NotSupportedException("Non seek-able streams are not supported");
            }
            _stream = stream;
            _buffer = new byte[bufferSize];
            _maxCharsInBuffer = _buffer.Length / BsonDefaults.MaxCharLength;
        }

        public Stream BaseStream
        {
            get { return _stream; }
        }

        public FastBsonWriter WriteProperty(string name, int value)
        {
            WriteType(BsonType.Int32).
            WritePropertyName(name);

            _buffer[0] = (byte)value;
            _buffer[1] = (byte)(value >> 8);
            _buffer[2] = (byte)(value >> 16);
            _buffer[3] = (byte)(value >> 24);

            _stream.Write(_buffer, 0, 4);
            return this;
        }

        public FastBsonWriter WriteProperty(string name, int? value)
        {
            if (value.HasValue)
            {
                return WriteProperty(name, value.Value);
            }

            return WriteNullProperty(name);
        }

        public unsafe FastBsonWriter WriteProperty(string name, double value)
        {
            WriteType(BsonType.Double);
            WritePropertyName(name);

            ulong doubleAsULong = *(ulong*)&value;

            _buffer[0] = (byte)doubleAsULong;
            _buffer[1] = (byte)(doubleAsULong >> 8);
            _buffer[2] = (byte)(doubleAsULong >> 16);
            _buffer[3] = (byte)(doubleAsULong >> 24);
            _buffer[4] = (byte)(doubleAsULong >> 32);
            _buffer[5] = (byte)(doubleAsULong >> 40);
            _buffer[6] = (byte)(doubleAsULong >> 48);
            _buffer[7] = (byte)(doubleAsULong >> 56);

            _stream.Write(_buffer, 0, 8);

            return this;
        }

        public FastBsonWriter WriteProperty(string name, double? value)
        {
            if (value.HasValue)
            {
                return WriteProperty(name, value.Value);
            }

            return WriteNullProperty(name);
        }

        public FastBsonWriter WriteProperty(string name, bool value)
        {
            WriteType(BsonType.Boolean);
            WritePropertyName(name);

            _stream.WriteByte((byte)(value ? 1 : 0));
            return this;
        }

        public FastBsonWriter WriteProperty(string name, bool? value)
        {
            if (value.HasValue)
            {
                return WriteProperty(name, value.Value);
            }

            return WriteNullProperty(name);
        }

        public FastBsonWriter WriteProperty(string name, long value)
        {
            WriteType(BsonType.Int64).
                WritePropertyName(name);

            WriteLong(value);

            return this;
        }

        public FastBsonWriter WriteProperty(string name, long? value)
        {
            if (value.HasValue)
            {
                return WriteProperty(name, value.Value);
            }

            return WriteNullProperty(name);
        }

        public FastBsonWriter WriteBinary(string name, byte[] value)
        {
            if (value == null)
            {
                return WriteNullProperty(name);
            }

            WriteType(BsonType.Binary);
            WritePropertyName(name);

            WriteInt(value.Length);

            _stream.WriteByte((byte)BinaryType.Generic);
            _stream.Write(value, 0, value.Length);

            return this;
        }

        public FastBsonWriter WriteStruct<TDocument>(string name, TDocument value)
            where TDocument : struct
        {
            WriteType(BsonType.Document);
            WritePropertyName(name);

            BsonSerializer<TDocument>.Instance.Serialize(this, value);

            return this;
        }

        public FastBsonWriter WriteNullableStruct<TDocument>(string name, TDocument? value)
            where TDocument : struct
        {
            if (value.HasValue)
            {
                return WriteStruct(name, value.Value);
            }

            return WriteNullProperty(name);
        }

        public FastBsonWriter WriteClass<TDocument>(string name, TDocument value)
            where TDocument : class
        {
            if (value != null)
            {
                WriteType(BsonType.Document);
                WritePropertyName(name);

                BsonSerializer<TDocument>.Instance.Serialize(this, value);
                return this;
            }

            return WriteNullProperty(name);
        }

        public FastBsonWriter WriteProperty(string name, string value)
        {
            if (value == null)
            {
                return WriteNullProperty(name);
            }

            WriteType(BsonType.String);
            WritePropertyName(name);

            //placeholder

            WriteInt(0);


            var startPosition = _stream.Position;

            WriteString(value);
            _stream.WriteByte(0); //string terminator
            var endPosition = _stream.Position;

            var length = endPosition - startPosition; //plus terminator

            _stream.Position = startPosition - 4;
            WriteInt((int)length);

            _stream.Position = endPosition;
            return this;
        }

        public FastBsonWriter WriteProperty(string name, DateTime value)
        {
            var utcValue = value.ToUniversalTime();

            long milliSeconds = (long)(utcValue - BsonDefaults.UnixEpoch).TotalMilliseconds;

            WriteType(BsonType.UtcDateTime);
            WritePropertyName(name)
            .WriteLong(milliSeconds);

            return this;
        }

        public FastBsonWriter WriteProperty(string name, DateTime? value)
        {
            if (value.HasValue)
            {
                return WriteProperty(name, value.Value);
            }

            return WriteNullProperty(name);
        }

        public FastBsonWriter WriteBsonId(string name, string value)
        {
            WriteType(BsonType.ObjectId);
            WritePropertyName(name);

            //empty id
            if (string.IsNullOrWhiteSpace(value))
            {
                Array.Clear(_buffer, 0, 12);
            }
            else
            {
                value.EnsureValidObjectIdString();

                _buffer[0] = value.ToByteFromHex(0);
                _buffer[1] = value.ToByteFromHex(2);
                _buffer[2] = value.ToByteFromHex(4);
                _buffer[3] = value.ToByteFromHex(6);
                _buffer[4] = value.ToByteFromHex(8);
                _buffer[5] = value.ToByteFromHex(10);
                _buffer[6] = value.ToByteFromHex(12);
                _buffer[7] = value.ToByteFromHex(14);
                _buffer[8] = value.ToByteFromHex(16);
                _buffer[9] = value.ToByteFromHex(18);
                _buffer[10] = value.ToByteFromHex(20);
                _buffer[11] = value.ToByteFromHex(22);
            }

            _stream.Write(_buffer, 0, 12);
            return this;
        }

        public FastBsonWriter WriteBsonId(string name, ObjectId? value)
        {
            if (value.HasValue)
            {
                return WriteBsonId(name, value.Value);
            }

            return WriteNullProperty(name);
        }

        public FastBsonWriter WriteBsonId(string name, ObjectId value)
        {
            WriteType(BsonType.ObjectId);
            WritePropertyName(name);

            _buffer[0] = value.Byte01;
            _buffer[1] = value.Byte02;
            _buffer[2] = value.Byte03;
            _buffer[3] = value.Byte04;
            _buffer[4] = value.Byte05;
            _buffer[5] = value.Byte06;
            _buffer[6] = value.Byte07;
            _buffer[7] = value.Byte08;
            _buffer[8] = value.Byte09;
            _buffer[9] = value.Byte10;
            _buffer[10] = value.Byte11;
            _buffer[11] = value.Byte12;

            _stream.Write(_buffer, 0, 12);

            return this;
        }

        private FastBsonWriter WriteType(BsonType bsonType)
        {
            _stream.WriteByte((byte) bsonType);
            return this;
        }

        private FastBsonWriter WritePropertyName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("property name could not be null", "name");
            }

            WriteString(name);
            _stream.WriteByte(0);// string terminator
            
            return this;
        }

        private FastBsonWriter WriteNullProperty(string name)
        {
            WriteType(BsonType.Null);
            WritePropertyName(name);

            return this;
        }

        internal void WriteInt(int value)
        {
            _buffer[0] = (byte)value;
            _buffer[1] = (byte)(value >> 8);
            _buffer[2] = (byte)(value >> 16);
            _buffer[3] = (byte)(value >> 24);
            _stream.Write(_buffer, 0, 4);
        }

        private unsafe void WriteString(string value)
        {
            int charStart = 0;
            int numLeft = value.Length;

            while (numLeft > 0)
            {
                int charsToProceed = (numLeft > _maxCharsInBuffer) ? _maxCharsInBuffer : numLeft;
                int byteLen;
                fixed (char* pChars = value)
                {
                    fixed (byte* pBytes = _buffer)
                    {
                        byteLen = BsonDefaults.Encoder.GetBytes(pChars + charStart, charsToProceed, pBytes, _buffer.Length, charsToProceed == numLeft);
                    }
                }

                _stream.Write(_buffer, 0, byteLen);
                charStart += charsToProceed;
                numLeft -= charsToProceed;
            }
        }

        private void WriteLong(long value)
        {
            _buffer[0] = (byte)value;
            _buffer[1] = (byte)(value >> 8);
            _buffer[2] = (byte)(value >> 16);
            _buffer[3] = (byte)(value >> 24);
            _buffer[4] = (byte)(value >> 32);
            _buffer[5] = (byte)(value >> 40);
            _buffer[6] = (byte)(value >> 48);
            _buffer[7] = (byte)(value >> 56);

            _stream.Write(_buffer, 0, 8);
        }

        public void Dispose()
        {
            _stream = null;
            _buffer = null;
        }
    }
}