using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace EndianBinaryIO
{
    public sealed class EndianBinaryWriter : EndianBinaryReaderWriter
    {
        public EndianBinaryWriter(Stream baseStream) : base(baseStream, false) { }
        public EndianBinaryWriter(Stream baseStream, Endianness endianness) : base(baseStream, endianness, false) { }
        public EndianBinaryWriter(Stream baseStream, Endianness endianness, EncodingType encoding) : base(baseStream, endianness, encoding, false) { }

        internal override void DoNotInheritOutsideOfThisAssembly() { }

        void SetBufferSize(int size)
        {
            if (buffer == null || buffer.Length < size)
                buffer = new byte[size];
        }
        void WriteBytesFromBuffer(int byteAmount, int primitiveSize)
        {
            Flip(byteAmount, primitiveSize);
            BaseStream.Write(buffer, 0, byteAmount);
        }

        public void Write(byte value)
        {
            SetBufferSize(1);
            buffer[0] = value;
            WriteBytesFromBuffer(1, 1);
        }
        public void Write(byte[] value, int offset, int count)
        {
            SetBufferSize(count);
            Array.Copy(value, offset, buffer, 0, count);
            WriteBytesFromBuffer(count, 1);
        }
        public void Write(sbyte value)
        {
            SetBufferSize(1);
            buffer[0] = (byte)value;
            WriteBytesFromBuffer(1, 1);
        }
        public void Write(sbyte[] value, int offset, int count)
        {
            SetBufferSize(count);
            for (int i = 0; i < count; i++)
                buffer[i] = (byte)value[i + offset];
            WriteBytesFromBuffer(count, 1);
        }
        public void Write(char value)
        {
            Write(value, Encoding);
        }
        public void Write(char value, EncodingType encodingType)
        {
            Encoding encoding = Utils.EncodingFromEnum(encodingType);
            int encodingSize = Utils.EncodingSize(encoding);
            SetBufferSize(encodingSize);
            Array.Copy(encoding.GetBytes(new string(value, 1)), 0, buffer, 0, encodingSize);
            WriteBytesFromBuffer(encodingSize, encodingSize);
        }
        public void Write(char[] value)
        {
            Write(value, 0, value.Length, Encoding);
        }
        public void Write(char[] value, EncodingType encodingType)
        {
            Write(value, 0, value.Length, encodingType);
        }
        public void Write(char[] value, int offset, int count)
        {
            Write(value, offset, count, Encoding);
        }
        public void Write(char[] value, int offset, int count, EncodingType encodingType)
        {
            Encoding encoding = Utils.EncodingFromEnum(encodingType);
            int encodingSize = Utils.EncodingSize(encoding);
            SetBufferSize(encodingSize * count);
            Array.Copy(encoding.GetBytes(value, offset, count), 0, buffer, 0, count * encodingSize);
            WriteBytesFromBuffer(encodingSize * count, encodingSize);
        }
        public void Write(string value, bool nullTerminated)
        {
            Write(value, nullTerminated, Encoding);
        }
        public void Write(string value, bool nullTerminated, EncodingType encodingType)
        {
            Write(value.ToCharArray(), 0, value.Length, encodingType);
            if (nullTerminated)
                Write('\0', encodingType);
        }
        public void Write(short value)
        {
            SetBufferSize(2);
            Array.Copy(BitConverter.GetBytes(value), 0, buffer, 0, 2);
            WriteBytesFromBuffer(2, 2);
        }
        public void Write(short[] value, int offset, int count)
        {
            SetBufferSize(2 * count);
            for (int i = 0; i < count; i++)
                Array.Copy(BitConverter.GetBytes(value[i + offset]), 0, buffer, i * 2, 2);
            WriteBytesFromBuffer(2 * count, 2);
        }
        public void Write(ushort value)
        {
            SetBufferSize(2);
            Array.Copy(BitConverter.GetBytes(value), 0, buffer, 0, 2);
            WriteBytesFromBuffer(2, 2);
        }
        public void Write(ushort[] value, int offset, int count)
        {
            SetBufferSize(2 * count);
            for (int i = 0; i < count; i++)
                Array.Copy(BitConverter.GetBytes(value[i + offset]), 0, buffer, i * 2, 2);
            WriteBytesFromBuffer(2 * count, 2);
        }
        public void Write(int value)
        {
            SetBufferSize(4);
            Array.Copy(BitConverter.GetBytes(value), 0, buffer, 0, 4);
            WriteBytesFromBuffer(4, 4);
        }
        public void Write(int[] value, int offset, int count)
        {
            SetBufferSize(4 * count);
            for (int i = 0; i < count; i++)
                Array.Copy(BitConverter.GetBytes(value[i + offset]), 0, buffer, i * 4, 4);
            WriteBytesFromBuffer(4 * count, 4);
        }
        public void Write(uint value)
        {
            SetBufferSize(4);
            Array.Copy(BitConverter.GetBytes(value), 0, buffer, 0, 4);
            WriteBytesFromBuffer(4, 4);
        }
        public void Write(uint[] value, int offset, int count)
        {
            SetBufferSize(4 * count);
            for (int i = 0; i < count; i++)
                Array.Copy(BitConverter.GetBytes(value[i + offset]), 0, buffer, i * 4, 4);
            WriteBytesFromBuffer(4 * count, 4);
        }
        public void Write(long value)
        {
            SetBufferSize(8);
            Array.Copy(BitConverter.GetBytes(value), 0, buffer, 0, 8);
            WriteBytesFromBuffer(8, 8);
        }
        public void Write(long[] value, int offset, int count)
        {
            SetBufferSize(8 * count);
            for (int i = 0; i < count; i++)
                Array.Copy(BitConverter.GetBytes(value[i + offset]), 0, buffer, i * 8, 8);
            WriteBytesFromBuffer(8 * count, 8);
        }
        public void Write(ulong value)
        {
            SetBufferSize(8);
            Array.Copy(BitConverter.GetBytes(value), 0, buffer, 0, 8);
            WriteBytesFromBuffer(8, 8);
        }
        public void Write(ulong[] value, int offset, int count)
        {
            SetBufferSize(8 * count);
            for (int i = 0; i < count; i++)
                Array.Copy(BitConverter.GetBytes(value[i + offset]), 0, buffer, i * 8, 8);
            WriteBytesFromBuffer(8 * count, 8);
        }
        public void Write(float value)
        {
            SetBufferSize(4);
            Array.Copy(BitConverter.GetBytes(value), 0, buffer, 0, 4);
            WriteBytesFromBuffer(4, 4);
        }
        public void Write(float[] value, int offset, int count)
        {
            SetBufferSize(4 * count);
            for (int i = 0; i < count; i++)
                Array.Copy(BitConverter.GetBytes(value[i + offset]), 0, buffer, i * 4, 4);
            WriteBytesFromBuffer(4 * count, 4);
        }
        public void Write(double value)
        {
            SetBufferSize(8);
            Array.Copy(BitConverter.GetBytes(value), 0, buffer, 0, 8);
            WriteBytesFromBuffer(8, 8);
        }
        public void Write(double[] value, int offset, int count)
        {
            SetBufferSize(8 * count);
            for (int i = 0; i < count; i++)
                Array.Copy(BitConverter.GetBytes(value[i + offset]), 0, buffer, i * 8, 8);
            WriteBytesFromBuffer(8 * count, 8);
        }

        public void WriteObject(object obj)
        {
            Type objType = obj.GetType();
            MemberInfo[] members = objType.FindMembers(MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public, null, null);

            foreach (var memberInfo in members)
            {
                // Members with the ignore attribute get skipped
                if (EndianBinaryAttribute.ValueOrDefault(memberInfo, typeof(BinaryIgnoreAttribute), false))
                    continue;

                int fixedLength = EndianBinaryAttribute.ValueOrDefault(memberInfo, typeof(BinaryFixedLengthAttribute), 0);
                BooleanSize booleanSize = EndianBinaryAttribute.ValueOrDefault(memberInfo, typeof(BinaryBooleanSizeAttribute), BooleanSize.U8);
                EncodingType encodingType = EndianBinaryAttribute.ValueOrDefault(memberInfo, typeof(BinaryStringEncodingAttribute), EncodingType.ASCII);
                bool nullTerminated = EndianBinaryAttribute.ValueOrDefault(memberInfo, typeof(BinaryStringNullTerminatedAttribute), false);

                Type memberType;
                object value = null;
                if (memberInfo.MemberType == MemberTypes.Property)
                {
                    memberType = ((PropertyInfo)memberInfo).PropertyType;
                    value = ((PropertyInfo)memberInfo).GetValue(obj, null);
                }
                else // Field
                {
                    memberType = ((FieldInfo)memberInfo).FieldType;
                    value = ((FieldInfo)memberInfo).GetValue(obj);
                }

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
                                        switch (booleanSize)
                                        {
                                            case BooleanSize.U8:
                                                for (int j = 0; j < fixedLength; j++)
                                                    Write(((bool[])value)[j] ? (sbyte)1 : (sbyte)0);
                                                break;
                                            case BooleanSize.U16:
                                                for (int j = 0; j < fixedLength; j++)
                                                    Write(((bool[])value)[j] ? (ushort)1 : (ushort)0);
                                                break;
                                            case BooleanSize.U32:
                                                for (int j = 0; j < fixedLength; j++)
                                                    Write(((bool[])value)[j] ? 1u : 0u);
                                                break;
                                            default: throw new ArgumentException("Invalid BooleanSize value.");
                                        }
                                        break;
                                    case 1: Write((byte[])value, 0, fixedLength); break;
                                    case 2: Write((sbyte[])value, 0, fixedLength); break;
                                    case 3: Write((char[])value, 0, fixedLength, encodingType); break;
                                    case 4: Write((short[])value, 0, fixedLength); break;
                                    case 5: Write((ushort[])value, 0, fixedLength); break;
                                    case 6: Write((int[])value, 0, fixedLength); break;
                                    case 7: Write((uint[])value, 0, fixedLength); break;
                                    case 8: Write((long[])value, 0, fixedLength); break;
                                    case 9: Write((ulong[])value, 0, fixedLength); break;
                                    case 10: Write((float[])value, 0, fixedLength); break;
                                    case 11: Write((double[])value, 0, fixedLength); break;
                                }
                            }
                            else
                            {
                                throw new NotSupportedException(objType.Name + " is not supported.");
                            }
                        }
                    }
                    else // If the element is not a primitive, it should be an IBinarySerializable
                    {
                        if (!typeof(IBinarySerializable).IsAssignableFrom(elementType))
                            throw new ArgumentException("This class is not serializable. (" + elementType.FullName + ")");
                        if (elementType.GetConstructor(new Type[0]) == null)
                            throw new ArgumentException("A type implementing IBinarySerializable must have a constructor with no parameters. (" + elementType.FullName + ")");
                        for (int j = 0; j < fixedLength; j++)
                            ((IBinarySerializable)((Array)value).GetValue(j)).Write(this);
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
                                            case BooleanSize.U8: Write(((bool)value) ? (byte)1 : (byte)0); break;
                                            case BooleanSize.U16: Write(((bool)value) ? (ushort)1 : (ushort)0); break;
                                            case BooleanSize.U32: Write(((bool)value) ? 1u : 0u); break;
                                            default: throw new ArgumentException("Invalid BooleanSize value.");
                                        }
                                        break;
                                    case 1: Write((byte)value); break;
                                    case 2: Write((sbyte)value); break;
                                    case 3: Write((char)value, encodingType); break;
                                    case 4: Write((short)value); break;
                                    case 5: Write((ushort)value); break;
                                    case 6: Write((int)value); break;
                                    case 7: Write((uint)value); break;
                                    case 8: Write((long)value); break;
                                    case 9: Write((ulong)value); break;
                                    case 10: Write((float)value); break;
                                    case 11: Write((double)value); break;
                                }
                            }
                            else
                            {
                                throw new NotSupportedException(objType.Name + " is not supported!");
                            }
                        }
                    }
                    else if (memberType.Name == "String")
                    {
                        if (nullTerminated)
                            Write((string)value, true, encodingType);
                        else
                        {
                            if (((string)value).Length != fixedLength)
                                throw new ArgumentException("String length does not match the intended fixed length.");
                            Write((string)value, false, encodingType);
                        }
                    }
                    else // IBinarySerializable
                    {
                        if (!typeof(IBinarySerializable).IsAssignableFrom(memberType))
                        {
                            throw new ArgumentException("This class is not serializable. (" + memberType.FullName + ")");
                        }
                        if (memberType.GetConstructor(new Type[0]) == null)
                        {
                            throw new ArgumentException("A type implementing IBinarySerializable must have a constructor with no parameters. (" + memberType.FullName + ")");
                        }
                        ((IBinarySerializable)value).Write(this);
                    }
                }
            }
        }
    }
}
