using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Text;

namespace Bsooner.Reading
{
    public class FastBsonReader
    {
        private readonly Stream _stream;
        private int _length;
        private BsonToken _token;
        private BsonType _bsonType;
        private string _propertyName;
        private List<byte> _stringBytes;
        private byte[] _buffer;

        public FastBsonReader(Stream stream)
        {
            _stream = stream;
            _length = int.MinValue;
            _token = BsonToken.None;
            _bsonType = BsonType.None;
            _propertyName = null;
            _stringBytes = new List<byte>(128);
            _buffer = new byte[128];
        }

        public BsonToken Token
        {
            get { return _token; }
        }

        public BsonType BsonType
        {
            get { return _bsonType; }
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }

        public bool Read()
        {
            switch (_token)
            {
                case BsonToken.None:
                    return ReadNone();
                case BsonToken.StartObject:
                    return ReadStartObject();
                case BsonToken.BsonType:
                    return ReadBsonType();
            }

            return false;
        }

        private bool ReadNone()
        {
            _length = _stream.ReadByte();
            _length |= _stream.ReadByte() << 8;
            _length |= _stream.ReadByte() << 16;
            _length |= _stream.ReadByte() << 24;
            var readObject = _length >= 5; //Minimum document length
            if (readObject)
            {
                _token = BsonToken.StartObject;
                _bsonType = BsonType.None;
            }
            return readObject;
        }

        private bool ReadStartObject()
        {
            _bsonType = (BsonType)_stream.ReadByte();
            if (_bsonType == BsonType.None)
            {
                _token = BsonToken.EndObject;
            }
            else
            {
                _token = BsonToken.BsonType;
            }
            return true;
        }

        private bool ReadBsonType()
        {
            byte current;
            while ((current = (byte)_stream.ReadByte()) > 0)
            {
                _stringBytes.Add(current);
            }
            //TODO: optimize memory traffic
            _propertyName = BsonDefaults.Encoding.GetString(_stringBytes.ToArray());
            _stringBytes.Clear();
            _token = BsonToken.PropertyName;
            return true;
        }

        public int ReadInt()
        {
            var first = _stream.ReadByte();
            var second = _stream.ReadByte();
            var third = _stream.ReadByte();
            var forth = _stream.ReadByte();
            var result = forth << 24 | third << 16 | second << 8 | first;
            return result;
        }

        public unsafe double ReadDouble()
        {
            _stream.Read(_buffer, 0, 8);
            fixed (byte* bufferPointer = _buffer)
            {
                return *(double*)bufferPointer;
            }
            /*
             Alternative way. Check performance later
                        var buffer = stackalloc byte[8];
                        buffer[0] = _buffer[0];
                        buffer[1] = _buffer[1];
                        buffer[2] = _buffer[2];
                        buffer[3] = _buffer[3];
                        buffer[4] = _buffer[4];
                        buffer[5] = _buffer[5];
                        buffer[6] = _buffer[6];
                        buffer[7] = _buffer[7];
                        return *(double*) buffer;
              */
        }

        public string ReadString()
        {
            var bytesProcessed = 0;
            var stringLength = ReadInt()-1;

            var decoder = BsonDefaults.Encoding.GetDecoder();

            const int bufferSize = 128;
            var byteBuffer = new byte[bufferSize];

            var charBuffer = new char[bufferSize];

            var stringBuilder = new StringBuilder();
            do
            {
                var bytesLeft = stringLength - bytesProcessed;
                var bytesToRead = bytesLeft > bufferSize ? bufferSize : bytesLeft;

                var bytesRead = _stream.Read(byteBuffer, 0, bytesToRead);

                var chars = decoder.GetChars(byteBuffer, 0, bytesRead, charBuffer, 0);

                stringBuilder.Append(charBuffer, 0, chars);
                bytesProcessed += bytesRead;
            }
            while (bytesProcessed < stringLength);
            return stringBuilder.ToString();

        }
    }
}