using System;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Int64SerializationTests : SerializationTests
    {
        public struct HasInt64
        {
            public Int64 Id;
        }

        public struct HasNullableInt64
        {
            public Int64? Id;
        }

        [Test]
        public void Serialize_not_nullable_long()
        {
            AssertSerializedEqually(new HasInt64 { Id = 42 });
        }

        [Test]
        public void Serialize_nullable_long_as_null()
        {
            AssertSerializedEqually(new HasNullableInt64 { Id = 42 });
        }

        [Test]
        public void Serialize_nullable_long_as_not_null()
        {
            AssertSerializedEqually(new HasNullableInt64 { Id = null });
        }    
    }
}