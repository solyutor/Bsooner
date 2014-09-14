using Bsooner;
using Bsooner.Reading;
using NUnit.Framework;

namespace Tests.Reading
{
    [TestFixture]
    public class DoubleDeserializationTests
    {
        [Test]
        public void Can_recognize_double()
        {
            var origin = new HasGeneric<double> { Property = 0x01234567 };
            var serialized = TestHelper.Serialize(origin);

            var reader = new FastBsonReader(serialized);
            reader.Read();

            var hasSomething = reader.Read();

            Assert.That(hasSomething, Is.True);
            Assert.That(reader.BsonType, Is.EqualTo(BsonType.Double));
            Assert.That(reader.Token, Is.EqualTo(BsonToken.BsonType));

            hasSomething = reader.Read();

            Assert.That(hasSomething, Is.True);
            Assert.That(reader.BsonType, Is.EqualTo(BsonType.Double));
            Assert.That(reader.Token, Is.EqualTo(BsonToken.PropertyName));
            Assert.That(reader.PropertyName, Is.EqualTo("Property"));

            var value = reader.ReadDouble();
            Assert.That(value, Is.EqualTo(origin.Property));
        }
    }
}