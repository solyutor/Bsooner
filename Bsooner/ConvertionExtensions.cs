using System;

namespace Bsooner
{
    public static class ConvertionExtensions
    {
        public static void EnsureValidObjectIdString(this string objectId)
        {
            if (objectId.Length != 24)
            {
                throw new ArgumentException("ObjectId string must be 24 character length. Characters [0-9] [a-f] allowed");

            }
            for (int j = 0; j < objectId.Length; j++)
            {
                var symbol = Char.ToLowerInvariant(objectId[j]);

                if ((symbol >= '0' && symbol <= '9') || ((symbol >= 'a' && symbol <= 'f')))
                {
                    continue;
                }
                throw new ArgumentException("ObjectId string must be 24 character length. Characters [0-9] [a-f] allowed");
            }
        }

        public static byte ToByteFromHex(this string self, int startIndex)
        {
            var firstSymbol = Char.ToLower(self[startIndex]);
            var secondSymbol = Char.ToLower(self[startIndex + 1]);

            byte upper = (byte)(ByteFromHex(firstSymbol) << 4);
            byte lower = ByteFromHex(secondSymbol);

            return (byte)(upper | lower);
        }

        private static byte ByteFromHex(char symbol)
        {
            switch (symbol)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
                case 'a':
                    return 10;
                case 'b':
                    return 11;
                case 'c':
                    return 12;
                case 'd':
                    return 13;
                case 'e':
                    return 14;
                case 'f':
                    return 15;
                default:
                    var errorMessage = string.Format("Expected chars [0-9] or [a-f], but '{0}'", symbol);
                    throw new ArgumentOutOfRangeException("symbol", errorMessage);
            }
        }
    }
}