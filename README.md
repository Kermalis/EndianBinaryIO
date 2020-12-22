# 📖 EndianBinaryIO

[![NuGet](https://img.shields.io/nuget/v/EndianBinaryIO.svg)](https://www.nuget.org/packages/EndianBinaryIO)
[![NuGet downloads](https://img.shields.io/nuget/dt/EndianBinaryIO)](https://www.nuget.org/packages/EndianBinaryIO)

A C# library that can read and write primitives, enums, arrays, and strings to streams using specified endianness, string encoding, and boolean sizes.
Objects can also be read from/written to streams via reflection and attributes.

The `IBinarySerializable` interface allows an object to be read and written in a customizable fashion.
Also included are attributes that can make reading and writing objects less of a headache.
For example, classes and structs in C# cannot have ignored members when marshalling, but **EndianBinaryIO** has a `BinaryIgnoreAttribute` that will ignore properties when reading and writing.

There is also an `EndianBitConverter` static class which resembles `System.BitConverter`. With it you can convert to/from data types using arrays rather than streams, all with specific endianness.

----
## 🚀 Usage:
Add the [EndianBinaryIO](https://www.nuget.org/packages/EndianBinaryIO) NuGet package to your project or download the .dll from [the releases tab](https://github.com/Kermalis/EndianBinaryIO/releases).

----
## Examples:
Assume we have the following definitions:
### C#:
```cs
enum ByteSizedEnum : byte
{
    Val1 = 0x20,
    Val2 = 0x80
}
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
    public DateTime Date { get; set; }

    // Property that is ignored when reading and writing
    [BinaryIgnore(true)]
    public ByteSizedEnum DoNotReadOrWrite { get; set; }

    // Arrays work as well
    [BinaryArrayFixedLength(16)]
    public uint[] ArrayWith16Elements { get; set; }

    // Boolean that occupies 4 bytes instead of one
    [BinaryBooleanSize(BooleanSize.U32)]
    public bool Bool32 { get; set; }

    // String encoded in ASCII
    // Reads chars until the stream encounters a '\0'
    // Writing will append a '\0' at the end of the string
    [BinaryEncoding("ASCII")]
    [BinaryStringNullTerminated(true)]
    public string NullTerminatedASCIIString { get; set; }

    // String encoded in UTF16-LE that will only read/write 10 chars
    // The BinaryStringTrimNullTerminatorsAttribute will indicate that every char from the first \0 will be removed from the string. This attribute also works with char arrays
    [BinaryEncoding("UTF-16")]
    [BinaryStringFixedLength(10)]
    [BinaryStringTrimNullTerminators(true)]
    public string UTF16String { get; set; }
}
```
And assume these are our input bytes (in little endian):
### Input Bytes (Little Endian):
```cs
0x00, 0x08, // ShortSizedEnum.Val2
0xFF, 0x01, // (short)511
0x00, 0x00, 0x4A, 0x7A, 0x9E, 0x01, 0xC0, 0x08, // (DateTime)Dec. 30, 1998

0x00, 0x00, 0x00, 0x00, // (uint)0
0x01, 0x00, 0x00, 0x00, // (uint)1
0x02, 0x00, 0x00, 0x00, // (uint)2
0x03, 0x00, 0x00, 0x00, // (uint)3
0x04, 0x00, 0x00, 0x00, // (uint)4
0x05, 0x00, 0x00, 0x00, // (uint)5
0x06, 0x00, 0x00, 0x00, // (uint)6
0x07, 0x00, 0x00, 0x00, // (uint)7
0x08, 0x00, 0x00, 0x00, // (uint)8
0x09, 0x00, 0x00, 0x00, // (uint)9
0x0A, 0x00, 0x00, 0x00, // (uint)10
0x0B, 0x00, 0x00, 0x00, // (uint)11
0x0C, 0x00, 0x00, 0x00, // (uint)12
0x0D, 0x00, 0x00, 0x00, // (uint)13
0x0E, 0x00, 0x00, 0x00, // (uint)14
0x0F, 0x00, 0x00, 0x00, // (uint)15

0x00, 0x00, 0x00, 0x00, // (bool32)false

0x45, 0x6E, 0x64, 0x69, 0x61, 0x6E, 0x42, 0x69, 0x6E, 0x61, 0x72, 0x79, 0x49, 0x4F, 0x00, // (ASCII)"EndianBinaryIO\0"

0x4B, 0x00, 0x65, 0x00, 0x72, 0x00, 0x6D, 0x00, 0x61, 0x00, 0x6C, 0x00, 0x69, 0x00, 0x73, 0x00, 0x00, 0x00, 0x00, 0x00, // (UTF16-LE)"Kermalis\0\0"
```

We can read/write the object manually or automatically (with reflection):
### Reading Manually:
```cs
var reader = new EndianBinaryReader(stream, endianness: Endianness.LittleEndian, booleanSize: BooleanSize.U32);
var obj = new MyBasicObj();

obj.Type = reader.ReadEnum<ShortSizedEnum>(); // Reads the enum type based on the amount of bytes of the enum's underlying type (short/2 in this case)
obj.Version = reader.ReadInt16(); // Reads a 'short' (2 bytes)
obj.Date = reader.ReadDateTime(); // Reads a 'DateTime' (8 bytes)

obj.ArrayWith16Elements = reader.ReadUInt32s(16); // Reads 16 'uint's (4 bytes each)

obj.Bool32 = reader.ReadBoolean(); // Reads a 'bool' (4 bytes in this case, since the reader was initiated with a default of BooleanSize.U32, but there is an overload to pass in one)

obj.NullTerminatedASCIIString = reader.ReadStringNullTerminated(Encoding.ASCII); // Reads ASCII chars until a '\0' is read, then returns a 'string'
obj.UTF16String = reader.ReadString(10, true, Encoding.Unicode); // Reads 10 UTF16-LE chars as a 'string' with the '\0's removed
```
### Reading Automatically (With Reflection):
```cs
var reader = new EndianBinaryReader(stream, endianness: Endianness.LittleEndian);
var obj = reader.ReadObject<MyBasicObj>(); // Create a 'MyBasicObj' and read all properties in order, ignoring any with a 'BinaryIgnoreAttribute'
                                           // Other objects that are properties in this object will also be read in the same way recursively
```

### Writing Manually:
```cs
var obj = new MyBasicObj
{
    Type = ShortSizedEnum.Val2,
    Version = 511,
    Date = new DateTime(1998, 12, 30),

    DoNotReadOrWrite = ByteSizedEnum.Val1,

    ArrayWith16Elements = new uint[16]
    {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
    },

    Bool32 = false,

    NullTerminatedASCIIString = "EndianBinaryIO",
    UTF16String = "Kermalis"
};

var writer = new EndianBinaryWriter(stream, endianness: Endianness.LittleEndian, booleanSize: BooleanSize.U32);
writer.Write(obj.Type); // Writes the enum type based on the amount of bytes of the enum's underlying type (short/2 in this case)
writer.Write(obj.Version); // Writes a 'short' (2 bytes)
writer.Write(obj.Date); // Writes a 'DateTime' (8 bytes)
writer.Write(obj.ArrayWith16Elements); // Writes 16 'uint's (4 bytes each)
writer.Write(obj.Bool32); // Writes a 'bool' (4 bytes in this case, since the reader was initiated with a default of BooleanSize.U32, but there is an overload to pass in one)
writer.Write(obj.NullTerminatedASCIIString, true, Encoding.ASCII); // Writes the chars in the 'string' as ASCII and appends a '\0' at the end
writer.Write(obj.UTF16String, 10, Encoding.Unicode); // Writes 10 UTF16-LE chars as a 'string'. If the string has more than 10 chars, it is truncated; if it has less, it is padded with '\0'
```
### Writing Automatically (With Reflection):
```cs
var obj = new MyBasicObj
{
    Type = ShortSizedEnum.Val2,
    Version = 511,
    Date = new DateTime(1998, 12, 30),

    DoNotReadOrWrite = ByteSizedEnum.Val1,

    ArrayWith16Elements = new uint[16]
    {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
    },

    Bool32 = false,

    NullTerminatedASCIIString = "EndianBinaryIO",
    UTF16String = "Kermalis"
};

var writer = new EndianBinaryWriter(stream, endianness: Endianness.LittleEndian);
writer.Write(obj); // Write all properties in the 'MyBasicObj' in order, ignoring any with a 'BinaryIgnoreAttribute'
                   // Other objects that are properties in this object will also be written in the same way recursively
```

### EndianBitConverter Example:
```cs
byte[] bytes = new byte[] { 0xFF, 0x00, 0x00, 0x00 };
uint value = (uint)EndianBitConverter.BytesToInt32(bytes, 0, Endianness.LittleEndian); // Will return (int)255

value = 128;
bytes = EndianBitConverter.Int32ToBytes((int)value, Endianness.LittleEndian); // Will return (byte[]){ 0x80, 0x00, 0x00, 0x00 }
```

----
## To Do:
* Documentation

----
## EndianBinaryIOTests Uses:
* [xUnit.net](https://github.com/xunit/xunit)