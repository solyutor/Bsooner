using Bsooner;
using Bsooner.Reading;
using NUnit.Framework;

namespace Tests.Reading
{
    [TestFixture]
    public class StringDeserializationTests
    {
        [Test]
        public void Can_recognize_string()
        {
            var origin = new HasGeneric<string> { Property = "Это очень, очень длинная строка! My extremely long string is going to be serialized to bson, and event deserialized back! My extremely long string is going to be serialized to bson, and event deserialized back! My extremely long string is going to be serialized to bson, and event deserialized back! My extremely long string is going to be serialized to bson, and event deserialized back! My extremely long string is going to be serialized to bson, and event deserialized back! My extremely long string is going to be serialized to bson, and event deserialized back! My extremely long string is going to be serialized to bson, and event deserialized back!" };
            var serialized = TestHelper.Serialize(origin);

            var reader = new FastBsonReader(serialized);
            reader.Read();

            var hasSomething = reader.Read();

            Assert.That(hasSomething, Is.True);
            Assert.That(reader.BsonType, Is.EqualTo(BsonType.String));
            Assert.That(reader.Token, Is.EqualTo(BsonToken.BsonType));

            hasSomething = reader.Read();

            Assert.That(hasSomething, Is.True);
            Assert.That(reader.BsonType, Is.EqualTo(BsonType.String));
            Assert.That(reader.Token, Is.EqualTo(BsonToken.PropertyName));
            Assert.That(reader.PropertyName, Is.EqualTo("Property"));

            var value = reader.ReadString();
            Assert.That(value, Is.EqualTo(origin.Property));
        }
    }
}