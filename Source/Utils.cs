using System;
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

        public static unsafe byte[] Int16ToBytes(short value)
        {
            byte[] bytes = new byte[2];
            fixed (byte* b = bytes)
            {
                *(short*)b = value;
            }
            return bytes;
        }
        public static unsafe byte[] Int32ToBytes(int value)
        {
            byte[] bytes = new byte[4];
            fixed (byte* b = bytes)
            {
                *(int*)b = value;
            }
            return bytes;
        }
        public static unsafe byte[] Int64ToBytes(long value)
        {
            byte[] bytes = new byte[8];
            fixed (byte* b = bytes)
            {
                *(long*)b = value;
            }
            return bytes;
        }
        public static unsafe byte[] SingleToBytes(float value)
        {
            byte[] bytes = new byte[4];
            fixed (byte* b = bytes)
            {
                *(float*)b = value;
            }
            return bytes;
        }
        public static unsafe byte[] DoubleToBytes(double value)
        {
            byte[] bytes = new byte[8];
            fixed (byte* b = bytes)
            {
                *(double*)b = value;
            }
            return bytes;
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
        public static unsafe short BytesToInt16(byte[] value, int startIndex)
        {
            fixed (byte* b = &value[startIndex])
            {
                if (SystemEndianness == Endianness.LittleEndian)
                {
                    return (short)((*b) | (*(b + 1) << 8));
                }
                else
                {
                    return (short)((*b << 8) | (*(b + 1)));
                }
            }
        }
        public static unsafe int BytesToInt32(byte[] value, int startIndex)
        {
            fixed (byte* b = &value[startIndex])
            {
                if (SystemEndianness == Endianness.LittleEndian)
                {
                    return (*b) | (*(b + 1) << 8) | (*(b + 2) << 16) | (*(b + 3) << 24);
                }
                else
                {
                    return (*b << 24) | (*(b + 1) << 16) | (*(b + 2) << 8) | (*(b + 3));
                }
            }
        }
        public static unsafe long BytesToInt64(byte[] value, int startIndex)
        {
            fixed (byte* b = &value[startIndex])
            {
                if (SystemEndianness == Endianness.LittleEndian)
                {
                    int i1 = (*b) | (*(b + 1) << 8) | (*(b + 2) << 16) | (*(b + 3) << 24);
                    int i2 = (*(b + 4)) | (*(b + 5) << 8) | (*(b + 6) << 16) | (*(b + 7) << 24);
                    return (uint)i1 | ((long)i2 << 32);
                }
                else
                {
                    int i1 = (*b << 24) | (*(b + 1) << 16) | (*(b + 2) << 8) | (*(b + 3));
                    int i2 = (*(b + 4) << 24) | (*(b + 5) << 16) | (*(b + 6) << 8) | (*(b + 7));
                    return (uint)i2 | ((long)i1 << 32);
                }
            }
        }
        public static unsafe float BytesToSingle(byte[] value, int startIndex)
        {
            int val = BytesToInt32(value, startIndex);
            return *(float*)&val;
        }
        public static unsafe double BytesToDouble(byte[] value, int startIndex)
        {
            long val = BytesToInt64(value, startIndex);
            return *(double*)&val;
        }
        // TODO: https://github.com/Kermalis/EndianBinaryIO/issues/3
        public static decimal BytesToDecimal(byte[] value, int startIndex)
        {
            int i1 = BytesToInt32(value, startIndex);
            int i2 = BytesToInt32(value, startIndex + 4);
            int i3 = BytesToInt32(value, startIndex + 8);
            int i4 = BytesToInt32(value, startIndex + 12);
            return new decimal(new int[] { i1, i2, i3, i4 });
        }

        public static T AttributeValueOrDefault<T>(FieldInfo fieldInfo, Type attributeType, T defaultValue)
        {
            object[] attributes = fieldInfo.GetCustomAttributes(attributeType, true);
            return attributes.Length == 0 ? defaultValue : (T)attributeType.GetProperty("Value").GetValue(attributes[0]);
        }

        public static void FlipPrimitives(byte[] buffer, Endianness targetEndianness, int byteCount, int primitiveSize)
        {
            if (SystemEndianness != targetEndianness)
            {
                for (int i = 0; i < byteCount; i += primitiveSize)
                {
                    int a = i;
                    int b = i + primitiveSize - 1;
                    while (a < b)
                    {
                        byte by = buffer[a];
                        buffer[a] = buffer[b];
                        buffer[b] = by;
                        a++;
                        b--;
                    }
                }
            }
        }
    }
}
