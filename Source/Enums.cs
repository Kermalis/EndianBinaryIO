namespace Kermalis.EndianBinaryIO
{
    public enum BooleanSize : byte
    {
        U8,
        U16,
        U32,
        MAX
    }

    public enum Endianness : byte
    {
        LittleEndian,
        BigEndian,
        MAX
    }

    public enum EncodingType : byte
    {
        ASCII,
        UTF7,
        UTF8,
        UTF16,
        BigEndianUTF16,
        UTF32,
        MAX
    }
}

