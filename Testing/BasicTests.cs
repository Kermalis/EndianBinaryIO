using Kermalis.EndianBinaryIO;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Xunit;

namespace Kermalis.EndianBinaryIOTests;

public sealed class BasicTests
{
	private sealed class MyBasicObj
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
		[BinaryStringFixedLength(10)]
		[BinaryStringTrimNullTerminators]
		public string UTF16String { get; set; }
	}

	#region Constants

	private static readonly DateTime _expectedDateTime = new(1998, 12, 30);
	private static readonly Int128 _expectedInt128 = Int128.Parse("48,045,707,429,126,174,655,160,174,263,614,327,112", NumberStyles.AllowThousands);
	private static readonly byte[] _bytes = new byte[]
	{
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
	};
	private static MyBasicObj GetObj()
	{
		return new MyBasicObj
		{
			Type = ShortSizedEnum.Val2,
			Version = 511,
			Date = _expectedDateTime,
			Int128 = _expectedInt128,

			DoNotReadOrWrite = ByteSizedEnum.Val1,

			ArrayWith16Elements = new uint[16]
			{
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
			},

			Bool32 = false,

			NullTerminatedASCIIString = "EndianBinaryIO",
			UTF16String = "Kermalis",
		};
	}

	#endregion

	[Fact]
	public void ReadObject()
	{
		MyBasicObj obj;
		using (var stream = new MemoryStream(_bytes))
		{
			obj = new EndianBinaryReader(stream, endianness: Endianness.LittleEndian).ReadObject<MyBasicObj>();
		}

		Assert.Equal(ShortSizedEnum.Val2, obj.Type); // Enum works
		Assert.Equal(511, obj.Version); // short works
		Assert.Equal(_expectedDateTime, obj.Date); // DateTime works
		Assert.Equal(_expectedInt128, obj.Int128); // Int128 works

		Assert.Equal(default, obj.DoNotReadOrWrite); // Ignored

		Assert.Equal(16, obj.ArrayWith16Elements.Length); // Fixed size array works
		for (uint i = 0; i < 16; i++)
		{
			Assert.Equal(i, obj.ArrayWith16Elements[i]); // Array works
		}

		Assert.False(obj.Bool32); // bool32 works

		Assert.Equal("EndianBinaryIO", obj.NullTerminatedASCIIString); // Stops reading at null terminator
		Assert.Equal("Kermalis", obj.UTF16String); // Fixed size (10 chars) UTF16-LE, with the \0s trimmed
	}

	[Fact]
	public void ReadManually()
	{
		using (var stream = new MemoryStream(_bytes))
		{
			var reader = new EndianBinaryReader(stream, endianness: Endianness.LittleEndian, booleanSize: BooleanSize.U32);
			var obj = new MyBasicObj();

			obj.Type = reader.ReadEnum<ShortSizedEnum>();
			Assert.Equal(ShortSizedEnum.Val2, obj.Type); // Enum works
			obj.Version = reader.ReadInt16();
			Assert.Equal(511, obj.Version); // short works
			obj.Date = reader.ReadDateTime();
			Assert.Equal(_expectedDateTime, obj.Date); // DateTime works
			obj.Int128 = reader.ReadInt128();
			Assert.Equal(_expectedInt128, obj.Int128); // Int128 works

			obj.ArrayWith16Elements = new uint[16];
			reader.ReadUInt32s(obj.ArrayWith16Elements);
			for (uint i = 0; i < 16; i++)
			{
				Assert.Equal(i, obj.ArrayWith16Elements[i]); // Array works
			}

			obj.Bool32 = reader.ReadBoolean();
			Assert.False(obj.Bool32); // bool32 works

			reader.ASCII = true;
			obj.NullTerminatedASCIIString = reader.ReadString_NullTerminated();
			Assert.Equal("EndianBinaryIO", obj.NullTerminatedASCIIString); // Stops reading at null terminator

			reader.ASCII = false;
			obj.UTF16String = reader.ReadString_Count_TrimNullTerminators(10);
			Assert.Equal("Kermalis", obj.UTF16String); // Fixed size (10 chars) UTF16-LE, with the \0s trimmed
		}
	}

	[Fact]
	public void WriteObject()
	{
		byte[] bytes = new byte[_bytes.Length];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: Endianness.LittleEndian).WriteObject(GetObj());
		}

		Assert.True(bytes.SequenceEqual(_bytes));
	}

	[Fact]
	public void WriteManually()
	{
		MyBasicObj obj = GetObj();

		byte[] bytes = new byte[_bytes.Length];
		using (var stream = new MemoryStream(bytes))
		{
			var writer = new EndianBinaryWriter(stream, endianness: Endianness.LittleEndian, booleanSize: BooleanSize.U32);
			writer.WriteEnum(obj.Type);
			writer.WriteInt16(obj.Version);
			writer.WriteDateTime(obj.Date);
			writer.WriteInt128(obj.Int128);

			writer.WriteUInt32s(obj.ArrayWith16Elements);

			writer.WriteBoolean(obj.Bool32);

			writer.ASCII = true;
			writer.WriteChars_NullTerminated(obj.NullTerminatedASCIIString);
			writer.ASCII = false;
			writer.WriteChars_Count(obj.UTF16String, 10);
		}

		Assert.True(bytes.SequenceEqual(_bytes));
	}

	[Fact]
	public void SpanIsProperlyTrimmed()
	{
		Span<char> test = stackalloc char[] { 'K', 'e', 'r', 'm', 'a', 'l', 'i', 's', '\0', '\0', };
		EndianBinaryPrimitives.TrimNullTerminators(ref test);

		Assert.True(test.SequenceEqual("Kermalis"));
	}

	[Fact]
	public void ReadOnlySpanIsProperlyTrimmed()
	{
		ReadOnlySpan<char> test = "Kermalis\0\0";
		EndianBinaryPrimitives.TrimNullTerminators(ref test);

		Assert.True(test.SequenceEqual("Kermalis"));
	}
}
