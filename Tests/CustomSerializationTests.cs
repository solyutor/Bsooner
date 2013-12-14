using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Bsooner;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CustomSerializationTests
    {
        public struct StringAsBoolean
        {
            public string IsGod;
        }

        public struct HasBoolean
        {
            public bool IsGod;
        }

        [Test]
        public void Can_change_serialation_of_property()
        {
            var probe = new StringAsBoolean {IsGod = "true"};
            var customs = new Dictionary<MemberInfo, CustomSerializer<StringAsBoolean>>
                          {
                              {TypeExtenstions.Member<StringAsBoolean, string>(x => x.IsGod), CustomSerializer}
                          };
            
            var bodySerializer = Builder.BuildSerialiazer<StringAsBoolean>(customs);
            var serializer = new BsonSerializer<StringAsBoolean>(bodySerializer);

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                serializer.Serialize(writer, probe);
                var actual = TestHelper.Dump(stream);

                var expected = new HasBoolean { IsGod = true }.SerializeFast();

                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        public static void CustomSerializer(BinaryWriter writer, StringAsBoolean instance)
        {
            if ("True".Equals(instance.IsGod, StringComparison.OrdinalIgnoreCase))
            {
                writer.WriteProperty("IsGod", true);
            }
        }
    }
}