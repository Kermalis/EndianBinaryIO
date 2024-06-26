﻿# 📖 EndianBinaryIO

[![NuGet](https://img.shields.io/nuget/v/EndianBinaryIO.svg)](https://www.nuget.org/packages/EndianBinaryIO)
[![NuGet downloads](https://img.shields.io/nuget/dt/EndianBinaryIO)](https://www.nuget.org/packages/EndianBinaryIO)

This .NET library provides a simple API to read/write bytes from/to streams and spans using user-specified endianness.
By default, supported types include primitives, enums, arrays, strings, and some common .NET struct types.
Objects can also be read/written from/to streams via reflection and attributes.
The developer can use the API even if their target behavior or data is not directly supported by using the `IBinarySerializable` interface, inheritting from the reader/writer, or using the manual `Span<T>`/`ReadOnlySpan<T>` methods without streams.
Performance is the focus when not using reflection; no allocations unless absolutely necessary!

The `IBinarySerializable` interface allows an object to be read and written in a customizable fashion during reflection.
Also included are attributes that can make reading and writing objects less of a headache.
For example, classes and structs in C# cannot have ignored members when marshalling, but **EndianBinaryIO** has a `BinaryIgnoreAttribute` that will ignore properties when reading and writing.

The `EndianBinaryPrimitives` static class which resembles `System.Buffers.Binary.BinaryPrimitives` is an API that converts to/from data types using `Span<T>`/`ReadOnlySpan<T>` with specific endianness, rather than streams.

----
## Changelog For v2.1.1
Check the comment on [the release page](https://github.com/Kermalis/EndianBinaryIO/releases/tag/v2.1.0)!

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
	Val2 = 0x80,
}
enum ShortSizedEnum : short
{
	Val1 = 0x40,
	Val2 = 0x800,
}

class MyBasicObj
{
	// Properties
	public ShortSizedEnum Type { get; set; }
	public short Version { get; set; }
	public DateTime Date { get; set; }
	public Int128 Int128 { get; set; }

	// Property that is ignored when reading and writing
	[BinaryIgnore]
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
	[BinaryASCII]
	[BinaryStringNullTerminated]
	public string NullTerminatedASCIIString { get; set; }

	// String encoded in UTF16-LE that will only read/write 10 chars
	// The BinaryStringTrimNullTerminatorsAttribute will indicate that every char from the first \0 will be removed from the string. This attribute also works with char arrays
	[BinaryStringFixedLength(10)]
	[BinaryStringTrimNullTerminators]
	public string UTF16String { get; set; }
}
```
And assume these are our input bytes (in little endian):
### Input Bytes (Little Endian):
```cs
0x00, 0x08, // ShortSizedEnum.Val2
0xFF, 0x01, // (short)511
0x00, 0x00, 0x4A, 0x7A, 0x9E, 0x01, 0xC0, 0x08, // (DateTime)Dec. 30, 1998
0x48, 0x49, 0x80, 0x44, 0x82, 0x44, 0x88, 0xC0, 0x42, 0x24, 0x88, 0x12, 0x44, 0x44, 0x25, 0x24, // (Int128)48,045,707,429,126,174,655,160,174,263,614,327,112

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
obj.Int128 = reader.ReadInt128(); // Reads an 'Int128' (16 bytes)

obj.ArrayWith16Elements = new uint[16];
reader.ReadUInt32s(obj.ArrayWith16Elements); // Reads 16 'uint's (4 bytes each)

obj.Bool32 = reader.ReadBoolean(); // Reads a 'bool' (4 bytes in this case, since the reader's current bool state is BooleanSize.U32)

reader.ASCII = true; // Set the reader's ASCII state to true
obj.NullTerminatedASCIIString = reader.ReadString_NullTerminated(); // Reads ASCII chars until a '\0' is read, then returns a 'string'

reader.ASCII = false; // Set the reader's ASCII state to false (UTF16-LE)
obj.UTF16String = reader.ReadString_Count_TrimNullTerminators(10); // Reads 10 UTF16-LE chars as a 'string' with the '\0's removed
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
	Int128 = Int128.Parse("48,045,707,429,126,174,655,160,174,263,614,327,112", NumberStyles.AllowThousands, NumberFormatInfo.InvariantInfo),

	DoNotReadOrWrite = ByteSizedEnum.Val1,

	ArrayWith16Elements = new uint[16]
	{
		0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
	},

	Bool32 = false,

	NullTerminatedASCIIString = "EndianBinaryIO",
	UTF16String = "Kermalis",
};

var writer = new EndianBinaryWriter(stream, endianness: Endianness.LittleEndian, booleanSize: BooleanSize.U32);
writer.WriteEnum(obj.Type); // Writes the enum type based on the amount of bytes of the enum's underlying type (short/2 in this case)
writer.WriteInt16(obj.Version); // Writes a 'short' (2 bytes)
writer.WriteDateTime(obj.Date); // Writes a 'DateTime' (8 bytes)
writer.WriteInt128(obj.Int128); // Writes an 'Int128' (16 bytes)
writer.WriteUInt32s(obj.ArrayWith16Elements); // Writes 16 'uint's (4 bytes each)
writer.WriteBoolean(obj.Bool32); // Writes a 'bool' (4 bytes in this case, since the reader's current bool state is BooleanSize.U32)

writer.ASCII = true; // Set the reader's ASCII state to true
writer.WriteChars_NullTerminated(obj.NullTerminatedASCIIString); // Writes the chars in the 'string' as ASCII and appends a '\0' at the end

writer.ASCII = false; // Set the reader's ASCII state to false (UTF16-LE)
writer.WriteChars_Count(obj.UTF16String, 10); // Writes 10 UTF16-LE chars as a 'string'. If the string has more than 10 chars, it is truncated; if it has less, it is padded with '\0'
```
### Writing Automatically (With Reflection):
```cs
var writer = new EndianBinaryWriter(stream, endianness: Endianness.LittleEndian);
writer.Write(obj); // Write all properties in the 'MyBasicObj' in order, ignoring any with a 'BinaryIgnoreAttribute'
// Other objects that are properties in this object will also be written in the same way recursively
```

### EndianBinaryPrimitives Example:
```cs
byte[] bytes = new byte[] { 0xFF, 0x00, 0x00, 0x00, 0xBB, 0xEE, 0xEE, 0xFF };
uint value = EndianBinaryPrimitives.ReadUInt32(bytes, Endianness.LittleEndian); // Will return 255

value = 128;
EndianBinaryPrimitives.WriteUInt32(bytes.AsSpan(4, 4), value, Endianness.LittleEndian); // bytes is now { 0xFF, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00 }
```

----
## EndianBinaryIOTests Uses:
* [xUnit.net](https://github.com/xunit/xunit)