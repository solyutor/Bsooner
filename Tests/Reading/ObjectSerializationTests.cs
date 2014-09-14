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


    }
}
