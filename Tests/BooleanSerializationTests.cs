using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class BooleanSerializationTests : SerializationTests
    {
        public struct HasBoolean
        {
            public bool IsGod;
        }

        public struct HasNullableBoolean
        {
            public bool? IsGod;
        }

        [Test]
        public void Serialize_true_bool()
        {
            AssertSerializedEqually(new HasBoolean { IsGod = true });
        }

        [Test]
        public void Serialize_false_bool()
        {
            AssertSerializedEqually(new HasBoolean { IsGod = false });
        }

        [Test]
        public void Serialize_bool_as_null()
        {
            AssertSerializedEqually(new HasNullableBoolean { IsGod = null });
        }

        [Test]
        public void Serialize_nullable_true_bool()
        {
            AssertSerializedEqually(new HasNullableBoolean { IsGod = true });
        }

        [Test]
        public void Serialize_nullable_false_bool()
        {
            AssertSerializedEqually(new HasNullableBoolean { IsGod = false });
        }
    }
}