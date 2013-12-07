using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class DocumentSerializationTests : SerializationTests
    {
        public struct Document
        {
            public int Id;
        }

        public struct HasStruct
        {
            public Document Document;
        }

        public class RefDoc
        {
             
        }

        public struct HasObject
        {
            public RefDoc Document;
        }
        
        [Test]
        public void Can_serialize_struct()
        {
            AssertSerializedEqually(new HasStruct{Document = new Document{Id = 66}});
        }

        [Test]
        public void Can_serialize_nullable_struct_when_null()
        {
            AssertSerializedEqually(new HasStruct { Document = new Document { Id = 66 } });
        }

        [Test]
        public void Can_serialize_nullable_struct_when_not_null()
        {
            AssertSerializedEqually(new HasStruct { Document = new Document { Id = 66 } });
        }
    }
}