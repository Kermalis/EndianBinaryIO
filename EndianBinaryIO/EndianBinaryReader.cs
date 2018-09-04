using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Kermalis.EndianBinaryIO
{
    public sealed class EndianBinaryReader : EndianBinaryBase
    {
        public EndianBinaryReader(Stream baseStream, Endianness endianness = Endianness.LittleEndian, EncodingType encoding = EncodingType.ASCII)
            : base(baseStream, endianness, encoding, true) { }

        internal override void DoNotInheritOutsideOfThisAssembly() { }

        void ReadBytesIntoBuffer(int byteAmount, int primitiveSize)
        {
            if (buffer == null || buffer.Length < byteAmount)
                buffer = new byte[byteAmount];
            BaseStream.Read(buffer, 0, byteAmount);
            Flip(byteAmount, primitiveSize);
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
            ReadBytesIntoBuffer(encodingSize, encodingSize);
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
            ReadBytesIntoBuffer(encodingSize * count, encodingSize);
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
            ReadBytesIntoBuffer(2, 2);
            return BitConverter.ToInt16(buffer, 0);
        }
        public short ReadInt16(long offset)
        {
            BaseStream.Position = offset;
            return ReadInt16();
        }
        public short[] ReadInt16s(int count)
        {
            ReadBytesIntoBuffer(2 * count, 2);
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
            ReadBytesIntoBuffer(2, 2);
            return BitConverter.ToUInt16(buffer, 0);
        }
        public ushort ReadUInt16(long offset)
        {
            BaseStream.Position = offset;
            return ReadUInt16();
        }
        public ushort[] ReadUInt16s(int count)
        {
            ReadBytesIntoBuffer(2 * count, 2);
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
            ReadBytesIntoBuffer(4, 4);
            return BitConverter.ToInt32(buffer, 0);
        }
        public int ReadInt32(long offset)
        {
            BaseStream.Position = offset;
            return ReadInt32();
        }
        public int[] ReadInt32s(int count)
        {
            ReadBytesIntoBuffer(4 * count, 4);
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
            ReadBytesIntoBuffer(4, 4);
            return BitConverter.ToUInt32(buffer, 0);
        }
        public uint ReadUInt32(long offset)
        {
            BaseStream.Position = offset;
            return ReadUInt32();
        }
        public uint[] ReadUInt32s(int count)
        {
            ReadBytesIntoBuffer(4 * count, 4);
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
            ReadBytesIntoBuffer(8, 8);
            return BitConverter.ToInt64(buffer, 0);
        }
        public long ReadInt64(long offset)
        {
            BaseStream.Position = offset;
            return ReadInt64();
        }
        public long[] ReadInt64s(int count)
        {
            ReadBytesIntoBuffer(8 * count, 8);
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
            ReadBytesIntoBuffer(8, 8);
            return BitConverter.ToUInt64(buffer, 0);
        }
        public ulong ReadUInt64(long offset)
        {
            BaseStream.Position = offset;
            return ReadUInt64();
        }
        public ulong[] ReadUInt64s(int count)
        {
            ReadBytesIntoBuffer(8 * count, 8);
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
            ReadBytesIntoBuffer(4, 4);
            return BitConverter.ToSingle(buffer, 0);
        }
        public float ReadSingle(long offset)
        {
            BaseStream.Position = offset;
            return ReadSingle();
        }
        public float[] ReadSingles(int count)
        {
            ReadBytesIntoBuffer(4 * count, 4);
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
            ReadBytesIntoBuffer(8, 8);
            return BitConverter.ToDouble(buffer, 0);
        }
        public double ReadDouble(long offset)
        {
            BaseStream.Position = offset;
            return ReadDouble();
        }
        public double[] ReadDoubles(int count)
        {
            ReadBytesIntoBuffer(8 * count, 8);
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
            ReadBytesIntoBuffer(16, 16);
            return Utils.BytesToDecimal(buffer, 0);
        }
        public decimal ReadDecimal(long offset)
        {
            BaseStream.Position = offset;
            return ReadDecimal();
        }
        public decimal[] ReadDecimals(int count)
        {
            ReadBytesIntoBuffer(16 * count, 16);
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
            Type objType = typeof(T);
            MemberInfo[] members = objType.FindMembers(MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public, null, null);

            if (objType.GetConstructor(new Type[0]) == null)
                throw new ArgumentException("Type does not have a constructor with no parameters. (" + objType.FullName + ")");
            T obj = (T)objType.InvokeMember("", BindingFlags.CreateInstance, null, null, new object[0]);

            foreach (var memberInfo in members)
            {
                // Members with the ignore attribute get skipped
                if (EndianBinaryAttribute.ValueOrDefault(memberInfo, typeof(BinaryIgnoreAttribute), false))
                    continue;

                int fixedLength = EndianBinaryAttribute.ValueOrDefault(memberInfo, typeof(BinaryFixedLengthAttribute), 0);
                BooleanSize booleanSize = EndianBinaryAttribute.ValueOrDefault(memberInfo, typeof(BinaryBooleanSizeAttribute), BooleanSize.U8);
                EncodingType encodingType = EndianBinaryAttribute.ValueOrDefault(memberInfo, typeof(BinaryStringEncodingAttribute), Encoding);
                bool nullTerminated = EndianBinaryAttribute.ValueOrDefault(memberInfo, typeof(BinaryStringNullTerminatedAttribute), false);

                Type memberType;
                object value = null;
                if (memberInfo.MemberType == MemberTypes.Property)
                    memberType = ((PropertyInfo)memberInfo).PropertyType;
                else // Field
                    memberType = ((FieldInfo)memberInfo).FieldType;

                if (memberType.IsArray)
                {
                    Type elementType = memberType.GetElementType();

                    if (elementType.IsPrimitive)
                    {
                        if (elementType.Name != null)
                        {
                            if (supportedTypes.TryGetValue(elementType.Name, out int typeID))
                            {
                                switch (typeID)
                                {
                                    case 0:
                                        {
                                            value = new bool[fixedLength];
                                            switch (booleanSize)
                                            {
                                                case BooleanSize.U8:
                                                    for (int j = 0; j < fixedLength; j++)
                                                        ((bool[])value)[j] = ReadByte() == 1;
                                                    break;
                                                case BooleanSize.U16:
                                                    for (int j = 0; j < fixedLength; j++)
                                                        ((bool[])value)[j] = ReadUInt16() == 1;
                                                    break;
                                                case BooleanSize.U32:
                                                    for (int j = 0; j < fixedLength; j++)
                                                        ((bool[])value)[j] = ReadUInt32() == 1;
                                                    break;
                                                default: throw new ArgumentException("Invalid BooleanSize value.");
                                            }
                                            break;
                                        }
                                    case 1: value = ReadBytes(fixedLength); break;
                                    case 2: value = ReadSBytes(fixedLength); break;
                                    case 3: value = ReadChars(fixedLength, encodingType); break;
                                    case 4: value = ReadInt16s(fixedLength); break;
                                    case 5: value = ReadUInt16s(fixedLength); break;
                                    case 6: value = ReadInt32s(fixedLength); break;
                                    case 7: value = ReadUInt32s(fixedLength); break;
                                    case 8: value = ReadInt64s(fixedLength); break;
                                    case 9: value = ReadUInt64s(fixedLength); break;
                                    case 10: value = ReadSingles(fixedLength); break;
                                    case 11: value = ReadDoubles(fixedLength); break;
                                }
                            }
                            else
                            {
                                throw new NotSupportedException(objType.Name + " is not supported.");
                            }
                        }
                    }
                    else
                    {
                        value = Array.CreateInstance(elementType, fixedLength);
                        if (typeof(IBinarySerializable).IsAssignableFrom(elementType))
                        {
                            if (elementType.GetConstructor(new Type[0]) == null)
                                throw new ArgumentException("A type implementing IBinarySerializable must have a constructor with no parameters. (" + elementType.FullName + ")");
                            for (int j = 0; j < fixedLength; j++)
                            {
                                IBinarySerializable binarySerializable = (IBinarySerializable)elementType.InvokeMember("", BindingFlags.CreateInstance, null, null, new object[0]);
                                binarySerializable.Read(this);
                                ((Array)value).SetValue(binarySerializable, j);
                            }
                        }
                        else // Element is not a supported primitive or IBinarySerializable, so create the array's objects
                        {
                            for (int i = 0; i < fixedLength; i++)
                                ((Array)value).SetValue(elementType.InvokeMember("", BindingFlags.CreateInstance, null, null, new object[] { this }), i);
                        }
                    }
                }
                else // Member is not an array
                {
                    if (memberType.IsEnum)
                        memberType = memberType.GetEnumUnderlyingType();

                    if (memberType.IsPrimitive)
                    {
                        if (memberType.Name != null)
                        {
                            if (supportedTypes.TryGetValue(memberType.Name, out int typeID))
                            {
                                switch (typeID)
                                {
                                    case 0:
                                        switch (booleanSize)
                                        {
                                            case BooleanSize.U8: value = ReadByte() == 1; break;
                                            case BooleanSize.U16: value = ReadUInt16() == 1; break;
                                            case BooleanSize.U32: value = ReadUInt32() == 1; break;
                                            default: throw new ArgumentException("Invalid BooleanSize value.");
                                        }
                                        break;
                                    case 1: value = ReadByte(); break;
                                    case 2: value = ReadSByte(); break;
                                    case 3: value = ReadChar(encodingType); break;
                                    case 4: value = ReadInt16(); break;
                                    case 5: value = ReadUInt16(); break;
                                    case 6: value = ReadInt32(); break;
                                    case 7: value = ReadUInt32(); break;
                                    case 8: value = ReadInt64(); break;
                                    case 9: value = ReadUInt64(); break;
                                    case 10: value = ReadSingle(); break;
                                    case 11: value = ReadDouble(); break;
                                }
                            }
                            else
                            {
                                throw new NotSupportedException(objType.Name + " is not supported.");
                            }
                        }
                    }
                    else if (memberType.Name == "String")
                    {
                        if (nullTerminated)
                            value = ReadString(encodingType);
                        else
                            value = ReadString(fixedLength, encodingType);
                    }
                    else if (typeof(IBinarySerializable).IsAssignableFrom(memberType))
                    {
                        if (memberType.GetConstructor(new Type[0]) == null)
                            throw new ArgumentException("A type implementing IBinarySerializable must have a constructor with no parameters. (" + memberType.FullName + ")");
                        value = memberType.InvokeMember("", BindingFlags.CreateInstance, null, null, new object[0]);
                        ((IBinarySerializable)value).Read(this);
                    }
                    else // Member is not a supported primitive, string, or IBinarySerializable, so create the specified object
                    {
                        value = memberType.InvokeMember("", BindingFlags.CreateInstance, null, null, new object[] { this });
                    }
                }

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
    }
}
