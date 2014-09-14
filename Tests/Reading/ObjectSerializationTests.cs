using Bsooner;
using Bsooner.Reading;
using NUnit.Framework;

namespace Tests.Reading
{
    [TestFixture]
    public class ObjectSerializationTests
    {
        public class SimpleObject
        {

        }

        public class IntPropertyObject<TProperty>
        {
            public TProperty Property;
        }

        [Test]
        public void Can_recognize_start_object()
        {
            var serialized = TestHelper.Serialize(new SimpleObject());

            var reader = new FastBsonReader(serialized);

            var hasSomething = reader.Read();

            Assert.That(hasSomething, Is.True);
            Assert.That(reader.BsonType, Is.EqualTo(BsonType.None));
            Assert.That(reader.Token, Is.EqualTo(BsonToken.StartObject));
        }

        [Test]
        public void Can_recognize_end_object()
        {
            var serialized = TestHelper.Serialize(new SimpleObject());

            var reader = new FastBsonReader(serialized);
            reader.Read();
            var hasSomething = reader.Read();

            Assert.That(hasSomething, Is.True);
            Assert.That(reader.BsonType, Is.EqualTo(BsonType.None));
            Assert.That(reader.Token, Is.EqualTo(BsonToken.EndObject));
        }

        [Test]
        public void Can_recognize_int_property_object()
        {
            var serialized = TestHelper.Serialize(new IntPropertyObject<int> { Property = 0x01234567  });

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

        }
    }
}
