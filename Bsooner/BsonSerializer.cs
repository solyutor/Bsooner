using System.IO;

namespace Bsooner
{
    public class BsonSerializer<T>
    {
        private readonly CustomSerializer<T> _writeBody;

        static BsonSerializer()
        {
            Instance = new BsonSerializer<T>(Builder.BuildSerialiazer<T>());
        }

        public readonly static BsonSerializer<T> Instance;

        public BsonSerializer(CustomSerializer<T> writeBody)
        {
            _writeBody = writeBody;
        }

        public void Serialize(BinaryWriter writer, T instance)
        {
            var stream = writer.BaseStream;

            var startPosition = stream.Position;
            
            writer.Write(new int()); //placeholder;

            _writeBody(writer, instance);

            writer.Write(new byte()); //objectTerminator

            var length = stream.Position - startPosition;

            var endPosition = stream.Position;

            stream.Position = startPosition;

            writer.Write((int)length);

            stream.Position = endPosition;
        }
    }
}