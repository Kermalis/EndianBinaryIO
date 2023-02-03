using Kermalis.EndianBinaryIO;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Xunit;

namespace Kermalis.EndianBinaryIOTests;

public sealed class UInt128Tests
{
	#region Constants

	private const int SIZEOF_UINT128 = sizeof(ulong) * 2;

	private static readonly UInt128 TEST_VAL = Parse("4,441,948,164,730,199,290,118,182");
	private static readonly byte[] _testValBytesLE = new byte[SIZEOF_UINT128]
	{
		0x26, 0x80, 0x67, 0x38, 0x19, 0x5C, 0x15, 0x7E, 0x9E, 0xAC, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00,
	};
	private static readonly byte[] _testValBytesBE = new byte[SIZEOF_UINT128]
	{
		0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0xAC, 0x9E, 0x7E, 0x15, 0x5C, 0x19, 0x38, 0x67, 0x80, 0x26,
	};

	private static readonly UInt128[] _testArr = new UInt128[4]
	{
		Parse("8,427,008,177,709,125,582,861,203"),
		Parse("5,054,904,958,755,150,864,423,633"),
		Parse("1,381,684,868,511,170,617,938,143"),
		Parse("3,631,327,785,041,621,745,644,989"),
	};
	private static readonly byte[] _testArrBytesLE = new byte[4 * SIZEOF_UINT128]
	{
		0x93, 0x33, 0xB6, 0xAF, 0x96, 0x4F, 0x58, 0x07, 0x7D, 0xF8, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00,
		0xD1, 0x9A, 0xB9, 0x15, 0x83, 0x01, 0x68, 0xF2, 0x6A, 0x2E, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00,
		0xDF, 0x10, 0x74, 0x14, 0xBC, 0x26, 0x6C, 0x49, 0x95, 0x24, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00,
		0xBD, 0xB5, 0x27, 0x86, 0x2D, 0x20, 0x76, 0xAC, 0xF6, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00,
	};
	private static readonly byte[] _testArrBytesBE = new byte[4 * SIZEOF_UINT128]
	{
		0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0xF8, 0x7D, 0x07, 0x58, 0x4F, 0x96, 0xAF, 0xB6, 0x33, 0x93,
		0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x2E, 0x6A, 0xF2, 0x68, 0x01, 0x83, 0x15, 0xB9, 0x9A, 0xD1,
		0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x24, 0x95, 0x49, 0x6C, 0x26, 0xBC, 0x14, 0x74, 0x10, 0xDF,
		0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0xF6, 0xAC, 0x76, 0x20, 0x2D, 0x86, 0x27, 0xB5, 0xBD,
	};

	private static UInt128 Parse(string num)
	{
		return UInt128.Parse(num, NumberStyles.AllowThousands, NumberFormatInfo.InvariantInfo);
	}

	#endregion

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadUInt128(bool le)
	{
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		UInt128 val;
		using (var stream = new MemoryStream(input))
		{
			val = new EndianBinaryReader(stream, endianness: e).ReadUInt128();
		}
		Assert.Equal(TEST_VAL, val);
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadUInt128s(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		var arr = new UInt128[4];
		using (var stream = new MemoryStream(input))
		{
			new EndianBinaryReader(stream, endianness: e).ReadUInt128s(arr);
		}
		Assert.True(arr.SequenceEqual(_testArr));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteUInt128(bool le)
	{
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[SIZEOF_UINT128];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteUInt128(TEST_VAL);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteUInt128s(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[4 * SIZEOF_UINT128];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteUInt128s(_testArr);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
}
