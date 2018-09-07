using Kermalis.EndianBinaryIO;
using System;
using System.Runtime.InteropServices;

#pragma warning disable CS0649

namespace Kermalis.EndianBinaryTesting
{
    class MyStruct
    {
        // Members
        public byte VersionMajor;
        public short VersionMinor;

        // Member that is ignored when reading and writing
        [BinaryIgnore(true)]
        public double DoNotReadOrWrite = Math.PI;

        // Arrays work as well
        [BinaryFixedLength(16)]
        public uint[] ArrayWith16Elements;

        // Boolean that occupies 4 bytes instead of one
        [BinaryBooleanSize(BooleanSize.U32)]
        public bool LongBool;

        // String encoded in ASCII
        // Reads chars until the stream encounters a '\0'
        // Writing will append a '\0' at the end of the string
        [BinaryStringEncoding(EncodingType.ASCII)]
        [BinaryStringNullTerminated(true)]
        public string NullTerminatedASCIIString;

        // String encoded in UTF-16 that will only read/write 10 chars
        [BinaryStringEncoding(EncodingType.UTF16)]
        [BinaryFixedLength(10)]
        public string UTF16String;
    }

    [StructLayout(LayoutKind.Explicit)]
    class MyExplicitStruct
    {
        [BinaryIgnore(true)]
        [FieldOffset(4)]
        public string IgnoredString = "Everyone ignores me.";

        [FieldOffset(0)]
        public int Int1;
        [FieldOffset(0)]
        public short Short1;
        [FieldOffset(2)]
        public short Short2;
        [FieldOffset(0)]
        public byte Byte1;
        [FieldOffset(1)]
        public byte Byte2;
        [FieldOffset(2)]
        public byte Byte3;
        [FieldOffset(3)]
        public byte Byte4;
    }
}
