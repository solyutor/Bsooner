using Bsooner;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ObjectIdSerializationTests
    {
        public struct HasObjectIdString
        {
            [ObjectId]
            public string Id;
        }

        public class HasObjectIdMember
        {
            public ObjectId Id;
        }

        public class HasNullableObjectIdMember
        {
            public ObjectId? Id;
        }

        [Test]
        public void Can_serialize_string_as_object_id()
        {
            var probe = new HasObjectIdString { Id = "000000000000000000000000" };
            var expected = "15-00-00-00-07-49-64-00-00-00-00-00-00-00-00-00-00-00-00-00-00";
            string actual = probe.SerializeFast();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Can_serialize_object_id_struct()
        {
            var probe = new HasObjectIdMember { Id = new ObjectId(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12) };
            var expected = "15-00-00-00-07-49-64-00-01-02-03-04-05-06-07-08-09-0a-0b-0c-00";
            string actual = probe.SerializeFast();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Can_serialize_nullable_object_id_struct_when_null()
        {
            var probe = new HasNullableObjectIdMember { Id = null };
            var expected = "09-00-00-00-0a-49-64-00-00";
            string actual = probe.SerializeFast();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Can_serialize_nullable_object_id_struct_when_not_null()
        {
            var probe = new HasNullableObjectIdMember { Id = new ObjectId(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12) };
            var expected = "15-00-00-00-07-49-64-00-01-02-03-04-05-06-07-08-09-0a-0b-0c-00";
            string actual = probe.SerializeFast();
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}