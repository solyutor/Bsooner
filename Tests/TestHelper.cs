﻿using System;
using System.IO;
using System.Linq;
using Bsooner;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Tests
{
    public static class TestHelper
    {
        public static string SerializeFast<T>(this T value)
        {
            using (var stream = new MemoryStream())
            using (var writer = new FastBsonWriter(stream))
            {
                BsonSerializer<T>.Instance.Serialize(writer, value);
                var result = Dump(stream);
                Console.WriteLine("Actual  : " + result);
                return result;
            }
        }

        public static MemoryStream Serialize<T>(this T value)
        {
            var serialser = new JsonSerializer();
            var stream = new MemoryStream();
            var writer = new BsonWriter(stream);

            serialser.Serialize(writer, value);
            writer.Flush();

            var result = Dump(stream);
            Console.WriteLine("Expected: " + result);
            stream.Position = 0;
            return stream;
        }

        public static string SerializeJsonNet<T>(this T value)
        {
            var serialser = new JsonSerializer();
            using (var stream = new MemoryStream())
            using (var writer = new BsonWriter(stream))
            {
                serialser.Serialize(writer, value);
                var result = Dump(stream);
                Console.WriteLine("Expected: " + result);
                return result;
            }
        }

        public static string Dump(MemoryStream stream)
        {
            var buffer = stream.GetBuffer();
            return string.Join("-", buffer.Take((int)stream.Position).Select(x => x.ToString("x2")));
        }
    }
}