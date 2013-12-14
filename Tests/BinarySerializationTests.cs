using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class BinarySerializationTests : SerializationTests
    {
        public struct HasBytes
        {
            public byte[] Bytes;
        }

        [Test]
        public void Can_serialize_struct()
        {
            AssertSerializedEqually(new HasBytes { Bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 } });
        }
    }
}