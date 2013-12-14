using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Int32SerializatrionTests
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
            var probe = new HasInt { Id = 42 };

            var expected = probe.SerializeJsonNet();
            var actual = probe.SerializeFast();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Serialize_nullable_int_as_null()
        {
            var probe = new HasNullableInt{ Id = 42 };

            var expected = probe.SerializeJsonNet();
            var actual = probe.SerializeFast();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Serialize_nullable_int_as_not_null()
        {
            var probe = new HasNullableInt { Id = null };

            var expected = probe.SerializeJsonNet();
            var actual = probe.SerializeFast();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}