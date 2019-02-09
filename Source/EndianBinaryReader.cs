using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Kermalis.EndianBinaryIO
{
    public class EndianBinaryReader : IDisposable
    {
        public Stream BaseStream { get; private set; }
        public Endianness Endianness { get; set; }
        public EncodingType Encoding { get; set; }
        byte[] buffer;

        bool disposed;

        public EndianBinaryReader(Stream baseStream, Endianness endianness = Endianness.LittleEndian, EncodingType encoding = EncodingType.ASCII)
        {
            if (baseStream == null)
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
        }
        ~EndianBinaryReader()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (BaseStream != null)
                    {
                        BaseStream.Close();
                    }
                }
                buffer = null;
                disposed = true;
            }
        }

        void ReadBytesIntoBuffer(int primitiveCount, int primitiveSize)
        {
            int byteCount = primitiveCount * primitiveSize;
            if (buffer == null || buffer.Length < byteCount)
            {
                buffer = new byte[byteCount];
            }
            BaseStream.Read(buffer, 0, byteCount);
            Utils.Flip(buffer, Endianness, byteCount, primitiveSize);
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

        public bool ReadBoolean(BooleanSize booleanSize)
        {
            switch (booleanSize)
            {
                case BooleanSize.U8:
                    {
                        ReadBytesIntoBuffer(1, 1);
                        return buffer[0] != 0;
                    }
                case BooleanSize.U16:
                    {
                        ReadBytesIntoBuffer(1, 2);
                        return BitConverter.ToInt16(buffer, 0) != 0;
                    }
                case BooleanSize.U32:
                    {
                        ReadBytesIntoBuffer(1, 4);
                        return BitConverter.ToInt32(buffer, 0) != 0;
                    }
                default: throw new ArgumentOutOfRangeException(nameof(booleanSize));
            }
        }
        public bool ReadBoolean(BooleanSize size, long offset)
        {
            BaseStream.Position = offset;
            return ReadBoolean(size);
        }
        public bool[] ReadBooleans(int count, BooleanSize size)
        {
            var array = new bool[count];
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
            return buffer[0];
        }
        public byte ReadByte(long offset)
        {
            BaseStream.Position = offset;
            return ReadByte();
        }
        public byte[] ReadBytes(int count)
        {
            ReadBytesIntoBuffer(count, 1);
            var array = new byte[count];
            Array.Copy(buffer, 0, array, 0, count);
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
            return (sbyte)buffer[0];
        }
        public sbyte ReadSByte(long offset)
        {
            BaseStream.Position = offset;
            return ReadSByte();
        }
        public sbyte[] ReadSBytes(int count)
        {
            ReadBytesIntoBuffer(count, 1);
            var array = new sbyte[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = (sbyte)buffer[i];
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
            return encoding.GetChars(buffer, 0, encodingSize)[0];
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
            return encoding.GetChars(buffer, 0, encodingSize * count);
        }
        public char[] ReadChars(int count, EncodingType encodingType, long offset)
        {
            BaseStream.Position = offset;
            return ReadChars(count, encodingType);
        }
        public string ReadString()
        {
            return ReadString(Encoding);
        }
        public string ReadString(long offset)
        {
            BaseStream.Position = offset;
            return ReadString();
        }
        public string ReadString(EncodingType encodingType)
        {
            string text = "";
            do
            {
                text += ReadChar(encodingType);
            }
            while (!text.EndsWith("\0", StringComparison.Ordinal));
            return text.Remove(text.Length - 1);
        }
        public string ReadString(EncodingType encodingType, long offset)
        {
            BaseStream.Position = offset;
            return ReadString(encodingType);
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
            return BitConverter.ToInt16(buffer, 0);
        }
        public short ReadInt16(long offset)
        {
            BaseStream.Position = offset;
            return ReadInt16();
        }
        public short[] ReadInt16s(int count)
        {
            ReadBytesIntoBuffer(count, 2);
            var array = new short[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = BitConverter.ToInt16(buffer, 2 * i);
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
            return BitConverter.ToUInt16(buffer, 0);
        }
        public ushort ReadUInt16(long offset)
        {
            BaseStream.Position = offset;
            return ReadUInt16();
        }
        public ushort[] ReadUInt16s(int count)
        {
            ReadBytesIntoBuffer(count, 2);
            var array = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = BitConverter.ToUInt16(buffer, 2 * i);
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
            return BitConverter.ToInt32(buffer, 0);
        }
        public int ReadInt32(long offset)
        {
            BaseStream.Position = offset;
            return ReadInt32();
        }
        public int[] ReadInt32s(int count)
        {
            ReadBytesIntoBuffer(count, 4);
            var array = new int[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = BitConverter.ToInt32(buffer, 4 * i);
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
            return BitConverter.ToUInt32(buffer, 0);
        }
        public uint ReadUInt32(long offset)
        {
            BaseStream.Position = offset;
            return ReadUInt32();
        }
        public uint[] ReadUInt32s(int count)
        {
            ReadBytesIntoBuffer(count, 4);
            var array = new uint[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = BitConverter.ToUInt32(buffer, 4 * i);
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
            return BitConverter.ToInt64(buffer, 0);
        }
        public long ReadInt64(long offset)
        {
            BaseStream.Position = offset;
            return ReadInt64();
        }
        public long[] ReadInt64s(int count)
        {
            ReadBytesIntoBuffer(count, 8);
            var array = new long[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = BitConverter.ToInt64(buffer, 8 * i);
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
            return BitConverter.ToUInt64(buffer, 0);
        }
        public ulong ReadUInt64(long offset)
        {
            BaseStream.Position = offset;
            return ReadUInt64();
        }
        public ulong[] ReadUInt64s(int count)
        {
            ReadBytesIntoBuffer(count, 8);
            var array = new ulong[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = BitConverter.ToUInt64(buffer, 8 * i);
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
            return BitConverter.ToSingle(buffer, 0);
        }
        public float ReadSingle(long offset)
        {
            BaseStream.Position = offset;
            return ReadSingle();
        }
        public float[] ReadSingles(int count)
        {
            ReadBytesIntoBuffer(count, 4);
            var array = new float[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = BitConverter.ToSingle(buffer, 4 * i);
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
            return BitConverter.ToDouble(buffer, 0);
        }
        public double ReadDouble(long offset)
        {
            BaseStream.Position = offset;
            return ReadDouble();
        }
        public double[] ReadDoubles(int count)
        {
            ReadBytesIntoBuffer(count, 8);
            var array = new double[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = BitConverter.ToDouble(buffer, 8 * i);
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
            return Utils.BytesToDecimal(buffer, 0);
        }
        public decimal ReadDecimal(long offset)
        {
            BaseStream.Position = offset;
            return ReadDecimal();
        }
        public decimal[] ReadDecimals(int count)
        {
            ReadBytesIntoBuffer(count, 16);
            var array = new decimal[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = Utils.BytesToDecimal(buffer, 16 * i);
            }
            return array;
        }
        public decimal[] ReadDecimals(int count, long offset)
        {
            BaseStream.Position = offset;
            return ReadDecimals(count);
        }

        public T ReadObject<T>()
        {
            return (T)ReadObject(typeof(T));
        }
        public object ReadObject(Type objType)
        {
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
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            Type objType = obj.GetType();
            if (typeof(IBinarySerializable).IsAssignableFrom(objType))
            {
                ((IBinarySerializable)obj).Read(this);
            }
            else
            {
                // Get public non-static fields
                FieldInfo[] fields = objType.GetFields(BindingFlags.Instance | BindingFlags.Public);
                // Check for a StructLayoutAttribute
                bool ordered = objType.StructLayoutAttribute.Value == LayoutKind.Explicit;
                // Store this object's start offset
                long objectStart = BaseStream.Position;

                foreach (FieldInfo fieldInfo in fields)
                {
                    // Fields with an IgnoreAttribute get skipped
                    if (Utils.AttributeValueOrDefault(fieldInfo, typeof(BinaryIgnoreAttribute), false))
                    {
                        continue;
                    }

                    BooleanSize booleanSize = Utils.AttributeValueOrDefault(fieldInfo, typeof(BinaryBooleanSizeAttribute), BooleanSize.U8);
                    EncodingType encodingType = Utils.AttributeValueOrDefault(fieldInfo, typeof(BinaryEncodingAttribute), Encoding);
                    bool nullTerminated = Utils.AttributeValueOrDefault(fieldInfo, typeof(BinaryStringNullTerminatedAttribute), true);

                    int arrayFixedLength = Utils.AttributeValueOrDefault(fieldInfo, typeof(BinaryArrayFixedLengthAttribute), 0);
                    int stringFixedLength = Utils.AttributeValueOrDefault(fieldInfo, typeof(BinaryStringFixedLengthAttribute), 0);
                    string arrayVariableLengthAnchor = Utils.AttributeValueOrDefault(fieldInfo, typeof(BinaryArrayVariableLengthAttribute), string.Empty);
                    int arrayVariableLength = 0;
                    if (!string.IsNullOrEmpty(arrayVariableLengthAnchor))
                    {
                        FieldInfo anchor = objType.GetField(arrayVariableLengthAnchor, BindingFlags.Instance | BindingFlags.Public);
                        if (anchor == null)
                        {
                            throw new MissingMemberException($"A field in \"{objType.FullName}\" has an invalid variable array length anchor.");
                        }
                        else
                        {
                            arrayVariableLength = Convert.ToInt32(anchor.GetValue(obj));
                        }
                    }
                    string stringVariableLengthAnchor = Utils.AttributeValueOrDefault(fieldInfo, typeof(BinaryStringVariableLengthAttribute), string.Empty);
                    int stringVariableLength = 0;
                    if (!string.IsNullOrEmpty(stringVariableLengthAnchor))
                    {
                        FieldInfo anchor = objType.GetField(stringVariableLengthAnchor, BindingFlags.Instance | BindingFlags.Public);
                        if (anchor == null)
                        {
                            throw new MissingMemberException($"A field in \"{objType.FullName}\" has an invalid variable string length anchor.");
                        }
                        else
                        {
                            stringVariableLength = Convert.ToInt32(anchor.GetValue(obj));
                        }
                    }
                    if ((arrayFixedLength > 0 && arrayVariableLength > 0)
                        || (stringFixedLength > 0 && stringVariableLength > 0))
                    {
                        throw new ArgumentException($"A field in \"{objType.FullName}\" has two length attributes.");
                    }
                    // One will be 0 and the other will be the intended length. If both are 0 and the reader attempts to read an array, it will throw in the next block
                    int arrayLength = Math.Max(arrayFixedLength, arrayVariableLength);
                    int stringLength = Math.Max(stringFixedLength, stringVariableLength);
                    if (stringLength > 0)
                    {
                        nullTerminated = false;
                    }
                    // Determine the field's start offset
                    long fieldStart = ordered ? objectStart + Utils.AttributeValueOrDefault(fieldInfo, typeof(FieldOffsetAttribute), 0) : BaseStream.Position;

                    object value = null;
                    Type fieldType = fieldInfo.FieldType;

                    if (fieldType.IsArray)
                    {
                        if (arrayLength < 0)
                        {
                            throw new ArgumentOutOfRangeException($"An array in \"{objType.FullName}\" attempted to be read with an invalid length.");
                        }
                        // Get array type
                        Type elementType = fieldType.GetElementType();
                        if (elementType.IsEnum)
                        {
                            elementType = elementType.GetEnumUnderlyingType();
                        }
                        switch (elementType.Name)
                        {
                            case "Boolean": value = ReadBooleans(arrayLength, booleanSize, fieldStart); break;
                            case "Byte": value = ReadBytes(arrayLength, fieldStart); break;
                            case "SByte": value = ReadSBytes(arrayLength, fieldStart); break;
                            case "Char": value = ReadChars(arrayLength, encodingType, fieldStart); break;
                            case "Int16": value = ReadInt16s(arrayLength, fieldStart); break;
                            case "UInt16": value = ReadUInt16s(arrayLength, fieldStart); break;
                            case "Int32": value = ReadInt32s(arrayLength, fieldStart); break;
                            case "UInt32": value = ReadUInt32s(arrayLength, fieldStart); break;
                            case "Int64": value = ReadInt64s(arrayLength, fieldStart); break;
                            case "UInt64": value = ReadUInt64s(arrayLength, fieldStart); break;
                            case "Single": value = ReadSingles(arrayLength, fieldStart); break;
                            case "Double": value = ReadDoubles(arrayLength, fieldStart); break;
                            case "Decimal": value = ReadDecimals(arrayLength, fieldStart); break;
                            default:
                                {
                                    // Create the array
                                    value = Array.CreateInstance(elementType, arrayLength);
                                    // Set position to the start of the array
                                    BaseStream.Position = fieldStart;
                                    if (typeof(IBinarySerializable).IsAssignableFrom(elementType))
                                    {
                                        for (int i = 0; i < arrayLength; i++)
                                        {
                                            var serializable = (IBinarySerializable)Activator.CreateInstance(elementType);
                                            serializable.Read(this);
                                            ((Array)value).SetValue(serializable, i);
                                        }
                                    }
                                    else if (elementType.Name == "String")
                                    {
                                        for (int i = 0; i < arrayLength; i++)
                                        {
                                            string str;
                                            if (nullTerminated)
                                            {
                                                str = ReadString(encodingType);
                                            }
                                            else
                                            {
                                                str = ReadString(stringLength, encodingType);
                                            }
                                            ((Array)value).SetValue(str, i);
                                        }
                                    }
                                    else // Element is not a supported type so try to read the array's objects
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
                        if (fieldType.IsEnum)
                        {
                            fieldType = fieldType.GetEnumUnderlyingType();
                        }
                        switch (fieldType.Name)
                        {
                            case "Boolean": value = ReadBoolean(booleanSize, fieldStart); break;
                            case "Byte": value = ReadByte(fieldStart); break;
                            case "SByte": value = ReadSByte(fieldStart); break;
                            case "Char": value = ReadChar(encodingType, fieldStart); break;
                            case "Int16": value = ReadInt16(fieldStart); break;
                            case "UInt16": value = ReadUInt16(fieldStart); break;
                            case "Int32": value = ReadInt32(fieldStart); break;
                            case "UInt32": value = ReadUInt32(fieldStart); break;
                            case "Int64": value = ReadInt64(fieldStart); break;
                            case "UInt64": value = ReadUInt64(fieldStart); break;
                            case "Single": value = ReadSingle(fieldStart); break;
                            case "Double": value = ReadDouble(fieldStart); break;
                            case "Decimal": value = ReadDecimal(fieldStart); break;
                            case "String":
                                {
                                    if (nullTerminated)
                                    {
                                        value = ReadString(encodingType, fieldStart);
                                    }
                                    else
                                    {
                                        value = ReadString(stringLength, encodingType, fieldStart);
                                    }
                                    break;
                                }
                            default:
                                {
                                    if (typeof(IBinarySerializable).IsAssignableFrom(fieldType))
                                    {
                                        value = Activator.CreateInstance(fieldType);
                                        BaseStream.Position = fieldStart;
                                        ((IBinarySerializable)value).Read(this);
                                    }
                                    else // Field is not a supported type so try to read the object
                                    {
                                        value = ReadObject(fieldType, fieldStart);
                                    }
                                    break;
                                }
                        }
                    }

                    // Set the value into the object field
                    fieldInfo.SetValue(obj, value);
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
