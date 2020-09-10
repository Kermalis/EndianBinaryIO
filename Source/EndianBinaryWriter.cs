using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Kermalis.EndianBinaryIO
{
    public class EndianBinaryWriter : IDisposable
    {
        public Stream BaseStream { get; }
        private Endianness _endianness;
        public Endianness Endianness
        {
            get => _endianness;
            set
            {
                if (value >= Endianness.MAX)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                _endianness = value;
            }
        }
        private EncodingType _encoding;
        public EncodingType Encoding
        {
            get => _encoding;
            set
            {
                if (value >= EncodingType.MAX)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                _encoding = value;
            }
        }
        private BooleanSize _booleanSize;
        public BooleanSize BooleanSize
        {
            get => _booleanSize;
            set
            {
                if (value >= BooleanSize.MAX)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                _booleanSize = value;
            }
        }

        private byte[] _buffer;
        private bool _isDisposed;

        public EndianBinaryWriter(Stream baseStream, Endianness endianness = Endianness.LittleEndian, EncodingType encoding = EncodingType.ASCII, BooleanSize booleanSize = BooleanSize.U8)
        {
            if (baseStream is null)
            {
                throw new ArgumentNullException(nameof(baseStream));
            }
            if (!baseStream.CanWrite)
            {
                throw new ArgumentException(nameof(baseStream));
            }
            BaseStream = baseStream;
            Endianness = endianness;
            Encoding = encoding;
            BooleanSize = booleanSize;
        }
        public void Dispose()
        {
            if (!_isDisposed)
            {
                BaseStream.Dispose();
                _buffer = null;
                _isDisposed = true;
            }
        }

        private void SetBufferSize(int size)
        {
            if (_buffer is null || _buffer.Length < size)
            {
                _buffer = new byte[size];
            }
        }
        private void WriteBytesFromBuffer(int byteCount)
        {
            BaseStream.Write(_buffer, 0, byteCount);
        }

        public void Write(bool value)
        {
            Write(value, BooleanSize);
        }
        public void Write(bool value, long offset)
        {
            BaseStream.Position = offset;
            Write(value, BooleanSize);
        }
        public void Write(bool value, BooleanSize booleanSize)
        {
            switch (booleanSize)
            {
                case BooleanSize.U8:
                {
                    SetBufferSize(1);
                    _buffer[0] = value ? (byte)1 : (byte)0;
                    WriteBytesFromBuffer(1);
                    break;
                }
                case BooleanSize.U16:
                {
                    _buffer = EndianBitConverter.Int16ToBytes(value ? (short)1 : (short)0, Endianness);
                    WriteBytesFromBuffer(2);
                    break;
                }
                case BooleanSize.U32:
                {
                    _buffer = EndianBitConverter.Int32ToBytes(value ? 1 : 0, Endianness);
                    WriteBytesFromBuffer(4);
                    break;
                }
                default: throw new ArgumentOutOfRangeException(nameof(booleanSize));
            }
        }
        public void Write(bool value, BooleanSize booleanSize, long offset)
        {
            BaseStream.Position = offset;
            Write(value, booleanSize);
        }
        public void Write(bool[] value)
        {
            Write(value, 0, value.Length, BooleanSize);
        }
        public void Write(bool[] value, long offset)
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length, BooleanSize);
        }
        public void Write(bool[] value, BooleanSize booleanSize)
        {
            Write(value, 0, value.Length, booleanSize);
        }
        public void Write(bool[] value, BooleanSize booleanSize, long offset)
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length, booleanSize);
        }
        public void Write(bool[] value, int index, int count)
        {
            Write(value, index, count, BooleanSize);
        }
        public void Write(bool[] value, int index, int count, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count, BooleanSize);
        }
        public void Write(bool[] value, int index, int count, BooleanSize booleanSize)
        {
            if (Utils.ValidateArrayIndexAndCount(value, index, count))
            {
                return;
            }
            for (int i = index; i < count; i++)
            {
                Write(value[i], booleanSize);
            }
        }
        public void Write(bool[] value, int index, int count, BooleanSize booleanSize, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count, booleanSize);
        }
        public void Write(byte value)
        {
            SetBufferSize(1);
            _buffer[0] = value;
            WriteBytesFromBuffer(1);
        }
        public void Write(byte value, long offset)
        {
            BaseStream.Position = offset;
            Write(value);
        }
        public void Write(byte[] value)
        {
            Write(value, 0, value.Length);
        }
        public void Write(byte[] value, long offset)
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length);
        }
        public void Write(byte[] value, int index, int count)
        {
            if (Utils.ValidateArrayIndexAndCount(value, index, count))
            {
                return;
            }
            SetBufferSize(count);
            for (int i = 0; i < count; i++)
            {
                _buffer[i] = value[i + index];
            }
            WriteBytesFromBuffer(count);
        }
        public void Write(byte[] value, int index, int count, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count);
        }
        public void Write(sbyte value)
        {
            SetBufferSize(1);
            _buffer[0] = (byte)value;
            WriteBytesFromBuffer(1);
        }
        public void Write(sbyte value, long offset)
        {
            BaseStream.Position = offset;
            Write(value);
        }
        public void Write(sbyte[] value)
        {
            Write(value, 0, value.Length);
        }
        public void Write(sbyte[] value, long offset)
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length);
        }
        public void Write(sbyte[] value, int index, int count)
        {
            if (Utils.ValidateArrayIndexAndCount(value, index, count))
            {
                return;
            }
            SetBufferSize(count);
            for (int i = 0; i < count; i++)
            {
                _buffer[i] = (byte)value[i + index];
            }
            WriteBytesFromBuffer(count);
        }
        public void Write(sbyte[] value, int index, int count, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count);
        }
        public void Write(char value)
        {
            Write(value, Encoding);
        }
        public void Write(char value, long offset)
        {
            BaseStream.Position = offset;
            Write(value, Encoding);
        }
        public void Write(char value, EncodingType encodingType)
        {
            Encoding encoding = Utils.EncodingFromEnum(encodingType);
            _buffer = encoding.GetBytes(new[] { value });
            WriteBytesFromBuffer(_buffer.Length);
        }
        public void Write(char value, EncodingType encodingType, long offset)
        {
            BaseStream.Position = offset;
            Write(value, encodingType);
        }
        public void Write(char[] value)
        {
            Write(value, 0, value.Length, Encoding);
        }
        public void Write(char[] value, long offset)
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length, Encoding);
        }
        public void Write(char[] value, EncodingType encodingType)
        {
            Write(value, 0, value.Length, encodingType);
        }
        public void Write(char[] value, EncodingType encodingType, long offset)
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length, encodingType);
        }
        public void Write(char[] value, int index, int count)
        {
            Write(value, index, count, Encoding);
        }
        public void Write(char[] value, int index, int count, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count, Encoding);
        }
        public void Write(char[] value, int index, int count, EncodingType encodingType)
        {
            if (Utils.ValidateArrayIndexAndCount(value, index, count))
            {
                return;
            }
            Encoding encoding = Utils.EncodingFromEnum(encodingType);
            _buffer = encoding.GetBytes(value, index, count);
            WriteBytesFromBuffer(_buffer.Length);
        }
        public void Write(char[] value, int index, int count, EncodingType encodingType, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count, encodingType);
        }
        public void Write(string value, bool nullTerminated)
        {
            Write(value, nullTerminated, Encoding);
        }
        public void Write(string value, bool nullTerminated, long offset)
        {
            BaseStream.Position = offset;
            Write(value, nullTerminated, Encoding);
        }
        public void Write(string value, bool nullTerminated, EncodingType encodingType)
        {
            Write(value.ToCharArray(), encodingType);
            if (nullTerminated)
            {
                Write('\0', encodingType);
            }
        }
        public void Write(string value, bool nullTerminated, EncodingType encodingType, long offset)
        {
            BaseStream.Position = offset;
            Write(value, nullTerminated, encodingType);
        }
        public void Write(string value, int charCount)
        {
            Write(value, charCount, Encoding);
        }
        public void Write(string value, int charCount, long offset)
        {
            BaseStream.Position = offset;
            Write(value, charCount, Encoding);
        }
        public void Write(string value, int charCount, EncodingType encodingType)
        {
            Utils.TruncateString(value, charCount, out char[] chars);
            Write(chars, encodingType);
        }
        public void Write(string value, int charCount, EncodingType encodingType, long offset)
        {
            BaseStream.Position = offset;
            Write(value, charCount, encodingType);
        }
        public void Write(string[] value, int index, int count, bool nullTerminated)
        {
            Write(value, index, count, nullTerminated, Encoding);
        }
        public void Write(string[] value, int index, int count, bool nullTerminated, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count, nullTerminated, Encoding);
        }
        public void Write(string[] value, int index, int count, bool nullTerminated, EncodingType encodingType)
        {
            if (Utils.ValidateArrayIndexAndCount(value, index, count))
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                Write(value[i + index], nullTerminated, encodingType);
            }
        }
        public void Write(string[] value, int index, int count, bool nullTerminated, EncodingType encodingType, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count, nullTerminated, encodingType);
        }
        public void Write(string[] value, int index, int count, int charCount)
        {
            Write(value, index, count, charCount, Encoding);
        }
        public void Write(string[] value, int index, int count, int charCount, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count, charCount, Encoding);
        }
        public void Write(string[] value, int index, int count, int charCount, EncodingType encodingType)
        {
            if (Utils.ValidateArrayIndexAndCount(value, index, count))
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                Write(value[i + index], charCount, encodingType);
            }
        }
        public void Write(string[] value, int index, int count, int charCount, EncodingType encodingType, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count, charCount, encodingType);
        }
        public void Write(short value)
        {
            _buffer = EndianBitConverter.Int16ToBytes(value, Endianness);
            WriteBytesFromBuffer(2);
        }
        public void Write(short value, long offset)
        {
            BaseStream.Position = offset;
            Write(value);
        }
        public void Write(short[] value)
        {
            Write(value, 0, value.Length);
        }
        public void Write(short[] value, long offset)
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length);
        }
        public void Write(short[] value, int index, int count)
        {
            _buffer = EndianBitConverter.Int16sToBytes(value, index, count, Endianness);
            WriteBytesFromBuffer(count * 2);
        }
        public void Write(short[] value, int index, int count, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count);
        }
        public void Write(ushort value)
        {
            _buffer = EndianBitConverter.Int16ToBytes((short)value, Endianness);
            WriteBytesFromBuffer(2);
        }
        public void Write(ushort value, long offset)
        {
            BaseStream.Position = offset;
            Write(value);
        }
        public void Write(ushort[] value)
        {
            Write(value, 0, value.Length);
        }
        public void Write(ushort[] value, long offset)
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length);
        }
        public void Write(ushort[] value, int index, int count)
        {
            _buffer = EndianBitConverter.Int16sToBytes(value, index, count, Endianness);
            WriteBytesFromBuffer(count * 2);
        }
        public void Write(ushort[] value, int index, int count, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count);
        }
        public void Write(int value)
        {
            _buffer = EndianBitConverter.Int32ToBytes(value, Endianness);
            WriteBytesFromBuffer(4);
        }
        public void Write(int value, long offset)
        {
            BaseStream.Position = offset;
            Write(value);
        }
        public void Write(int[] value)
        {
            Write(value, 0, value.Length);
        }
        public void Write(int[] value, long offset)
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length);
        }
        public void Write(int[] value, int index, int count)
        {
            _buffer = EndianBitConverter.Int32sToBytes(value, index, count, Endianness);
            WriteBytesFromBuffer(count * 4);
        }
        public void Write(int[] value, int index, int count, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count);
        }
        public void Write(uint value)
        {
            _buffer = EndianBitConverter.Int32ToBytes((int)value, Endianness);
            WriteBytesFromBuffer(4);
        }
        public void Write(uint value, long offset)
        {
            BaseStream.Position = offset;
            Write(value);
        }
        public void Write(uint[] value)
        {
            Write(value, 0, value.Length);
        }
        public void Write(uint[] value, long offset)
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length);
        }
        public void Write(uint[] value, int index, int count)
        {
            _buffer = EndianBitConverter.Int32sToBytes(value, index, count, Endianness);
            WriteBytesFromBuffer(count * 4);
        }
        public void Write(uint[] value, int index, int count, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count);
        }
        public void Write(long value)
        {
            _buffer = EndianBitConverter.Int64ToBytes(value, Endianness);
            WriteBytesFromBuffer(8);
        }
        public void Write(long value, long offset)
        {
            BaseStream.Position = offset;
            Write(value);
        }
        public void Write(long[] value)
        {
            Write(value, 0, value.Length);
        }
        public void Write(long[] value, long offset)
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length);
        }
        public void Write(long[] value, int index, int count)
        {
            _buffer = EndianBitConverter.Int64sToBytes(value, index, count, Endianness);
            WriteBytesFromBuffer(count * 8);
        }
        public void Write(long[] value, int index, int count, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count);
        }
        public void Write(ulong value)
        {
            _buffer = EndianBitConverter.Int64ToBytes((long)value, Endianness);
            WriteBytesFromBuffer(8);
        }
        public void Write(ulong value, long offset)
        {
            BaseStream.Position = offset;
            Write(value);
        }
        public void Write(ulong[] value)
        {
            Write(value, 0, value.Length);
        }
        public void Write(ulong[] value, long offset)
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length);
        }
        public void Write(ulong[] value, int index, int count)
        {
            _buffer = EndianBitConverter.Int64sToBytes(value, index, count, Endianness);
            WriteBytesFromBuffer(count * 8);
        }
        public void Write(ulong[] value, int index, int count, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count);
        }
        public void Write(float value)
        {
            _buffer = EndianBitConverter.SingleToBytes(value, Endianness);
            WriteBytesFromBuffer(4);
        }
        public void Write(float value, long offset)
        {
            BaseStream.Position = offset;
            Write(value);
        }
        public void Write(float[] value)
        {
            Write(value, 0, value.Length);
        }
        public void Write(float[] value, long offset)
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length);
        }
        public void Write(float[] value, int index, int count)
        {
            _buffer = EndianBitConverter.SinglesToBytes(value, index, count, Endianness);
            WriteBytesFromBuffer(count * 4);
        }
        public void Write(float[] value, int index, int count, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count);
        }
        public void Write(double value)
        {
            _buffer = EndianBitConverter.DoubleToBytes(value, Endianness);
            WriteBytesFromBuffer(8);
        }
        public void Write(double value, long offset)
        {
            BaseStream.Position = offset;
            Write(value);
        }
        public void Write(double[] value)
        {
            Write(value, 0, value.Length);
        }
        public void Write(double[] value, long offset)
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length);
        }
        public void Write(double[] value, int index, int count)
        {
            _buffer = EndianBitConverter.DoublesToBytes(value, index, count, Endianness);
            WriteBytesFromBuffer(count * 8);
        }
        public void Write(double[] value, int index, int count, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count);
        }
        public void Write(decimal value)
        {
            _buffer = EndianBitConverter.DecimalToBytes(value, Endianness);
            WriteBytesFromBuffer(16);
        }
        public void Write(decimal value, long offset)
        {
            BaseStream.Position = offset;
            Write(value);
        }
        public void Write(decimal[] value)
        {
            Write(value, 0, value.Length);
        }
        public void Write(decimal[] value, long offset)
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length);
        }
        public void Write(decimal[] value, int index, int count)
        {
            _buffer = EndianBitConverter.DecimalsToBytes(value, index, count, Endianness);
            WriteBytesFromBuffer(count * 16);
        }
        public void Write(decimal[] value, int index, int count, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count);
        }

        // #13 - Handle "Enum" abstract type so we get the correct type in that case
        // For example, writer.Write((Enum)Enum.Parse(enumType, value))
        // No "struct" restriction on writes
        public void Write<TEnum>(TEnum value) where TEnum : Enum
        {
            Type underlyingType = Enum.GetUnderlyingType(value.GetType());
            switch (Type.GetTypeCode(underlyingType))
            {
                case TypeCode.Byte: Write(Convert.ToByte(value)); break;
                case TypeCode.SByte: Write(Convert.ToSByte(value)); break;
                case TypeCode.Int16: Write(Convert.ToInt16(value)); break;
                case TypeCode.UInt16: Write(Convert.ToUInt16(value)); break;
                case TypeCode.Int32: Write(Convert.ToInt32(value)); break;
                case TypeCode.UInt32: Write(Convert.ToUInt32(value)); break;
                case TypeCode.Int64: Write(Convert.ToInt64(value)); break;
                case TypeCode.UInt64: Write(Convert.ToUInt64(value)); break;
                default: throw new ArgumentOutOfRangeException(nameof(underlyingType));
            }
        }
        public void Write<TEnum>(TEnum value, long offset) where TEnum : Enum
        {
            BaseStream.Position = offset;
            Write(value);
        }
        public void Write<TEnum>(TEnum[] value) where TEnum : Enum
        {
            Write(value, 0, value.Length);
        }
        public void Write<TEnum>(TEnum[] value, long offset) where TEnum : Enum
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length);
        }
        public void Write<TEnum>(TEnum[] value, int index, int count) where TEnum : Enum
        {
            if (Utils.ValidateArrayIndexAndCount(value, index, count))
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                Write(value[i + index]);
            }
        }
        public void Write<TEnum>(TEnum[] value, int index, int count, long offset) where TEnum : Enum
        {
            BaseStream.Position = offset;
            Write(value, index, count);
        }

        public void Write(DateTime value)
        {
            Write(value.ToBinary());
        }
        public void Write(DateTime value, long offset)
        {
            BaseStream.Position = offset;
            Write(value);
        }
        public void Write(DateTime[] value)
        {
            Write(value, 0, value.Length);
        }
        public void Write(DateTime[] value, long offset)
        {
            BaseStream.Position = offset;
            Write(value, 0, value.Length);
        }
        public void Write(DateTime[] value, int index, int count)
        {
            if (Utils.ValidateArrayIndexAndCount(value, index, count))
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                Write(value[i + index]);
            }
        }
        public void Write(DateTime[] value, int index, int count, long offset)
        {
            BaseStream.Position = offset;
            Write(value, index, count);
        }

        public void Write(IBinarySerializable obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            obj.Write(this);
        }
        public void Write(IBinarySerializable obj, long offset)
        {
            BaseStream.Position = offset;
            Write(obj);
        }
        public void Write(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            if (obj is IBinarySerializable bs)
            {
                bs.Write(this);
                return;
            }

            Type objType = obj.GetType();
            Utils.ThrowIfCannotReadWriteType(objType);

            // Get public non-static properties
            foreach (PropertyInfo propertyInfo in objType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (Utils.AttributeValueOrDefault<BinaryIgnoreAttribute, bool>(propertyInfo, false))
                {
                    continue; // Skip properties with BinaryIgnoreAttribute
                }

                Type propertyType = propertyInfo.PropertyType;
                object value = propertyInfo.GetValue(obj);

                if (propertyType.IsArray)
                {
                    int arrayLength = Utils.GetArrayLength(obj, objType, propertyInfo);
                    if (arrayLength != 0) // Do not need to do anything for length 0
                    {
                        // Get array type
                        Type elementType = propertyType.GetElementType();
                        if (elementType.IsEnum)
                        {
                            elementType = Enum.GetUnderlyingType(elementType);
                        }
                        switch (Type.GetTypeCode(elementType))
                        {
                            case TypeCode.Boolean:
                            {
                                BooleanSize booleanSize = Utils.AttributeValueOrDefault<BinaryBooleanSizeAttribute, BooleanSize>(propertyInfo, BooleanSize);
                                Write((bool[])value, 0, arrayLength, booleanSize);
                                break;
                            }
                            case TypeCode.Byte: Write((byte[])value, 0, arrayLength); break;
                            case TypeCode.SByte: Write((sbyte[])value, 0, arrayLength); break;
                            case TypeCode.Char:
                            {
                                EncodingType encodingType = Utils.AttributeValueOrDefault<BinaryEncodingAttribute, EncodingType>(propertyInfo, Encoding);
                                Write((char[])value, 0, arrayLength, encodingType);
                                break;
                            }
                            case TypeCode.Int16: Write((short[])value, 0, arrayLength); break;
                            case TypeCode.UInt16: Write((ushort[])value, 0, arrayLength); break;
                            case TypeCode.Int32: Write((int[])value, 0, arrayLength); break;
                            case TypeCode.UInt32: Write((uint[])value, 0, arrayLength); break;
                            case TypeCode.Int64: Write((long[])value, 0, arrayLength); break;
                            case TypeCode.UInt64: Write((ulong[])value, 0, arrayLength); break;
                            case TypeCode.Single: Write((float[])value, 0, arrayLength); break;
                            case TypeCode.Double: Write((double[])value, 0, arrayLength); break;
                            case TypeCode.Decimal: Write((decimal[])value, 0, arrayLength); break;
                            case TypeCode.DateTime: Write((DateTime[])value, 0, arrayLength); break;
                            case TypeCode.String:
                            {
                                Utils.GetStringLength(obj, objType, propertyInfo, false, out bool? nullTerminated, out int stringLength);
                                EncodingType encodingType = Utils.AttributeValueOrDefault<BinaryEncodingAttribute, EncodingType>(propertyInfo, Encoding);
                                if (nullTerminated.HasValue)
                                {
                                    Write((string[])value, 0, arrayLength, nullTerminated.Value, encodingType);
                                }
                                else
                                {
                                    Write((string[])value, 0, arrayLength, stringLength, encodingType);
                                }
                                break;
                            }
                            case TypeCode.Object:
                            {
                                if (typeof(IBinarySerializable).IsAssignableFrom(elementType))
                                {
                                    for (int i = 0; i < arrayLength; i++)
                                    {
                                        var serializable = (IBinarySerializable)((Array)value).GetValue(i);
                                        serializable.Write(this);
                                    }
                                }
                                else // Element's type is not supported so try to write the array's objects
                                {
                                    for (int i = 0; i < arrayLength; i++)
                                    {
                                        object elementObj = ((Array)value).GetValue(i);
                                        Write(elementObj);
                                    }
                                }
                                break;
                            }
                            default: throw new ArgumentOutOfRangeException(nameof(elementType));
                        }
                    }
                }
                else
                {
                    if (propertyType.IsEnum)
                    {
                        propertyType = Enum.GetUnderlyingType(propertyType);
                    }
                    switch (Type.GetTypeCode(propertyType))
                    {
                        case TypeCode.Boolean:
                        {
                            BooleanSize booleanSize = Utils.AttributeValueOrDefault<BinaryBooleanSizeAttribute, BooleanSize>(propertyInfo, BooleanSize);
                            Write((bool)value, booleanSize);
                            break;
                        }
                        case TypeCode.Byte: Write((byte)value); break;
                        case TypeCode.SByte: Write((sbyte)value); break;
                        case TypeCode.Char:
                        {
                            EncodingType encodingType = Utils.AttributeValueOrDefault<BinaryEncodingAttribute, EncodingType>(propertyInfo, Encoding);
                            Write((char)value, encodingType);
                            break;
                        }
                        case TypeCode.Int16: Write((short)value); break;
                        case TypeCode.UInt16: Write((ushort)value); break;
                        case TypeCode.Int32: Write((int)value); break;
                        case TypeCode.UInt32: Write((uint)value); break;
                        case TypeCode.Int64: Write((long)value); break;
                        case TypeCode.UInt64: Write((ulong)value); break;
                        case TypeCode.Single: Write((float)value); break;
                        case TypeCode.Double: Write((double)value); break;
                        case TypeCode.Decimal: Write((decimal)value); break;
                        case TypeCode.DateTime: Write((DateTime)value); break;
                        case TypeCode.String:
                        {
                            Utils.GetStringLength(obj, objType, propertyInfo, false, out bool? nullTerminated, out int stringLength);
                            EncodingType encodingType = Utils.AttributeValueOrDefault<BinaryEncodingAttribute, EncodingType>(propertyInfo, Encoding);
                            if (nullTerminated.HasValue)
                            {
                                Write((string)value, nullTerminated.Value, encodingType);
                            }
                            else
                            {
                                Write((string)value, stringLength, encodingType);
                            }
                            break;
                        }
                        case TypeCode.Object:
                        {
                            if (typeof(IBinarySerializable).IsAssignableFrom(propertyType))
                            {
                                ((IBinarySerializable)value).Write(this);
                            }
                            else // property's type is not supported so try to write the object
                            {
                                Write(value);
                            }
                            break;
                        }
                        default: throw new ArgumentOutOfRangeException(nameof(propertyType));
                    }
                }
            }
        }
        public void Write(object obj, long offset)
        {
            BaseStream.Position = offset;
            Write(obj);
        }
    }
}
