using System;
using System.Linq;
using System.Reflection;
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
                default: throw new ArgumentOutOfRangeException(nameof(encodingType));
            }
        }
        public static int EncodingSize(Encoding encoding)
        {
            if (encoding == Encoding.UTF32)
            {
                return 4;
            }
            if (encoding == Encoding.Unicode || encoding == Encoding.BigEndianUnicode)
            {
                return 2;
            }
            return 1;
        }

        public static unsafe byte[] DecimalToBytes(decimal value)
        {
            byte[] bytes = new byte[16];
            fixed (byte* b = bytes)
            {
                *(decimal*)b = value;
            }
            return bytes;
        }
        public static decimal BytesToDecimal(byte[] value, int startIndex)
        {
            int i1 = BitConverter.ToInt32(value, startIndex);
            int i2 = BitConverter.ToInt32(value, startIndex + 4);
            int i3 = BitConverter.ToInt32(value, startIndex + 8);
            int i4 = BitConverter.ToInt32(value, startIndex + 12);
            return new decimal(new int[] { i1, i2, i3, i4 });
        }

        public static void TruncateOrNot(string str, int length, ref char[] toArray)
        {
            toArray = new char[length];
            char[] strAsChars = str.ToCharArray().Take(length).ToArray();
            Buffer.BlockCopy(strAsChars, 0, toArray, 0, strAsChars.Length * 2);
        }
        public static T AttributeValueOrDefault<T>(ICustomAttributeProvider obj, Type attributeType, T defaultValue)
        {
            object[] attributes = obj.GetCustomAttributes(attributeType, true);
            if (attributes.Length == 0)
            {
                return defaultValue;
            }
            MemberInfo valueMember = attributeType.GetMember("Value")[0];
            return (T)((PropertyInfo)valueMember).GetValue(attributes[0]);
        }
    }
}
