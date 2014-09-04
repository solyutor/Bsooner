using System.IO;

namespace Bsooner.Reading
{
    public class FastBsonReader
    {
        private readonly MemoryStream _stream;
        private int _length;

        public FastBsonReader(MemoryStream stream)
        {
            _stream = stream;
            _length = int.MinValue;
        }

        public BsonType BsonType
        {
            get { return BsonType.ObjectId; }
        }

        public bool Read()
        {
            if (_length == int.MinValue)
            {
                _length = _stream.ReadByte();
                _length |= _stream.ReadByte() << 8;
                _length |= _stream.ReadByte() << 16;
                _length |= _stream.ReadByte() << 24;
            }
            return false;
        }
    }
}