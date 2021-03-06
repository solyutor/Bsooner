﻿using NUnit.Framework;

namespace Tests
{
    public class SerializationTests
    {
        protected static void AssertSerializedEqually<T>(T probe)
        {
            var expected = probe.SerializeJsonNet();
            var actual = probe.SerializeFast();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}