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
            var probe = new HasBoolean { IsGod = true };

            var expected = probe.SerializeJsonNet();
            var actual = probe.SerializeFast();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Serialize_false_bool()
        {
            var probe = new HasBoolean { IsGod = false };

            var expected = probe.SerializeJsonNet();
            var actual = probe.SerializeFast();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Serialize_bool_as_null()
        {
            var probe = new HasNullableBoolean { IsGod = null };

            var expected = probe.SerializeJsonNet();
            var actual = probe.SerializeFast();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Serialize_nullable_true_bool()
        {
            var probe = new HasNullableBoolean { IsGod = true };

            var expected = probe.SerializeJsonNet();
            var actual = probe.SerializeFast();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Serialize_nullable_false_bool()
        {
            var probe = new HasNullableBoolean { IsGod = false };

            var expected = probe.SerializeJsonNet();
            var actual = probe.SerializeFast();

            Assert.That(actual, Is.EqualTo(expected));
        }

    }
}