using System;
using System.IO;

namespace Bsooner
{
    public class BsonSerializer<T>
    {
        private static readonly Action<BinaryWriter, T> WriteBody;

        static BsonSerializer()
        {
            Instance = new BsonSerializer<T>();
            WriteBody = Builder.BuildSerialiazer<T>();
        }

        public readonly static BsonSerializer<T> Instance;

        public void Serialize(BinaryWriter writer, T instance)
        {
            var stream = writer.BaseStream;

            var startPosition = stream.Position;
            
            writer.Write(new int()); //placeholder;

            WriteBody(writer, instance);

            writer.Write(new byte()); //objectTerminator

            var length = stream.Position - startPosition;

            var endPosition = stream.Position;

            stream.Position = startPosition;

            writer.Write((int)length);

            stream.Position = endPosition;
        }
    }
}