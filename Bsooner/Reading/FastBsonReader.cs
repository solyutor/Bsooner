using System.Collections.Generic;
using System.IO;

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

        public FastBsonReader(Stream stream)
        {
            _stream = stream;
            _length = int.MinValue;
            _token = BsonToken.None;
            _bsonType = BsonType.None;
            _propertyName = null;
            _stringBytes = new List<byte>(128);
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
            while ((current = (byte) _stream.ReadByte()) > 0)
            {
                _stringBytes.Add(current);
            }
            //TODO: optimize memory traffic
            _propertyName = BsonDefaults.Encoding.GetString(_stringBytes.ToArray());
            _stringBytes.Clear();
            _token = BsonToken.PropertyName;
            return true;
        }
    }
}