namespace Bsooner
{
    public enum BinaryType : byte
    {
        Generic = 0x00,
        Function = 0x01,
        BinaryOld = 0x02,
        UuidOld = 0x03,
        Uuid = 0x04,
        MD5 = 0x05,
        UserDefined = 0x80,
    }
}