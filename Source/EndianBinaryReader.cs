using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Kermalis.EndianBinaryIO
{
    public class EndianBinaryReader : IDisposable
    {
        public Stream BaseStream { get; }
        public Endianness Endianness { get; set; }
        public EncodingType Encoding { get; set; }
        public BooleanSize BooleanSize { get; set; }

        private byte[] _buffer;
        private bool _isDisposed;

        public EndianBinaryReader(Stream baseStream, Endianness endianness = Endianness.LittleEndian, EncodingType encoding = EncodingType.ASCII, BooleanSize booleanSize = BooleanSize.U8)
        {
            if (baseStream is null)
            {
                throw new ArgumentNullException(nameof(baseStream));
            }
            if (!baseStream.CanRead)
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

        private void ReadBytesIntoBuffer(int primitiveCount, int primitiveSize)
        {
            int byteCount = primitiveCount * primitiveSize;
            if (_buffer == null || _buffer.Length < byteCount)
            {
                _buffer = new byte[byteCount];
            }
            if (BaseStream.Read(_buffer, 0, byteCount) != byteCount)
            {
                throw new EndOfStreamException();
            }
            Utils.FlipPrimitives(_buffer, Endianness, byteCount, primitiveSize);
        }

        public byte PeekByte()
        {
            long pos = BaseStream.Position;
            byte b = ReadByte();
            BaseStream.Position = pos;
            return b;
        }
        public byte PeekByte(long offset)
        {
            BaseStream.Position = offset;
            return PeekByte();
        }
        public byte[] PeekBytes(int count)
        {
            long pos = BaseStream.Position;
            byte[] b = ReadBytes(count);
            BaseStream.Position = pos;
            return b;
        }
        public byte[] PeekBytes(int count, long offset)
        {
            BaseStream.Position = offset;
            return PeekBytes(count);
        }
        public char PeekChar()
        {
            long pos = BaseStream.Position;
            char c = ReadChar();
            BaseStream.Position = pos;
            return c;
        }
        public char PeekChar(long offset)
        {
            BaseStream.Position = offset;
            return PeekChar();
        }
        public char PeekChar(EncodingType encodingType)
        {
            long pos = BaseStream.Position;
            char c = ReadChar(encodingType);
            BaseStream.Position = pos;
            return c;
        }
        public char PeekChar(EncodingType encodingType, long offset)
        {
            BaseStream.Position = offset;
            return PeekChar(encodingType);
        }

        public bool ReadBoolean()
        {
            return ReadBoolean(BooleanSize);
        }
        public bool ReadBoolean(long offset)
        {
            BaseStream.Position = offset;
            return ReadBoolean(BooleanSize);
        }
        public bool ReadBoolean(BooleanSize booleanSize)
        {
            switch (booleanSize)
            {
                case BooleanSize.U8:
                {
                    ReadBytesIntoBuffer(1, 1);
                    return _buffer[0] != 0;
                }
                case BooleanSize.U16:
                {
                    ReadBytesIntoBuffer(1, 2);
                    return Utils.BytesToInt16(_buffer, 0) != 0;
                }
                case BooleanSize.U32:
                {
                    ReadBytesIntoBuffer(1, 4);
                    return Utils.BytesToInt32(_buffer, 0) != 0;
                }
                default: throw new ArgumentOutOfRangeException(nameof(booleanSize));
            }
        }
        public bool ReadBoolean(BooleanSize booleanSize, long offset)
        {
            BaseStream.Position = offset;
            return ReadBoolean(booleanSize);
        }
        public bool[] ReadBooleans(int count)
        {
            return ReadBooleans(count, BooleanSize);
        }
        public bool[] ReadBooleans(int count, long offset)
        {
            BaseStream.Position = offset;
            return ReadBooleans(count, BooleanSize);
        }
        public bool[] ReadBooleans(int count, BooleanSize size)
        {
            bool[] array = new bool[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = ReadBoolean(size);
            }
            return array;
        }
        public bool[] ReadBooleans(int count, BooleanSize size, long offset)
        {
            BaseStream.Position = offset;
            return ReadBooleans(count, size);
        }
        public byte ReadByte()
        {
            ReadBytesIntoBuffer(1, 1);
            return _buffer[0];
        }
        public byte ReadByte(long offset)
        {
            BaseStream.Position = offset;
            return ReadByte();
        }
        public byte[] ReadBytes(int count)
        {
            ReadBytesIntoBuffer(count, 1);
            byte[] array = new byte[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = _buffer[i];
            }
            return array;
        }
        public byte[] ReadBytes(int count, long offset)
        {
            BaseStream.Position = offset;
            return ReadBytes(count);
        }
        public sbyte ReadSByte()
        {
            ReadBytesIntoBuffer(1, 1);
            return (sbyte)_buffer[0];
        }
        public sbyte ReadSByte(long offset)
        {
            BaseStream.Position = offset;
            return ReadSByte();
        }
        public sbyte[] ReadSBytes(int count)
        {
            ReadBytesIntoBuffer(count, 1);
            sbyte[] array = new sbyte[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = (sbyte)_buffer[i];
            }
            return array;
        }
        public sbyte[] ReadSBytes(int count, long offset)
        {
            BaseStream.Position = offset;
            return ReadSBytes(count);
        }
        public char ReadChar()
        {
            return ReadChar(Encoding);
        }
        public char ReadChar(long offset)
        {
            BaseStream.Position = offset;
            return ReadChar();
        }
        public char ReadChar(EncodingType encodingType)
        {
            Encoding encoding = Utils.EncodingFromEnum(encodingType);
            int encodingSize = Utils.EncodingSize(encoding);
            ReadBytesIntoBuffer(1, encodingSize);
            return encoding.GetChars(_buffer, 0, encodingSize)[0];
        }
        public char ReadChar(EncodingType encodingType, long offset)
        {
            BaseStream.Position = offset;
            return ReadChar(encodingType);
        }
        public char[] ReadChars(int count)
        {
            return ReadChars(count, Encoding);
        }
        public char[] ReadChars(int count, long offset)
        {
            BaseStream.Position = offset;
            return ReadChars(count);
        }
        public char[] ReadChars(int count, EncodingType encodingType)
        {
            Encoding encoding = Utils.EncodingFromEnum(encodingType);
            int encodingSize = Utils.EncodingSize(encoding);
            ReadBytesIntoBuffer(count, encodingSize);
            return encoding.GetChars(_buffer, 0, encodingSize * count);
        }
        public char[] ReadChars(int count, EncodingType encodingType, long offset)
        {
            BaseStream.Position = offset;
            return ReadChars(count, encodingType);
        }
        public string ReadStringNullTerminated()
        {
            return ReadStringNullTerminated(Encoding);
        }
        public string ReadStringNullTerminated(long offset)
        {
            BaseStream.Position = offset;
            return ReadStringNullTerminated();
        }
        public string ReadStringNullTerminated(EncodingType encodingType)
        {
            string text = "";
            do
            {
                text += ReadChar(encodingType);
            }
            while (!text.EndsWith("\0", StringComparison.Ordinal));
            return text.Remove(text.Length - 1);
        }
        public string ReadStringNullTerminated(EncodingType encodingType, long offset)
        {
            BaseStream.Position = offset;
            return ReadStringNullTerminated(encodingType);
        }
        public string ReadString(int charCount)
        {
            return ReadString(charCount, Encoding);
        }
        public string ReadString(int charCount, long offset)
        {
            BaseStream.Position = offset;
            return ReadString(charCount);
        }
        public string ReadString(int charCount, EncodingType encodingType)
        {
            return new string(ReadChars(charCount, encodingType));
        }
        public string ReadString(int charCount, EncodingType encodingType, long offset)
        {
            BaseStream.Position = offset;
            return ReadString(charCount, encodingType);
        }
        public short ReadInt16()
        {
            ReadBytesIntoBuffer(1, 2);
            return Utils.BytesToInt16(_buffer, 0);
        }
        public short ReadInt16(long offset)
        {
            BaseStream.Position = offset;
            return ReadInt16();
        }
        public short[] ReadInt16s(int count)
        {
            ReadBytesIntoBuffer(count, 2);
            short[] array = new short[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = Utils.BytesToInt16(_buffer, 2 * i);
            }
            return array;
        }
        public short[] ReadInt16s(int count, long offset)
        {
            BaseStream.Position = offset;
            return ReadInt16s(count);
        }
        public ushort ReadUInt16()
        {
            ReadBytesIntoBuffer(1, 2);
            return (ushort)Utils.BytesToInt16(_buffer, 0);
        }
        public ushort ReadUInt16(long offset)
        {
            BaseStream.Position = offset;
            return ReadUInt16();
        }
        public ushort[] ReadUInt16s(int count)
        {
            ReadBytesIntoBuffer(count, 2);
            ushort[] array = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = (ushort)Utils.BytesToInt16(_buffer, 2 * i);
            }
            return array;
        }
        public ushort[] ReadUInt16s(int count, long offset)
        {
            BaseStream.Position = offset;
            return ReadUInt16s(count);
        }
        public int ReadInt32()
        {
            ReadBytesIntoBuffer(1, 4);
            return Utils.BytesToInt32(_buffer, 0);
        }
        public int ReadInt32(long offset)
        {
            BaseStream.Position = offset;
            return ReadInt32();
        }
        public int[] ReadInt32s(int count)
        {
            ReadBytesIntoBuffer(count, 4);
            int[] array = new int[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = Utils.BytesToInt32(_buffer, 4 * i);
            }
            return array;
        }
        public int[] ReadInt32s(int count, long offset)
        {
            BaseStream.Position = offset;
            return ReadInt32s(count);
        }
        public uint ReadUInt32()
        {
            ReadBytesIntoBuffer(1, 4);
            return (uint)Utils.BytesToInt32(_buffer, 0);
        }
        public uint ReadUInt32(long offset)
        {
            BaseStream.Position = offset;
            return ReadUInt32();
        }
        public uint[] ReadUInt32s(int count)
        {
            ReadBytesIntoBuffer(count, 4);
            uint[] array = new uint[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = (uint)Utils.BytesToInt32(_buffer, 4 * i);
            }
            return array;
        }
        public uint[] ReadUInt32s(int count, long offset)
        {
            BaseStream.Position = offset;
            return ReadUInt32s(count);
        }
        public long ReadInt64()
        {
            ReadBytesIntoBuffer(1, 8);
            return Utils.BytesToInt64(_buffer, 0);
        }
        public long ReadInt64(long offset)
        {
            BaseStream.Position = offset;
            return ReadInt64();
        }
        public long[] ReadInt64s(int count)
        {
            ReadBytesIntoBuffer(count, 8);
            long[] array = new long[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = Utils.BytesToInt64(_buffer, 8 * i);
            }
            return array;
        }
        public long[] ReadInt64s(int count, long offset)
        {
            BaseStream.Position = offset;
            return ReadInt64s(count);
        }
        public ulong ReadUInt64()
        {
            ReadBytesIntoBuffer(1, 8);
            return (ulong)Utils.BytesToInt64(_buffer, 0);
        }
        public ulong ReadUInt64(long offset)
        {
            BaseStream.Position = offset;
            return ReadUInt64();
        }
        public ulong[] ReadUInt64s(int count)
        {
            ReadBytesIntoBuffer(count, 8);
            ulong[] array = new ulong[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = (ulong)Utils.BytesToInt64(_buffer, 8 * i);
            }
            return array;
        }
        public ulong[] ReadUInt64s(int count, long offset)
        {
            BaseStream.Position = offset;
            return ReadUInt64s(count);
        }
        public float ReadSingle()
        {
            ReadBytesIntoBuffer(1, 4);
            return Utils.BytesToSingle(_buffer, 0);
        }
        public float ReadSingle(long offset)
        {
            BaseStream.Position = offset;
            return ReadSingle();
        }
        public float[] ReadSingles(int count)
        {
            ReadBytesIntoBuffer(count, 4);
            float[] array = new float[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = Utils.BytesToSingle(_buffer, 4 * i);
            }
            return array;
        }
        public float[] ReadSingles(int count, long offset)
        {
            BaseStream.Position = offset;
            return ReadSingles(count);
        }
        public double ReadDouble()
        {
            ReadBytesIntoBuffer(1, 8);
            return Utils.BytesToDouble(_buffer, 0);
        }
        public double ReadDouble(long offset)
        {
            BaseStream.Position = offset;
            return ReadDouble();
        }
        public double[] ReadDoubles(int count)
        {
            ReadBytesIntoBuffer(count, 8);
            double[] array = new double[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = Utils.BytesToDouble(_buffer, 8 * i);
            }
            return array;
        }
        public double[] ReadDoubles(int count, long offset)
        {
            BaseStream.Position = offset;
            return ReadDoubles(count);
        }
        public decimal ReadDecimal()
        {
            ReadBytesIntoBuffer(1, 16);
            return Utils.BytesToDecimal(_buffer, 0);
        }
        public decimal ReadDecimal(long offset)
        {
            BaseStream.Position = offset;
            return ReadDecimal();
        }
        public decimal[] ReadDecimals(int count)
        {
            ReadBytesIntoBuffer(count, 16);
            decimal[] array = new decimal[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = Utils.BytesToDecimal(_buffer, 16 * i);
            }
            return array;
        }
        public decimal[] ReadDecimals(int count, long offset)
        {
            BaseStream.Position = offset;
            return ReadDecimals(count);
        }

        public TEnum ReadEnum<TEnum>() where TEnum : struct, Enum
        {
            // Do not allow writing abstract "Enum" because there is no way to know which underlying type to read
            // Yes "struct" restriction on reads
            Type enumType = typeof(TEnum);
            Type underlyingType = Enum.GetUnderlyingType(enumType);
            object value;
            switch (underlyingType.FullName)
            {
                case "System.Byte": value = ReadByte(); break;
                case "System.SByte": value = ReadSByte(); break;
                case "System.Int16": value = ReadInt16(); break;
                case "System.UInt16": value = ReadUInt16(); break;
                case "System.Int32": value = ReadInt32(); break;
                case "System.UInt32": value = ReadUInt32(); break;
                case "System.Int64": value = ReadInt64(); break;
                case "System.UInt64": value = ReadUInt64(); break;
                default: throw new ArgumentOutOfRangeException(nameof(underlyingType));
            }
            return (TEnum)Enum.ToObject(enumType, value);
        }
        public TEnum ReadEnum<TEnum>(long offset) where TEnum : struct, Enum
        {
            BaseStream.Position = offset;
            return ReadEnum<TEnum>();
        }
        public TEnum[] ReadEnums<TEnum>(int count) where TEnum : struct, Enum
        {
            var array = new TEnum[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = ReadEnum<TEnum>();
            }
            return array;
        }
        public TEnum[] ReadEnums<TEnum>(int count, long offset) where TEnum : struct, Enum
        {
            BaseStream.Position = offset;
            return ReadEnums<TEnum>(count);
        }

        public DateTime ReadDateTime()
        {
            return DateTime.FromBinary(ReadInt64());
        }
        public DateTime ReadDateTime(long offset)
        {
            BaseStream.Position = offset;
            return ReadDateTime();
        }
        public DateTime[] ReadDateTimes(int count)
        {
            var array = new DateTime[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = ReadDateTime();
            }
            return array;
        }
        public DateTime[] ReadDateTimes(int count, long offset)
        {
            BaseStream.Position = offset;
            return ReadDateTimes(count);
        }

        public T ReadObject<T>()
        {
            return (T)ReadObject(typeof(T));
        }
        public object ReadObject(Type objType)
        {
            Utils.ThrowIfCannotReadWriteType(objType);
            object obj = Activator.CreateInstance(objType);
            ReadIntoObject(obj);
            return obj;
        }
        public T ReadObject<T>(long offset)
        {
            BaseStream.Position = offset;
            return ReadObject<T>();
        }
        public object ReadObject(Type objType, long offset)
        {
            BaseStream.Position = offset;
            return ReadObject(objType);
        }
        public void ReadIntoObject(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            Type objType = obj.GetType();
            Utils.ThrowIfCannotReadWriteType(objType);
            if (typeof(IBinarySerializable).IsAssignableFrom(objType))
            {
                ((IBinarySerializable)obj).Read(this);
            }
            else
            {
                foreach (PropertyInfo propertyInfo in objType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (!Utils.AttributeValueOrDefault(propertyInfo, typeof(BinaryIgnoreAttribute), false))
                    {
                        BooleanSize booleanSize = Utils.AttributeValueOrDefault(propertyInfo, typeof(BinaryBooleanSizeAttribute), BooleanSize);
                        EncodingType encodingType = Utils.AttributeValueOrDefault(propertyInfo, typeof(BinaryEncodingAttribute), Encoding);
                        bool nullTerminated = Utils.AttributeValueOrDefault(propertyInfo, typeof(BinaryStringNullTerminatedAttribute), true);

                        int arrayFixedLength = Utils.AttributeValueOrDefault(propertyInfo, typeof(BinaryArrayFixedLengthAttribute), 0);
                        int stringFixedLength = Utils.AttributeValueOrDefault(propertyInfo, typeof(BinaryStringFixedLengthAttribute), 0);
                        string arrayVariableLengthAnchor = Utils.AttributeValueOrDefault(propertyInfo, typeof(BinaryArrayVariableLengthAttribute), string.Empty);
                        int arrayVariableLength = 0;
                        if (!string.IsNullOrEmpty(arrayVariableLengthAnchor))
                        {
                            PropertyInfo anchor = objType.GetProperty(arrayVariableLengthAnchor, BindingFlags.Instance | BindingFlags.Public);
                            if (anchor is null)
                            {
                                throw new MissingMemberException($"A property in \"{objType.FullName}\" has an invalid variable array length anchor ({arrayVariableLengthAnchor}).");
                            }
                            else
                            {
                                arrayVariableLength = Convert.ToInt32(anchor.GetValue(obj));
                            }
                        }
                        string stringVariableLengthAnchor = Utils.AttributeValueOrDefault(propertyInfo, typeof(BinaryStringVariableLengthAttribute), string.Empty);
                        int stringVariableLength = 0;
                        if (!string.IsNullOrEmpty(stringVariableLengthAnchor))
                        {
                            PropertyInfo anchor = objType.GetProperty(stringVariableLengthAnchor, BindingFlags.Instance | BindingFlags.Public);
                            if (anchor is null)
                            {
                                throw new MissingMemberException($"A property in \"{objType.FullName}\" has an invalid variable string length anchor ({stringVariableLengthAnchor}).");
                            }
                            else
                            {
                                stringVariableLength = Convert.ToInt32(anchor.GetValue(obj));
                            }
                        }
                        if ((arrayFixedLength > 0 && arrayVariableLength > 0)
                            || (stringFixedLength > 0 && stringVariableLength > 0))
                        {
                            throw new ArgumentException($"A property in \"{objType.FullName}\" has two length attributes.");
                        }
                        // One will be 0 and the other will be the intended length, so it is safe to use Math.Max to get the intended length
                        int arrayLength = Math.Max(arrayFixedLength, arrayVariableLength);
                        int stringLength = Math.Max(stringFixedLength, stringVariableLength);
                        if (stringLength > 0)
                        {
                            nullTerminated = false;
                        }

                        Type propertyType = propertyInfo.PropertyType;
                        object value;

                        if (propertyType.IsArray)
                        {
                            if (arrayLength < 0)
                            {
                                throw new ArgumentOutOfRangeException($"An array in \"{objType.FullName}\" attempted to be read with an invalid length ({arrayLength}).");
                            }
                            // Get array type
                            Type elementType = propertyType.GetElementType();
                            if (elementType.IsEnum)
                            {
                                elementType = Enum.GetUnderlyingType(elementType);
                            }
                            switch (elementType.FullName)
                            {
                                case "System.Boolean": value = ReadBooleans(arrayLength, booleanSize); break;
                                case "System.Byte": value = ReadBytes(arrayLength); break;
                                case "System.SByte": value = ReadSBytes(arrayLength); break;
                                case "System.Char": value = ReadChars(arrayLength, encodingType); break;
                                case "System.Int16": value = ReadInt16s(arrayLength); break;
                                case "System.UInt16": value = ReadUInt16s(arrayLength); break;
                                case "System.Int32": value = ReadInt32s(arrayLength); break;
                                case "System.UInt32": value = ReadUInt32s(arrayLength); break;
                                case "System.Int64": value = ReadInt64s(arrayLength); break;
                                case "System.UInt64": value = ReadUInt64s(arrayLength); break;
                                case "System.Single": value = ReadSingles(arrayLength); break;
                                case "System.Double": value = ReadDoubles(arrayLength); break;
                                case "System.Decimal": value = ReadDecimals(arrayLength); break;
                                case "System.String":
                                {
                                    value = Array.CreateInstance(elementType, arrayLength);
                                    for (int i = 0; i < arrayLength; i++)
                                    {
                                        string str;
                                        if (nullTerminated)
                                        {
                                            str = ReadStringNullTerminated(encodingType);
                                        }
                                        else
                                        {
                                            str = ReadString(stringLength, encodingType);
                                        }
                                        ((Array)value).SetValue(str, i);
                                    }
                                    break;
                                }
                                default:
                                {
                                    value = Array.CreateInstance(elementType, arrayLength);
                                    if (typeof(IBinarySerializable).IsAssignableFrom(elementType))
                                    {
                                        for (int i = 0; i < arrayLength; i++)
                                        {
                                            var serializable = (IBinarySerializable)Activator.CreateInstance(elementType);
                                            serializable.Read(this);
                                            ((Array)value).SetValue(serializable, i);
                                        }
                                    }
                                    else // Element's type is not supported so try to read the array's objects
                                    {
                                        for (int i = 0; i < arrayLength; i++)
                                        {
                                            object elementObj = ReadObject(elementType);
                                            ((Array)value).SetValue(elementObj, i);
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (propertyType.IsEnum)
                            {
                                propertyType = Enum.GetUnderlyingType(propertyType);
                            }
                            switch (propertyType.FullName)
                            {
                                case "System.Boolean": value = ReadBoolean(booleanSize); break;
                                case "System.Byte": value = ReadByte(); break;
                                case "System.SByte": value = ReadSByte(); break;
                                case "System.Char": value = ReadChar(encodingType); break;
                                case "System.Int16": value = ReadInt16(); break;
                                case "System.UInt16": value = ReadUInt16(); break;
                                case "System.Int32": value = ReadInt32(); break;
                                case "System.UInt32": value = ReadUInt32(); break;
                                case "System.Int64": value = ReadInt64(); break;
                                case "System.UInt64": value = ReadUInt64(); break;
                                case "System.Single": value = ReadSingle(); break;
                                case "System.Double": value = ReadDouble(); break;
                                case "System.Decimal": value = ReadDecimal(); break;
                                case "System.String":
                                {
                                    if (nullTerminated)
                                    {
                                        value = ReadStringNullTerminated(encodingType);
                                    }
                                    else
                                    {
                                        value = ReadString(stringLength, encodingType);
                                    }
                                    break;
                                }
                                default:
                                {
                                    if (typeof(IBinarySerializable).IsAssignableFrom(propertyType))
                                    {
                                        value = Activator.CreateInstance(propertyType);
                                        ((IBinarySerializable)value).Read(this);
                                    }
                                    else // The property's type is not supported so try to read the object
                                    {
                                        value = ReadObject(propertyType);
                                    }
                                    break;
                                }
                            }
                        }

                        // Set the value into the property
                        propertyInfo.SetValue(obj, value);
                    }
                }
            }
        }
        public void ReadIntoObject(object obj, long offset)
        {
            BaseStream.Position = offset;
            ReadIntoObject(obj);
        }
    }
}
