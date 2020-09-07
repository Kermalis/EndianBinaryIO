[![NuGet](https://img.shields.io/nuget/v/EndianBinaryIO.svg)](https://www.nuget.org/packages/EndianBinaryIO) [![downloads](https://img.shields.io/nuget/dt/EndianBinaryIO)](https://www.nuget.org/packages/EndianBinaryIO)

# 📖 EndianBinaryIO

A C# library that can read and write primitives, enums, arrays, and strings to streams using specified endianness, string encoding, and boolean sizes.
Objects can also be read from/written to streams via reflection and attributes.

The IBinarySerializable interface allows an object to be read and written in a customizable fashion.
Also included are attributes that can make reading and writing objects less of a headache.
For example, classes and structs in C# cannot have ignored members when marshalling, but EndianBinaryIO has a BinaryIgnoreAttribute that will ignore properties when reading and writing.

----
## 🚀 Usage:
Add the [EndianBinaryIO](https://www.nuget.org/packages/EndianBinaryIO) NuGet package to your project or download the .dll from [the releases tab](https://github.com/Kermalis/EndianBinaryIO/releases).

----
## Examples:
Assume we have the following definitions:
### C#:
```cs
enum ShortSizedEnum : short
{
    Val1 = 0x40,
    Val2 = 0x800
}

class MyBasicObj
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
```
And assume these are our input bytes (in little endian):
### Input Bytes (Little Endian):
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

0x4B, 0x00, 0x65, 0x00, 0x72, 0x00, 0x6D, 0x00, 0x61, 0x00, 0x6C, 0x00, 0x69, 0x00, 0x73, 0x00, 0x00, 0x00, 0x00, 0x00
```

We can read/write the object manually or automatically (with reflection):
### Reading Manually:
```cs
MyBasicObj obj;
using (var reader = new EndianBinaryReader(stream, endianness: Endianness.LittleEndian, booleanSize: BooleanSize.U32))
{
    obj = new MyBasicObj();

    obj.Type = reader.ReadEnum<ShortSizedEnum>(); // Enum works
    obj.Version = reader.ReadInt16(); // short works

    obj.ArrayWith16Elements = reader.ReadUInt32s(16); // Array works

    obj.Bool32 = reader.ReadBoolean(); // bool32 works

    obj.NullTerminatedASCIIString = reader.ReadStringNullTerminated(EncodingType.ASCII); // Stops reading at null terminator
    obj.UTF16String = reader.ReadString(10, EncodingType.UTF16); // Fixed size (10 chars) utf16
}
```
### Reading Automatically (With Reflection):
```cs
MyBasicObj obj;
using (var reader = new EndianBinaryReader(stream, endianness: Endianness.LittleEndian))
{
    obj = reader.ReadObject<MyBasicObj>();
}
```

### Writing Manually:
```cs
using (var writer = new EndianBinaryWriter(stream, endianness: Endianness.LittleEndian))
{
    var obj = new MyBasicObj
    {
        Type = ShortSizedEnum.Val2,
        Version = 511,

        DoNotReadOrWrite = ByteSizedEnum.Val1,

        ArrayWith16Elements = new uint[16]
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
        },

        Bool32 = false,

        NullTerminatedASCIIString = "EndianBinaryIO",

        UTF16String = "Kermalis"
    };
    
    writer.Write(obj.Type);
    writer.Write(obj.Version);
    writer.Write(obj.ArrayWith16Elements);
    writer.Write(obj.Bool32);
    writer.Write(obj.NullTerminatedASCIIString, true, EncodingType.ASCII);
    writer.Write(obj.UTF16String, 10, EncodingType.UTF16);
}
```
### Writing Automatically (With Reflection):
```cs
using (var writer = new EndianBinaryWriter(stream, endianness: Endianness.LittleEndian, booleanSize: BooleanSize.U32))
{
    var obj = new MyBasicObj
    {
        Type = ShortSizedEnum.Val2,
        Version = 511,

        DoNotReadOrWrite = ByteSizedEnum.Val1,

        ArrayWith16Elements = new uint[16]
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
        },

        Bool32 = false,

        NullTerminatedASCIIString = "EndianBinaryIO",

        UTF16String = "Kermalis"
    };

    writer.Write(obj);
}
```

----
## To Do:
* Documentation

----
## EndianBinaryIOTests Uses:
* [xUnit.net](https://github.com/xunit/xunit)