using System;
using System.Text;

namespace Bsooner
{
    public static class BsonDefaults
    {
        public static readonly DateTime UnixEpoch;

        public static readonly UTF8Encoding Encoding;

        public static readonly Encoder Encoder;
        

        /// <summary>
        /// Max chars it UTF-6 Encoding per code point
        /// </summary>
        public const byte MaxCharLength = 6;


        static BsonDefaults()
        {
            UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Encoding = new UTF8Encoding(false, true);
            Encoder = Encoding.GetEncoder();
        }
    }
}