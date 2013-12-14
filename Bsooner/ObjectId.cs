﻿using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Bsooner
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct ObjectId
    {
        /// <summary>
        /// Represents and empty identifier
        /// </summary>
        public static readonly ObjectId Empty = new ObjectId();

        public readonly byte Byte01;
        public readonly byte Byte02;
        public readonly byte Byte03;
        public readonly byte Byte04;
        public readonly byte Byte05;
        public readonly byte Byte06;
        public readonly byte Byte07;
        public readonly byte Byte08;
        public readonly byte Byte09;
        public readonly byte Byte10;
        public readonly byte Byte11;
        public readonly byte Byte12;

        /// <summary>
        /// Return true if the instance is empty, false otherwise.
        /// </summary>
        public bool IsEmpty
        {
            get { return this.Equals(Empty); }
        }

        public ObjectId(byte byte01, byte byte02, byte byte03, byte byte04, byte byte05, byte byte06, byte byte07, byte byte08, byte byte09, byte byte10, byte byte11, byte byte12)
        {
            Byte01 = byte01;
            Byte02 = byte02;
            Byte03 = byte03;
            Byte04 = byte04;
            Byte05 = byte05;
            Byte06 = byte06;
            Byte07 = byte07;
            Byte08 = byte08;
            Byte09 = byte09;
            Byte10 = byte10;
            Byte11 = byte11;
            Byte12 = byte12;
        }

        public ObjectId(string val)
        {
            if (!IsValidOid(val))
            {
                throw new ArgumentException("Invalid oid string");
            }

            Byte01 = val.ToByteFromHex(0);
            Byte02 = val.ToByteFromHex(2);
            Byte03 = val.ToByteFromHex(4);
            Byte04 = val.ToByteFromHex(6);
            Byte05 = val.ToByteFromHex(8);
            Byte06 = val.ToByteFromHex(10);
            Byte07 = val.ToByteFromHex(12);
            Byte08 = val.ToByteFromHex(14);
            Byte09 = val.ToByteFromHex(16);
            Byte10 = val.ToByteFromHex(18);
            Byte11 = val.ToByteFromHex(20);
            Byte12 = val.ToByteFromHex(22);
        }

        public ObjectId(byte[] val)
        {
            Byte01 = val[0];
            Byte02 = val[1];
            Byte03 = val[2];
            Byte04 = val[3];
            Byte05 = val[4];
            Byte06 = val[5];
            Byte07 = val[6];
            Byte08 = val[7];
            Byte09 = val[8];
            Byte10 = val[9];
            Byte11 = val[10];
            Byte12 = val[11];
        }

        public ObjectId(ArraySegment<byte> segment)
        {
            var val = segment.Array;
            Byte01 = val[segment.Offset + 0];
            Byte02 = val[segment.Offset + 1];
            Byte03 = val[segment.Offset + 2];
            Byte04 = val[segment.Offset + 3];
            Byte05 = val[segment.Offset + 4];
            Byte06 = val[segment.Offset + 5];
            Byte07 = val[segment.Offset + 6];
            Byte08 = val[segment.Offset + 7];
            Byte09 = val[segment.Offset + 8];
            Byte10 = val[segment.Offset + 9];
            Byte11 = val[segment.Offset + 10];
            Byte12 = val[segment.Offset + 11];
        }

        public ObjectId(BinaryReader reader)
        {
            Byte01 = reader.ReadByte();
            Byte02 = reader.ReadByte();
            Byte03 = reader.ReadByte();
            Byte04 = reader.ReadByte();
            Byte05 = reader.ReadByte();
            Byte06 = reader.ReadByte();
            Byte07 = reader.ReadByte();
            Byte08 = reader.ReadByte();
            Byte09 = reader.ReadByte();
            Byte10 = reader.ReadByte();
            Byte11 = reader.ReadByte();
            Byte12 = reader.ReadByte();

        }

        public static bool IsValidOid(string oid)
        {
            if (oid.Length != 24)
            {
                return false;

            }
            for (int j = 0; j < oid.Length; j++)
            {
                var symbol = Char.ToLowerInvariant(oid[j]);

                if ((symbol >= '0' && symbol <= '9') || ((symbol >= 'a' && symbol <= 'f')))
                {
                    continue;
                }
                return false;
            }
            return true;
        }


        public byte[] ToBytes()
        {
            return new[] { Byte01, Byte02, Byte03, Byte04, Byte05, Byte06, Byte07, Byte08, Byte09, Byte10, Byte11, Byte12 };
        }

        public override string ToString()
        {
            return BitConverter.ToString(ToBytes()).Replace("-", "").ToLower();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is ObjectId))
            {
                return false;
            }
            return Equals((ObjectId)obj);
        }

        /// <summary>
        /// Check if identifier is equal to other one
        /// </summary>
        public bool Equals(ObjectId other)
        {
            return
                Byte01 == other.Byte01 &&
                Byte02 == other.Byte02 &&
                Byte03 == other.Byte03 &&
                Byte04 == other.Byte04 &&
                Byte05 == other.Byte05 &&
                Byte06 == other.Byte06 &&
                Byte07 == other.Byte07 &&
                Byte08 == other.Byte08 &&
                Byte09 == other.Byte09 &&
                Byte10 == other.Byte10 &&
                Byte11 == other.Byte11 &&
                Byte12 == other.Byte12;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Byte01.GetHashCode();
                hashCode = (hashCode * 397) ^ Byte02.GetHashCode();
                hashCode = (hashCode * 397) ^ Byte03.GetHashCode();
                hashCode = (hashCode * 397) ^ Byte04.GetHashCode();
                hashCode = (hashCode * 397) ^ Byte05.GetHashCode();
                hashCode = (hashCode * 397) ^ Byte06.GetHashCode();
                hashCode = (hashCode * 397) ^ Byte07.GetHashCode();
                hashCode = (hashCode * 397) ^ Byte08.GetHashCode();
                hashCode = (hashCode * 397) ^ Byte09.GetHashCode();
                hashCode = (hashCode * 397) ^ Byte10.GetHashCode();
                hashCode = (hashCode * 397) ^ Byte11.GetHashCode();
                hashCode = (hashCode * 397) ^ Byte12.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(ObjectId a, ObjectId b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ObjectId a, ObjectId b)
        {
            return !(a == b);
        }

        public static explicit operator ObjectId(string val)
        {
            return new ObjectId(val);
        }
    }
}