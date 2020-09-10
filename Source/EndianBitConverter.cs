using System;

namespace Kermalis.EndianBinaryIO
{
    public static class EndianBitConverter
    {
        public static Endianness SystemEndianness { get; } = BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;

        public static unsafe byte[] Int16ToBytes(short value, Endianness targetEndianness)
        {
            byte[] bytes = new byte[2];
            fixed (byte* b = bytes)
            {
                *(short*)b = value;
            }
            if (SystemEndianness != targetEndianness)
            {
                FlipPrimitives(bytes, 0, 1, 2);
            }
            return bytes;
        }
        public static unsafe byte[] Int16sToBytes<T>(T[] value, int startIndex, int count, Endianness targetEndianness) where T : IConvertible
        {
            if (Utils.ValidateArrayIndexAndCount(value, startIndex, count))
            {
                return Array.Empty<byte>();
            }
            if (!Utils.ValidateReadArraySize(count, out byte[] array))
            {
                array = new byte[2 * count];
                fixed (byte* b = array)
                {
                    for (int i = 0; i < count; i++)
                    {
                        ((short*)b)[i] = Convert.ToInt16(value[startIndex + i]);
                    }
                }
                if (SystemEndianness != targetEndianness)
                {
                    FlipPrimitives(array, 0, count, 2);
                }
            }
            return array;
        }
        public static unsafe byte[] Int32ToBytes(int value, Endianness targetEndianness)
        {
            byte[] bytes = new byte[4];
            fixed (byte* b = bytes)
            {
                *(int*)b = value;
            }
            if (SystemEndianness != targetEndianness)
            {
                FlipPrimitives(bytes, 0, 1, 4);
            }
            return bytes;
        }
        public static unsafe byte[] Int32sToBytes<T>(T[] value, int startIndex, int count, Endianness targetEndianness) where T : IConvertible
        {
            if (Utils.ValidateArrayIndexAndCount(value, startIndex, count))
            {
                return Array.Empty<byte>();
            }
            if (!Utils.ValidateReadArraySize(count, out byte[] array))
            {
                array = new byte[4 * count];
                fixed (byte* b = array)
                {
                    for (int i = 0; i < count; i++)
                    {
                        ((int*)b)[i] = Convert.ToInt32(value[startIndex + i]);
                    }
                }
                if (SystemEndianness != targetEndianness)
                {
                    FlipPrimitives(array, 0, count, 4);
                }
            }
            return array;
        }
        public static unsafe byte[] Int64ToBytes(long value, Endianness targetEndianness)
        {
            byte[] bytes = new byte[8];
            fixed (byte* b = bytes)
            {
                *(long*)b = value;
            }
            if (SystemEndianness != targetEndianness)
            {
                FlipPrimitives(bytes, 0, 1, 8);
            }
            return bytes;
        }
        public static unsafe byte[] Int64sToBytes<T>(T[] value, int startIndex, int count, Endianness targetEndianness) where T : IConvertible
        {
            if (Utils.ValidateArrayIndexAndCount(value, startIndex, count))
            {
                return Array.Empty<byte>();
            }
            if (!Utils.ValidateReadArraySize(count, out byte[] array))
            {
                array = new byte[8 * count];
                fixed (byte* b = array)
                {
                    for (int i = 0; i < count; i++)
                    {
                        ((long*)b)[i] = Convert.ToInt64(value[startIndex + i]);
                    }
                }
                if (SystemEndianness != targetEndianness)
                {
                    FlipPrimitives(array, 0, count, 8);
                }
            }
            return array;
        }
        public static unsafe byte[] SingleToBytes(float value, Endianness targetEndianness)
        {
            byte[] bytes = new byte[4];
            fixed (byte* b = bytes)
            {
                *(float*)b = value;
            }
            if (SystemEndianness != targetEndianness)
            {
                FlipPrimitives(bytes, 0, 1, 4);
            }
            return bytes;
        }
        public static unsafe byte[] SinglesToBytes<T>(T[] value, int startIndex, int count, Endianness targetEndianness) where T : IConvertible
        {
            if (Utils.ValidateArrayIndexAndCount(value, startIndex, count))
            {
                return Array.Empty<byte>();
            }
            if (!Utils.ValidateReadArraySize(count, out byte[] array))
            {
                array = new byte[4 * count];
                fixed (byte* b = array)
                {
                    for (int i = 0; i < count; i++)
                    {
                        ((float*)b)[i] = Convert.ToSingle(value[startIndex + i]);
                    }
                }
                if (SystemEndianness != targetEndianness)
                {
                    FlipPrimitives(array, 0, count, 4);
                }
            }
            return array;
        }
        public static unsafe byte[] DoubleToBytes(double value, Endianness targetEndianness)
        {
            byte[] bytes = new byte[8];
            fixed (byte* b = bytes)
            {
                *(double*)b = value;
            }
            if (SystemEndianness != targetEndianness)
            {
                FlipPrimitives(bytes, 0, 1, 8);
            }
            return bytes;
        }
        public static unsafe byte[] DoublesToBytes<T>(T[] value, int startIndex, int count, Endianness targetEndianness) where T : IConvertible
        {
            if (Utils.ValidateArrayIndexAndCount(value, startIndex, count))
            {
                return Array.Empty<byte>();
            }
            if (!Utils.ValidateReadArraySize(count, out byte[] array))
            {
                array = new byte[8 * count];
                fixed (byte* b = array)
                {
                    for (int i = 0; i < count; i++)
                    {
                        ((double*)b)[i] = Convert.ToDouble(value[startIndex + i]);
                    }
                }
                if (SystemEndianness != targetEndianness)
                {
                    FlipPrimitives(array, 0, count, 8);
                }
            }
            return array;
        }
        public static unsafe byte[] DecimalToBytes(decimal value, Endianness targetEndianness)
        {
            byte[] bytes = new byte[16];
            fixed (byte* b = bytes)
            {
                *(decimal*)b = value;
            }
            if (SystemEndianness != targetEndianness)
            {
                FlipPrimitives(bytes, 0, 1, 16);
            }
            return bytes;
        }
        public static unsafe byte[] DecimalsToBytes<T>(T[] value, int startIndex, int count, Endianness targetEndianness) where T : IConvertible
        {
            if (Utils.ValidateArrayIndexAndCount(value, startIndex, count))
            {
                return Array.Empty<byte>();
            }
            if (!Utils.ValidateReadArraySize(count, out byte[] array))
            {
                array = new byte[16 * count];
                fixed (byte* b = array)
                {
                    for (int i = 0; i < count; i++)
                    {
                        ((decimal*)b)[i] = Convert.ToDecimal(value[startIndex + i]);
                    }
                }
                if (SystemEndianness != targetEndianness)
                {
                    FlipPrimitives(array, 0, count, 16);
                }
            }
            return array;
        }

        public static unsafe short BytesToInt16(byte[] value, int startIndex, Endianness sourceEndianness)
        {
            if (SystemEndianness != sourceEndianness)
            {
                FlipPrimitives(value, startIndex, 1, 2);
            }
            fixed (byte* b = &value[startIndex])
            {
                return *(short*)b;
            }
        }
        public static unsafe T[] BytesToInt16s<T>(byte[] value, int startIndex, int count, Endianness sourceEndianness) where T : IConvertible
        {
            if (Utils.ValidateArrayIndexAndCount(value, startIndex, count * 2))
            {
                return Array.Empty<T>();
            }
            if (!Utils.ValidateReadArraySize(count, out T[] array))
            {
                if (SystemEndianness != sourceEndianness)
                {
                    FlipPrimitives(value, startIndex, count, 2);
                }
                array = new T[count];
                fixed (byte* b = &value[startIndex])
                {
                    for (int i = 0; i < count; i++)
                    {
                        array[i] = (T)Convert.ChangeType(((short*)b)[i], typeof(T));
                    }
                }
            }
            return array;
        }
        public static unsafe int BytesToInt32(byte[] value, int startIndex, Endianness sourceEndianness)
        {
            if (SystemEndianness != sourceEndianness)
            {
                FlipPrimitives(value, startIndex, 1, 4);
            }
            fixed (byte* b = &value[startIndex])
            {
                return *(int*)b;
            }
        }
        public static unsafe T[] BytesToInt32s<T>(byte[] value, int startIndex, int count, Endianness sourceEndianness) where T : IConvertible
        {
            if (Utils.ValidateArrayIndexAndCount(value, startIndex, count * 4))
            {
                return Array.Empty<T>();
            }
            if (!Utils.ValidateReadArraySize(count, out T[] array))
            {
                if (SystemEndianness != sourceEndianness)
                {
                    FlipPrimitives(value, startIndex, count, 4);
                }
                array = new T[count];
                fixed (byte* b = &value[startIndex])
                {
                    for (int i = 0; i < count; i++)
                    {
                        array[i] = (T)Convert.ChangeType(((int*)b)[i], typeof(T));
                    }
                }
            }
            return array;
        }
        public static unsafe long BytesToInt64(byte[] value, int startIndex, Endianness sourceEndianness)
        {
            if (SystemEndianness != sourceEndianness)
            {
                FlipPrimitives(value, startIndex, 1, 8);
            }
            fixed (byte* b = &value[startIndex])
            {
                return *(long*)b;
            }
        }
        public static unsafe T[] BytesToInt64s<T>(byte[] value, int startIndex, int count, Endianness sourceEndianness) where T : IConvertible
        {
            if (Utils.ValidateArrayIndexAndCount(value, startIndex, count * 8))
            {
                return Array.Empty<T>();
            }
            if (!Utils.ValidateReadArraySize(count, out T[] array))
            {
                if (SystemEndianness != sourceEndianness)
                {
                    FlipPrimitives(value, startIndex, count, 8);
                }
                array = new T[count];
                fixed (byte* b = &value[startIndex])
                {
                    for (int i = 0; i < count; i++)
                    {
                        array[i] = (T)Convert.ChangeType(((long*)b)[i], typeof(T));
                    }
                }
            }
            return array;
        }
        public static unsafe float BytesToSingle(byte[] value, int startIndex, Endianness sourceEndianness)
        {
            if (SystemEndianness != sourceEndianness)
            {
                FlipPrimitives(value, startIndex, 1, 4);
            }
            fixed (byte* b = &value[startIndex])
            {
                return *(float*)b;
            }
        }
        public static unsafe T[] BytesToSingles<T>(byte[] value, int startIndex, int count, Endianness sourceEndianness) where T : IConvertible
        {
            if (Utils.ValidateArrayIndexAndCount(value, startIndex, count * 4))
            {
                return Array.Empty<T>();
            }
            if (!Utils.ValidateReadArraySize(count, out T[] array))
            {
                if (SystemEndianness != sourceEndianness)
                {
                    FlipPrimitives(value, startIndex, count, 4);
                }
                array = new T[count];
                fixed (byte* b = &value[startIndex])
                {
                    for (int i = 0; i < count; i++)
                    {
                        array[i] = (T)Convert.ChangeType(((float*)b)[i], typeof(T));
                    }
                }
            }
            return array;
        }
        public static unsafe double BytesToDouble(byte[] value, int startIndex, Endianness sourceEndianness)
        {
            if (SystemEndianness != sourceEndianness)
            {
                FlipPrimitives(value, startIndex, 1, 8);
            }
            fixed (byte* b = &value[startIndex])
            {
                return *(double*)b;
            }
        }
        public static unsafe T[] BytesToDoubles<T>(byte[] value, int startIndex, int count, Endianness sourceEndianness) where T : IConvertible
        {
            if (Utils.ValidateArrayIndexAndCount(value, startIndex, count * 8))
            {
                return Array.Empty<T>();
            }
            if (!Utils.ValidateReadArraySize(count, out T[] array))
            {
                if (SystemEndianness != sourceEndianness)
                {
                    FlipPrimitives(value, startIndex, count, 8);
                }
                array = new T[count];
                fixed (byte* b = &value[startIndex])
                {
                    for (int i = 0; i < count; i++)
                    {
                        array[i] = (T)Convert.ChangeType(((double*)b)[i], typeof(T));
                    }
                }
            }
            return array;
        }
        public static unsafe decimal BytesToDecimal(byte[] value, int startIndex, Endianness sourceEndianness)
        {
            if (SystemEndianness != sourceEndianness)
            {
                FlipPrimitives(value, startIndex, 1, 16);
            }
            fixed (byte* b = &value[startIndex])
            {
                return *(decimal*)b;
            }
        }
        public static unsafe T[] BytesToDecimals<T>(byte[] value, int startIndex, int count, Endianness sourceEndianness) where T : IConvertible
        {
            if (Utils.ValidateArrayIndexAndCount(value, startIndex, count * 16))
            {
                return Array.Empty<T>();
            }
            if (!Utils.ValidateReadArraySize(count, out T[] array))
            {
                if (SystemEndianness != sourceEndianness)
                {
                    FlipPrimitives(value, startIndex, count, 16);
                }
                array = new T[count];
                fixed (byte* b = &value[startIndex])
                {
                    for (int i = 0; i < count; i++)
                    {
                        array[i] = (T)Convert.ChangeType(((decimal*)b)[i], typeof(T));
                    }
                }
            }
            return array;
        }

        private static void FlipPrimitives(byte[] buffer, int startIndex, int primitiveCount, int primitiveSize)
        {
            int byteCount = primitiveCount * primitiveSize;
            for (int i = startIndex; i < byteCount + startIndex; i += primitiveSize)
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
