using Kermalis.EndianBinaryIO;
using System.IO;
using System.Linq;
using Xunit;

namespace Kermalis.EndianBinaryIOTests;

public sealed class Int16Tests
{
	#region Constants

	private const short TEST_VAL = -6_969;
	private static readonly byte[] _testValBytesLE = new byte[sizeof(short)]
	{
		0xC7, 0xE4,
	};
	private static readonly byte[] _testValBytesBE = new byte[sizeof(short)]
	{
		0xE4, 0xC7,
	};

	private static readonly short[] _testArr = new short[4]
	{
		-8_517,
		-22_343,
		26_381,
		2_131,
	};
	private static readonly byte[] _testArrBytesLE = new byte[4 * sizeof(short)]
	{
		0xBB, 0xDE,
		0xB9, 0xA8,
		0x0D, 0x67,
		0x53, 0x08,
	};
	private static readonly byte[] _testArrBytesBE = new byte[4 * sizeof(short)]
	{
		0xDE, 0xBB,
		0xA8, 0xB9,
		0x67, 0x0D,
		0x08, 0x53,
	};

	#endregion

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadInt16(bool le)
	{
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		short val;
		using (var stream = new MemoryStream(input))
		{
			val = new EndianBinaryReader(stream, endianness: e).ReadInt16();
		}
		Assert.Equal(TEST_VAL, val);
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadInt16s(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		short[] arr = new short[4];
		using (var stream = new MemoryStream(input))
		{
			new EndianBinaryReader(stream, endianness: e).ReadInt16s(arr);
		}
		Assert.True(arr.SequenceEqual(_testArr));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteInt16(bool le)
	{
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[sizeof(short)];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteInt16(TEST_VAL);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteInt16s(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[4 * sizeof(short)];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteInt16s(_testArr);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
}
