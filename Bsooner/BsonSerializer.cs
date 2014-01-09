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

        public void Serialize(FastBsonWriter writer, T instance)
        {
            var stream = writer.BaseStream;

            var startPosition = stream.Position;
            
            writer.WriteInt(0);//placeholder;

            _writeBody(writer, instance);

           writer.BaseStream.WriteByte(0); //objectTerminator

            var length = stream.Position - startPosition;

            var endPosition = stream.Position;

            stream.Position = startPosition;

            writer.WriteInt((int)length);

            stream.Position = endPosition;
        }
    }
}