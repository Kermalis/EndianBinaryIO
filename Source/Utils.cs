using System;
using System.Text;

namespace Kermalis.EndianBinaryIO
{
    class Utils
    {
        public static Endianness SystemEndianness => BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;

        public static Encoding EncodingFromEnum(EncodingType encodingType)
        {
            switch (encodingType)
            {
                case EncodingType.ASCII: return Encoding.ASCII;
                case EncodingType.UTF7: return Encoding.UTF7;
                case EncodingType.UTF8: return Encoding.UTF8;
                case EncodingType.UTF16: return Encoding.Unicode;
                case EncodingType.BigEndianUTF16: return Encoding.BigEndianUnicode;
                case EncodingType.UTF32: return Encoding.UTF32;
            }
            throw new ArgumentException("Invalid encoding type.");
        }
        public static int EncodingSize(Encoding encoding)
        {
            if (encoding == Encoding.UTF32)
                return 4;
            if (encoding == Encoding.Unicode || encoding == Encoding.BigEndianUnicode)
                return 2;
            else
                return 1;
        }

        public unsafe static byte[] DecimalToBytes(decimal value)
        {
            byte[] bytes = new byte[16];
            fixed (byte* b = bytes)
                *((decimal*)b) = value;
            return bytes;
        }
        public static decimal BytesToDecimal(byte[] value, int startIndex)
        {
            var i1 = BitConverter.ToInt32(value, startIndex);
            var i2 = BitConverter.ToInt32(value, startIndex + 4);
            var i3 = BitConverter.ToInt32(value, startIndex + 8);
            var i4 = BitConverter.ToInt32(value, startIndex + 12);

            return new decimal(new int[] { i1, i2, i3, i4 });
        }
    }
}
