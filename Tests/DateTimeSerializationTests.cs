using System;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class DateTimeSerializationTests : SerializationTests
    {
        public struct HasDateTime
        {
            public DateTime BirthDay;
        }

        public struct HasNullableDateTime
        {
            public DateTime? BirthDay;
        }

        [Test]
        public void Can_serialize_date_time_property()
        {
            AssertSerializedEqually(new HasDateTime { BirthDay = DateTime.Now });
        }

        [Test]
        public void Can_serialize_nullable_date_time_when_null()
        {
            AssertSerializedEqually(new HasNullableDateTime { BirthDay = null });
        }

        [Test]
        public void Can_serialize_nullable_date_time_when_not_null()
        {
            AssertSerializedEqually(new HasNullableDateTime { BirthDay = DateTime.Now });
        }
    }
}