using NUnit.Framework;

namespace Bsooner
{
    [TestFixture]
    public class ScratchPad
    {
        [Test, Ignore("Serializes is out of order")]
        public void TEST()
        {
            var testStruct = new SerialTest { Id = 25 };

            var expected = testStruct.SerializeJsonNet();
            var actual = testStruct.SerializeFast();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}