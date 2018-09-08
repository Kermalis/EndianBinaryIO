# EndianBinaryIO

A C# library that can read and write primitives, enums, arrays, and strings with specified endianness and encoding.

The IBinarySerializable interface allows an object to be read and written in a customizable fashion.
Also included are attributes that can make reading and writing objects less of a headache.
For example, classes and structs in C# cannot have ignored members when marshalling, but EndianBinaryIO has a BinaryIgnoreAttribute that will ignore members when reading and writing.

----
# Example:
### Class:
```cs
    enum ShortSizedEnum : short
    {
        Val1 = 0x40,
        Val2 = 0x800,
    }

    class MyBasicStruct
    {
        // Members
        public ShortSizedEnum Type;
        public short Version;

        // Member that is ignored when reading and writing
        [BinaryIgnore(true)]
        public double DoNotReadOrWrite = Math.PI;

        // Arrays work as well
        [BinaryArrayFixedLength(16)]
        public uint[] ArrayWith16Elements;

        // Boolean that occupies 4 bytes instead of one
        [BinaryBooleanSize(BooleanSize.U32)]
        public bool LongBool;

        // String encoded in ASCII
        // Reads chars until the stream encounters a '\0'
        // Writing will append a '\0' at the end of the string
        [BinaryEncoding(EncodingType.ASCII)]
        [BinaryStringNullTerminated(true)]
        public string NullTerminatedASCIIString;

        // String encoded in UTF-16 that will only read/write 10 chars
        [BinaryEncoding(EncodingType.UTF16)]
        [BinaryStringFixedLength(10)]
        public string UTF16String;
    }
```
### Byte Representation (Little Endian):
```cs
    0x00, 0x08,
    0xFF, 0x01,

    0x00, 0x00, 0x00, 0x00,
    0x01, 0x00, 0x00, 0x00,
    0x02, 0x00, 0x00, 0x00,
    0x03, 0x00, 0x00, 0x00,
    0x04, 0x00, 0x00, 0x00,
    0x05, 0x00, 0x00, 0x00,
    0x06, 0x00, 0x00, 0x00,
    0x07, 0x00, 0x00, 0x00,
    0x08, 0x00, 0x00, 0x00,
    0x09, 0x00, 0x00, 0x00,
    0x0A, 0x00, 0x00, 0x00,
    0x0B, 0x00, 0x00, 0x00,
    0x0C, 0x00, 0x00, 0x00,
    0x0D, 0x00, 0x00, 0x00,
    0x0E, 0x00, 0x00, 0x00,
    0x0F, 0x00, 0x00, 0x00,

    0x00, 0x00, 0x00, 0x00,

    0x45, 0x6E, 0x64, 0x69, 0x61, 0x6E, 0x42, 0x69, 0x6E, 0x61, 0x72, 0x79, 0x49, 0x4F, 0x00,

    0x42, 0x00, 0x69, 0x00, 0x6E, 0x00, 0x61, 0x00, 0x72, 0x00, 0x79, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
```

----
# To Do:
* Something like Marshal.SizeOf()
* Alignment