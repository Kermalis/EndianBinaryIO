using System;
using System.Reflection;
using System.Text;

namespace Kermalis.EndianBinaryIO
{
    internal sealed class Utils
    {
        public static Endianness SystemEndianness { get; } = BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;

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
        public static unsafe decimal BytesToDecimal(byte[] value, int startIndex)
        {
            int i1, i2, i3, i4;
            int[] bits;
            fixed (byte* b = &value[startIndex])
            {
                if (SystemEndianness == Endianness.LittleEndian)
                {
                    i1 = (*b) | (*(b + 1) << 8) | (*(b + 2) << 16) | (*(b + 3) << 24);
                    i2 = (*(b + 4)) | (*(b + 5) << 8) | (*(b + 6) << 16) | (*(b + 7) << 24);
                    i3 = (*(b + 8)) | (*(b + 9) << 8) | (*(b + 10) << 16) | (*(b + 11) << 24);
                    i4 = (*(b + 12)) | (*(b + 13) << 8) | (*(b + 14) << 16) | (*(b + 15) << 24);
                    bits = new int[] { i3, i4, i2, i1 };
                }
                else
                {
                    i1 = (*b << 24) | (*(b + 1) << 16) | (*(b + 2) << 8) | (*(b + 3));
                    i2 = (*(b + 4) << 24) | (*(b + 5) << 16) | (*(b + 6) << 8) | (*(b + 7));
                    i3 = (*(b + 8) << 24) | (*(b + 9) << 16) | (*(b + 10) << 8) | (*(b + 11));
                    i4 = (*(b + 12) << 24) | (*(b + 13) << 16) | (*(b + 14) << 8) | (*(b + 15));
                    bits = new int[] { i2, i1, i3, i4 }; // Not tested, as I do not have a big endian system
                }
            }
            return new decimal(bits);
        }

        public static T AttributeValueOrDefault<T>(PropertyInfo propertyInfo, Type attributeType, T defaultValue)
        {
            object[] attributes = propertyInfo.GetCustomAttributes(attributeType, true);
            return attributes.Length == 0 ? defaultValue : (T)attributeType.GetProperty("Value").GetValue(attributes[0]);
        }

        public static void FlipPrimitives(byte[] buffer, Endianness targetEndianness, int byteCount, int primitiveSize)
        {
            if (SystemEndianness == targetEndianness || primitiveSize <= 1)
            {
                return;
            }
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

        public static void ThrowIfCannotReadWriteType(Type type)
        {
            if (type.IsArray || type.IsEnum || type.IsInterface || type.IsPointer || type.IsPrimitive)
            {
                throw new ArgumentException(nameof(type), $"Cannot read/write \"{type.FullName}\" objects.");
            }
        }

        public static int GetArrayLength(object obj, Type objType, PropertyInfo propertyInfo)
        {
            int Validate(int value)
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException($"An array property in \"{objType.FullName}\" has an invalid length attribute ({value})");
                }
                return value;
            }

            int? fixedLength = AttributeValueOrDefault(propertyInfo, typeof(BinaryArrayFixedLengthAttribute), (int?)null);
            string variableLength = AttributeValueOrDefault(propertyInfo, typeof(BinaryArrayVariableLengthAttribute), (string)null);
            if (fixedLength.HasValue)
            {
                if (variableLength != null)
                {
                    throw new ArgumentException($"An array property in \"{objType.FullName}\" has two array length attributes. Only one should be provided.");
                }
                return Validate(fixedLength.Value);
            }
            if (variableLength is null)
            {
                throw new MissingMemberException($"An array property in \"{objType.FullName}\" is missing an array length attribute. One should be provided.");
            }
            PropertyInfo anchor = objType.GetProperty(variableLength, BindingFlags.Instance | BindingFlags.Public);
            if (anchor is null)
            {
                throw new MissingMemberException($"An array property in \"{objType.FullName}\" has an invalid {nameof(BinaryArrayVariableLengthAttribute)} ({variableLength}).");
            }
            return Validate(Convert.ToInt32(anchor.GetValue(obj)));
        }
        public static void GetStringLength(object obj, Type objType, PropertyInfo propertyInfo, bool forReads, out bool? nullTerminated, out int stringLength)
        {
            int Validate(int value)
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException($"A string property in \"{objType.FullName}\" has an invalid length attribute ({value})");
                }
                return value;
            }

            nullTerminated = AttributeValueOrDefault(propertyInfo, typeof(BinaryStringNullTerminatedAttribute), (bool?)null);
            int? fixedLength = AttributeValueOrDefault(propertyInfo, typeof(BinaryStringFixedLengthAttribute), (int?)null);
            string variableLength = AttributeValueOrDefault(propertyInfo, typeof(BinaryStringVariableLengthAttribute), (string)null);
            if (nullTerminated.HasValue)
            {
                if (fixedLength.HasValue || variableLength != null)
                {
                    throw new ArgumentException($"A string property in \"{objType.FullName}\" has a string length attribute and a {nameof(BinaryStringNullTerminatedAttribute)}; cannot use both.");
                }
                if (forReads && !nullTerminated.Value)
                {
                    throw new ArgumentException($"A string property in \"{objType.FullName}\" has a {nameof(BinaryStringNullTerminatedAttribute)} that's set to false." +
                        $" Must use null termination or provide a string length when reading.");
                }
                stringLength = -1;
                return;
            }
            if (fixedLength.HasValue)
            {
                if (variableLength != null)
                {
                    throw new ArgumentException($"A string property in \"{objType.FullName}\" has two string length attributes. Only one should be provided.");
                }
                stringLength = Validate(fixedLength.Value);
                return;
            }
            if (variableLength is null)
            {
                throw new MissingMemberException($"A string property in \"{objType.FullName}\" is missing a string length attribute and has no {nameof(BinaryStringNullTerminatedAttribute)}. One should be provided.");
            }
            PropertyInfo anchor = objType.GetProperty(variableLength, BindingFlags.Instance | BindingFlags.Public);
            if (anchor is null)
            {
                throw new MissingMemberException($"A string property in \"{objType.FullName}\" has an invalid {nameof(BinaryStringVariableLengthAttribute)} ({variableLength}).");
            }
            stringLength = Validate(Convert.ToInt32(anchor.GetValue(obj)));
        }
    }
}
