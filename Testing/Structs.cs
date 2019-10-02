using Kermalis.EndianBinaryIO;
using System;

namespace Kermalis.EndianBinaryTesting
{
    internal enum ShortSizedEnum : short
    {
        Val1 = 0x40,
        Val2 = 0x800
    }

    internal sealed class MyBasicStruct
    {
        // Properties
        public ShortSizedEnum Type { get; set; }
        public short Version { get; set; }

        // Property that is ignored when reading and writing
        [BinaryIgnore(true)]
        public double DoNotReadOrWrite { get; set; } = Math.PI;

        // Arrays work as well
        [BinaryArrayFixedLength(16)]
        public uint[] ArrayWith16Elements { get; set; }

        // Boolean that occupies 4 bytes instead of one
        [BinaryBooleanSize(BooleanSize.U32)]
        public bool Bool32 { get; set; }

        // String encoded in ASCII
        // Reads chars until the stream encounters a '\0'
        // Writing will append a '\0' at the end of the string
        [BinaryEncoding(EncodingType.ASCII)]
        [BinaryStringNullTerminated(true)]
        public string NullTerminatedASCIIString { get; set; }

        // String encoded in UTF-16 that will only read/write 10 chars
        [BinaryEncoding(EncodingType.UTF16)]
        [BinaryStringFixedLength(10)]
        public string UTF16String { get; set; }
    }

    internal sealed class MyLengthyStruct
    {
        [BinaryArrayFixedLength(3)]
        [BinaryEncoding(EncodingType.ASCII)]
        [BinaryStringNullTerminated(true)]
        public string[] NullTerminatedStringArray { get; set; }

        [BinaryArrayFixedLength(3)]
        [BinaryEncoding(EncodingType.ASCII)]
        [BinaryStringFixedLength(5)]
        public string[] SizedStringArray { get; set; }

        public byte VariableLengthProperty { get; set; }
        [BinaryArrayVariableLength(nameof(VariableLengthProperty))]
        public ShortSizedEnum[] VariableSizedArray { get; set; }
    }
}
