namespace Bsooner
{
    public enum BsonType : byte
    {
        None = 0,
        Double = 0x01,
        String = 0x02,
        Document = 0x03,
        Array = 0x04,
        Binary = 0x05,
        ObjectId = 0x07,
        Boolean = 0x08, //"\x00" 	Boolean "false" "\x01" 	Boolean "true"
        UtcDateTime = 0x09, 
        Null = 0x0A,
        Int32 = 0x10,
        Int64 = 0x12
    }
}