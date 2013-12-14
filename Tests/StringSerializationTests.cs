using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class StringSerializationTests : SerializationTests
    {
        public struct HasString
        {
            public string Name;
        }

        [Test]
        public void Can_serialize_struct()
        {
            AssertSerializedEqually(new HasString { Name = "Isaak Newton" });
        }
    }
}