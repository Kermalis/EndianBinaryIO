using Kermalis.EndianBinaryIO;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Xunit;

namespace Kermalis.EndianBinaryIOTests;

public sealed class Int128Tests
{
	#region Constants

	private const int SIZEOF_INT128 = sizeof(ulong) * 2;

	private static readonly Int128 TEST_VAL = Parse("-9,183,616,886,827,840,433,572,302");
	private static readonly byte[] _testValBytesLE = new byte[SIZEOF_INT128]
	{
		0x32, 0xB6, 0x07, 0x84, 0xAF, 0x4D, 0x89, 0x21, 0x4B, 0x67, 0xF8, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
	};
	private static readonly byte[] _testValBytesBE = new byte[SIZEOF_INT128]
	{
		0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xF8, 0x67, 0x4B, 0x21, 0x89, 0x4D, 0xAF, 0x84, 0x07, 0xB6, 0x32,
	};

	private static readonly Int128[] _testArr = new Int128[4]
	{
		Parse("-5,484,660,365,300,721,980,907,729"),
		Parse("-3,392,080,724,347,328,401,378,812"),
		Parse("1,850,493,629,233,363,857,060,483"),
		Parse("7,096,725,192,671,155,766,764,600"),
	};
	private static readonly byte[] _testArrBytesLE = new byte[4 * SIZEOF_INT128]
	{
		0x2F, 0x5F, 0xD2, 0x2C, 0x05, 0x9F, 0x40, 0xF7, 0x93, 0x76, 0xFB, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
		0x04, 0xF2, 0x61, 0x35, 0x07, 0x04, 0x7B, 0xEF, 0xB2, 0x31, 0xFD, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
		0x83, 0x32, 0x5A, 0x49, 0x4D, 0x1C, 0xED, 0x75, 0xDB, 0x87, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
		0x38, 0x24, 0x25, 0x27, 0xE1, 0xB0, 0x5A, 0x3E, 0xCA, 0xDE, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00,
	};
	private static readonly byte[] _testArrBytesBE = new byte[4 * SIZEOF_INT128]
	{
		0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFB, 0x76, 0x93, 0xF7, 0x40, 0x9F, 0x05, 0x2C, 0xD2, 0x5F, 0x2F,
		0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFD, 0x31, 0xB2, 0xEF, 0x7B, 0x04, 0x07, 0x35, 0x61, 0xF2, 0x04,
		0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x87, 0xDB, 0x75, 0xED, 0x1C, 0x4D, 0x49, 0x5A, 0x32, 0x83,
		0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0xDE, 0xCA, 0x3E, 0x5A, 0xB0, 0xE1, 0x27, 0x25, 0x24, 0x38,
	};

	private static Int128 Parse(string num)
	{
		return Int128.Parse(num, NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands, NumberFormatInfo.InvariantInfo);
	}

	#endregion

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadInt128(bool le)
	{
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		Int128 val;
		using (var stream = new MemoryStream(input))
		{
			val = new EndianBinaryReader(stream, endianness: e).ReadInt128();
		}
		Assert.Equal(TEST_VAL, val);
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadInt128s(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		var arr = new Int128[4];
		using (var stream = new MemoryStream(input))
		{
			new EndianBinaryReader(stream, endianness: e).ReadInt128s(arr);
		}
		Assert.True(arr.SequenceEqual(_testArr));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteInt128(bool le)
	{
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[SIZEOF_INT128];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteInt128(TEST_VAL);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteInt128s(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[4 * SIZEOF_INT128];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteInt128s(_testArr);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
}
