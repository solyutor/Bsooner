using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Int32SerializatrionTests : SerializationTests
    {
        public struct HasInt
        {
            public int Id;
        }

        public struct HasNullableInt
        {
            public int? Id;
        }

        [Test]
        public void Serialize_not_nullable_int()
        {
            AssertSerializedEqually(new HasInt {Id = 42});
        }

        [Test]
        public void Serialize_nullable_int_as_null()
        {
            AssertSerializedEqually(new HasNullableInt { Id = null });
        }

        [Test]
        public void Serialize_nullable_int_as_not_null()
        {
            AssertSerializedEqually(new HasNullableInt { Id = 42 });
        }
    }
}