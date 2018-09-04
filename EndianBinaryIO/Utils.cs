using System;
using System.Text;

namespace EndianBinaryIO
{
    class Utils
    {
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
    }
}
