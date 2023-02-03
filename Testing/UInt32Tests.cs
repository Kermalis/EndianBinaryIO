using Kermalis.EndianBinaryIO;
using System.IO;
using System.Linq;
using Xunit;

namespace Kermalis.EndianBinaryIOTests;

public sealed class UInt32Tests
{
	#region Constants

	private const uint TEST_VAL = 2_307_841_074;
	private static readonly byte[] _testValBytesLE = new byte[sizeof(uint)]
	{
		0x32, 0xDC, 0x8E, 0x89,
	};
	private static readonly byte[] _testValBytesBE = new byte[sizeof(uint)]
	{
		0x89, 0x8E, 0xDC, 0x32,
	};

	private static readonly uint[] _testArr = new uint[4]
	{
		2_091_540_746,
		411_473_902,
		1_365_957_744,
		3_249_860_615,
	};
	private static readonly byte[] _testArrBytesLE = new byte[4 * sizeof(uint)]
	{
		0x0A, 0x61, 0xAA, 0x7C,
		0xEE, 0x97, 0x86, 0x18,
		0x70, 0xDC, 0x6A, 0x51,
		0x07, 0xF0, 0xB4, 0xC1,
	};
	private static readonly byte[] _testArrBytesBE = new byte[4 * sizeof(uint)]
	{
		0x7C, 0xAA, 0x61, 0x0A,
		0x18, 0x86, 0x97, 0xEE,
		0x51, 0x6A, 0xDC, 0x70,
		0xC1, 0xB4, 0xF0, 0x07,
	};

	#endregion

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadUInt32(bool le)
	{
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		uint val;
		using (var stream = new MemoryStream(input))
		{
			val = new EndianBinaryReader(stream, endianness: e).ReadUInt32();
		}
		Assert.Equal(TEST_VAL, val);
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadUInt32s(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		uint[] arr = new uint[4];
		using (var stream = new MemoryStream(input))
		{
			new EndianBinaryReader(stream, endianness: e).ReadUInt32s(arr);
		}
		Assert.True(arr.SequenceEqual(_testArr));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteUInt32(bool le)
	{
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[sizeof(uint)];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteUInt32(TEST_VAL);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteUInt32s(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[4 * sizeof(uint)];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteUInt32s(_testArr);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
}
