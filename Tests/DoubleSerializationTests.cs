using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class DoubleSerializationTests : SerializationTests
    {
        public struct HasDouble
        {
            public double Factor { get; set; }
        }

        public struct HasDoubleInt
        {
            public double? Factor;
        }

        [Test]
        public void Serialize_not_nullable_double()
        {
            AssertSerializedEqually(new HasDouble { Factor = 44 });
        }

        [Test]
        public void Serialize_nullable_int_as_null()
        {
            AssertSerializedEqually(new HasDoubleInt { Factor = 44 });
        }

        [Test]
        public void Serialize_nullable_int_as_not_null()
        {
            AssertSerializedEqually(new HasDoubleInt { Factor = null });
        }
    }
}