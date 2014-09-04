using Bsooner;
using Bsooner.Reading;
using Newtonsoft.Json.Bson;
using NUnit.Framework;

namespace Tests.Reading
{
    [TestFixture]
    public class ObjectSerializationTests : SerializationTests
    {
        public class SimpleObject
        {
            
        }
        [Test]
        public void Can_deserialize_object()
        {
            var serialized = TestHelper.Serialize(new SimpleObject());

            var reader = new FastBsonReader(serialized);

            var hasSomething = reader.Read();

            Assert.That(reader.BsonType, Is.EqualTo(BsonType.Document));


            //var reader2 = new BsonReader(serialized);

            //if (reader2.Read())
            //{
                
            //}
        }
    }
}
