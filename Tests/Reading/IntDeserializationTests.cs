using Bsooner;
using Bsooner.Reading;
using NUnit.Framework;

namespace Tests.Reading
{
    [TestFixture]
    public class IntDeserializationTests
    {
        [Test]
        public void Can_recognize_int()
        {
            var origin = 0x01234567;
            var hasGeneric = new HasGeneric<int> { Property = origin };
            var serialized = TestHelper.Serialize(hasGeneric);

            var reader = new FastBsonReader(serialized);
            reader.Read();

            var hasSomething = reader.Read();

            Assert.That(hasSomething, Is.True);
            Assert.That(reader.BsonType, Is.EqualTo(BsonType.Int32));
            Assert.That(reader.Token, Is.EqualTo(BsonToken.BsonType));

            hasSomething = reader.Read();

            Assert.That(hasSomething, Is.True);
            Assert.That(reader.BsonType, Is.EqualTo(BsonType.Int32));
            Assert.That(reader.Token, Is.EqualTo(BsonToken.PropertyName));
            Assert.That(reader.PropertyName, Is.EqualTo("Property"));

            var value = reader.ReadInt();

            Assert.That(value, Is.EqualTo(origin));
        }
    }
}