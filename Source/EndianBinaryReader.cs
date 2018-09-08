using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Kermalis.EndianBinaryIO
{
    public sealed class EndianBinaryReader : EndianBinaryBase
    {
        public EndianBinaryReader(Stream baseStream, Endianness endianness = Endianness.LittleEndian, EncodingType encoding = EncodingType.ASCII)
            : base(baseStream, endianness, encoding, true) { }

        internal override void DoNotInheritOutsideOfThisAssembly() { }

        void ReadBytesIntoBuffer(int primitiveCount, int primitiveSize)
        {
            int byteCount = primitiveCount * primitiveSize;
            if (buffer == null || buffer.Length < byteCount)
                buffer = new byte[byteCount];
            BaseStream.Read(buffer, 0, byteCount);
            Flip(byteCount, primitiveSize);
        }

        public byte PeekByte()
        {
            long pos = BaseStream.Position;
            var b = ReadByte();
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
            var b = ReadBytes(count);
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
            var c = ReadChar();
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
            var c = ReadChar(encodingType);
            BaseStream.Position = pos;
            return c;
        }
        public char PeekChar(EncodingType encodingType, long offset)
        {
            BaseStream.Position = offset;
            return PeekChar(encodingType);
        }

        public bool ReadBoolean(BooleanSize size)
        {
            switch (size)
            {
                case BooleanSize.U8:
                    ReadBytesIntoBuffer(1, 1);
                    return buffer[0] != 0;
                case BooleanSize.U16:
                    ReadBytesIntoBuffer(1, 2);
                    return BitConverter.ToInt16(buffer, 0) != 0;
                case BooleanSize.U32:
                    ReadBytesIntoBuffer(1, 4);
                    return BitConverter.ToInt32(buffer, 0) != 0;
                default: throw new ArgumentException("Invalid BooleanSize.");
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
                array[i] = ReadBoolean(size);
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
                array[i] = (sbyte)buffer[i];
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
                text += ReadChar(encodingType);
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
                array[i] = BitConverter.ToInt16(buffer, 2 * i);
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
                array[i] = BitConverter.ToUInt16(buffer, 2 * i);
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
                array[i] = BitConverter.ToInt32(buffer, 4 * i);
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
                array[i] = BitConverter.ToInt64(buffer, 8 * i);
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
                array[i] = BitConverter.ToUInt64(buffer, 8 * i);
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
                array[i] = BitConverter.ToSingle(buffer, 4 * i);
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
                array[i] = BitConverter.ToDouble(buffer, 8 * i);
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
                array[i] = Utils.BytesToDecimal(buffer, 16 * i);
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
            // Get public non-static fields and properties
            MemberInfo[] members = objType.FindMembers(MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public, null, null);
            // Check for a StructLayoutAttribute
            bool ordered = objType.StructLayoutAttribute.Value == LayoutKind.Explicit;
            // Create the object; will throw if no parameterless constructor is found
            object obj = objType.InvokeMember("", BindingFlags.CreateInstance, null, null, new object[0]);
            // Store this object's start offset
            long objectStart = BaseStream.Position;

            foreach (var memberInfo in members)
            {
                // Members with an IgnoreAttribute get skipped
                if (Utils.AttributeValueOrDefault(memberInfo, typeof(BinaryIgnoreAttribute), false))
                    continue;

                BooleanSize booleanSize = Utils.AttributeValueOrDefault(memberInfo, typeof(BinaryBooleanSizeAttribute), BooleanSize.U8);
                EncodingType encodingType = Utils.AttributeValueOrDefault(memberInfo, typeof(BinaryEncodingAttribute), Encoding);
                bool nullTerminated = Utils.AttributeValueOrDefault(memberInfo, typeof(BinaryStringNullTerminatedAttribute), true);

                int arrayFixedLength = Utils.AttributeValueOrDefault(memberInfo, typeof(BinaryArrayFixedLengthAttribute), 0);
                int stringFixedLength = Utils.AttributeValueOrDefault(memberInfo, typeof(BinaryStringFixedLengthAttribute), 0);
                string arrayVariableLengthMember = Utils.AttributeValueOrDefault(memberInfo, typeof(BinaryArrayVariableLengthAttribute), string.Empty);
                int arrayVariableLength = 0;
                if (!string.IsNullOrEmpty(arrayVariableLengthMember))
                {
                    var matchingAnchors = objType.GetMember(arrayVariableLengthMember, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public);
                    if (matchingAnchors.Length != 1)
                    {
                        throw new MissingMemberException("A member in \"" + objType.FullName + "\" has an invalid variable array length anchor.");
                    }
                    else
                    {
                        var anchor = matchingAnchors[0];
                        if (anchor.MemberType == MemberTypes.Property)
                            arrayVariableLength = Convert.ToInt32(((PropertyInfo)anchor).GetValue(obj, null));
                        else
                            arrayVariableLength = Convert.ToInt32(((FieldInfo)anchor).GetValue(obj));
                    }
                }
                string stringVariableLengthMember = Utils.AttributeValueOrDefault(memberInfo, typeof(BinaryStringVariableLengthAttribute), string.Empty);
                int stringVariableLength = 0;
                if (!string.IsNullOrEmpty(stringVariableLengthMember))
                {
                    var matchingAnchors = objType.GetMember(stringVariableLengthMember, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public);
                    if (matchingAnchors.Length != 1)
                    {
                        throw new MissingMemberException("A member in \"" + objType.FullName + "\" has an invalid variable string length anchor.");
                    }
                    else
                    {
                        var anchor = matchingAnchors[0];
                        if (anchor.MemberType == MemberTypes.Property)
                            stringVariableLength = Convert.ToInt32(((PropertyInfo)anchor).GetValue(obj, null));
                        else
                            stringVariableLength = Convert.ToInt32(((FieldInfo)anchor).GetValue(obj));
                    }
                }
                if ((arrayFixedLength > 0 && arrayVariableLength > 0)
                    || (stringFixedLength > 0 && stringVariableLength > 0))
                    throw new ArgumentException("A member in \"" + objType.FullName + "\" has two length attributes.");
                // One will be 0 and the other will be the intended length. If both are 0 and the reader attempts to read an array, it will throw in the next block
                int arrayLength = Math.Max(arrayFixedLength, arrayVariableLength);
                int stringLength = Math.Max(stringFixedLength, stringVariableLength);
                if (stringLength > 0)
                    nullTerminated = false;

                // Determine member's start offset
                long memberStart = ordered ?
                    objectStart + Utils.AttributeValueOrDefault(memberInfo, typeof(FieldOffsetAttribute), -1) :
                    BaseStream.Position;

                // Get member's type
                Type memberType;
                object value = null;
                if (memberInfo.MemberType == MemberTypes.Property)
                    memberType = ((PropertyInfo)memberInfo).PropertyType;
                else
                    memberType = ((FieldInfo)memberInfo).FieldType;

                if (memberType.IsArray)
                {
                    if (arrayLength < 1)
                        throw new ArgumentOutOfRangeException("An array in \"" + objType.FullName + "\" attempted to be read with an invalid length.");
                    // Get array type
                    Type elementType = memberType.GetElementType();
                    if (elementType.IsEnum)
                        elementType = elementType.GetEnumUnderlyingType();
                    switch (elementType.Name)
                    {
                        case "Boolean": value = ReadBooleans(arrayLength, booleanSize, memberStart); break;
                        case "Byte": value = ReadBytes(arrayLength, memberStart); break;
                        case "SByte": value = ReadSBytes(arrayLength, memberStart); break;
                        case "Char": value = ReadChars(arrayLength, encodingType, memberStart); break;
                        case "Int16": value = ReadInt16s(arrayLength, memberStart); break;
                        case "UInt16": value = ReadUInt16s(arrayLength, memberStart); break;
                        case "Int32": value = ReadInt32s(arrayLength, memberStart); break;
                        case "UInt32": value = ReadUInt32s(arrayLength, memberStart); break;
                        case "Int64": value = ReadInt64s(arrayLength, memberStart); break;
                        case "UInt64": value = ReadUInt64s(arrayLength, memberStart); break;
                        case "Single": value = ReadSingles(arrayLength, memberStart); break;
                        case "Double": value = ReadDoubles(arrayLength, memberStart); break;
                        case "Decimal": value = ReadDecimals(arrayLength, memberStart); break;
                        default:
                            // Create the array
                            value = Array.CreateInstance(elementType, arrayLength);
                            // Set position to the start of the array
                            BaseStream.Position = memberStart;
                            if (typeof(IBinarySerializable).IsAssignableFrom(elementType))
                            {
                                if (elementType.GetConstructor(new Type[0]) == null)
                                    throw new ArgumentException("A type implementing IBinarySerializable must have a constructor with no parameters. (" + elementType.FullName + ")");
                                for (int i = 0; i < arrayLength; i++)
                                {
                                    IBinarySerializable serializable = (IBinarySerializable)elementType.InvokeMember("", BindingFlags.CreateInstance, null, null, new object[0]);
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
                                        str = ReadString(encodingType);
                                    else
                                        str = ReadString(stringLength, encodingType);
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
                else
                {
                    if (memberType.IsEnum)
                        memberType = memberType.GetEnumUnderlyingType();
                    switch (memberType.Name)
                    {
                        case "Boolean": value = ReadBoolean(booleanSize, memberStart); break;
                        case "Byte": value = ReadByte(memberStart); break;
                        case "SByte": value = ReadSByte(memberStart); break;
                        case "Char": value = ReadChar(encodingType, memberStart); break;
                        case "Int16": value = ReadInt16(memberStart); break;
                        case "UInt16": value = ReadUInt16(memberStart); break;
                        case "Int32": value = ReadInt32(memberStart); break;
                        case "UInt32": value = ReadUInt32(memberStart); break;
                        case "Int64": value = ReadInt64(memberStart); break;
                        case "UInt64": value = ReadUInt64(memberStart); break;
                        case "Single": value = ReadSingle(memberStart); break;
                        case "Double": value = ReadDouble(memberStart); break;
                        case "Decimal": value = ReadDecimal(memberStart); break;
                        case "String":
                            if (nullTerminated)
                                value = ReadString(encodingType, memberStart);
                            else
                                value = ReadString(stringLength, encodingType, memberStart);
                            break;
                        default:
                            if (typeof(IBinarySerializable).IsAssignableFrom(memberType))
                            {
                                if (memberType.GetConstructor(new Type[0]) == null)
                                    throw new ArgumentException("A type implementing IBinarySerializable must have a constructor with no parameters. (" + memberType.FullName + ")");
                                value = memberType.InvokeMember("", BindingFlags.CreateInstance, null, null, new object[0]);
                                BaseStream.Position = memberStart;
                                ((IBinarySerializable)value).Read(this);
                            }
                            else // Element is not a supported type so try to read the object
                            {
                                value = ReadObject(memberType, memberStart);
                            }
                            break;
                    }
                }

                // Set the value into the object member
                if (memberInfo.MemberType == MemberTypes.Property)
                    ((PropertyInfo)memberInfo).SetValue(obj, value, null);
                else
                    ((FieldInfo)memberInfo).SetValue(obj, value);
            }

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
    }
}
