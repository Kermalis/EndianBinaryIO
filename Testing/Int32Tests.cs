using Kermalis.EndianBinaryIO;
using System.IO;
using System.Linq;
using Xunit;

namespace Kermalis.EndianBinaryIOTests;

public sealed class Int32Tests
{
	#region Constants

	private const int TEST_VAL = -13_477;
	private static readonly byte[] _testValBytesLE = new byte[sizeof(int)]
	{
		0x5B, 0xCB, 0xFF, 0xFF,
	};
	private static readonly byte[] _testValBytesBE = new byte[sizeof(int)]
	{
		0xFF, 0xFF, 0xCB, 0x5B,
	};

	private static readonly int[] _testArr = new int[4]
	{
		-2_024_826_956,
		-570_721_400,
		1_250_296_726,
		1_161_309_695,
	};
	private static readonly byte[] _testArrBytesLE = new byte[4 * sizeof(int)]
	{
		0xB4, 0x97, 0x4F, 0x87,
		0x88, 0x7B, 0xFB, 0xDD,
		0x96, 0x03, 0x86, 0x4A,
		0xFF, 0x2D, 0x38, 0x45,
	};
	private static readonly byte[] _testArrBytesBE = new byte[4 * sizeof(int)]
	{
		0x87, 0x4F, 0x97, 0xB4,
		0xDD, 0xFB, 0x7B, 0x88,
		0x4A, 0x86, 0x03, 0x96,
		0x45, 0x38, 0x2D, 0xFF,
	};

	#endregion

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadInt32(bool le)
	{
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		int val;
		using (var stream = new MemoryStream(input))
		{
			val = new EndianBinaryReader(stream, endianness: e).ReadInt32();
		}
		Assert.Equal(TEST_VAL, val);
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadInt32s(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		int[] arr = new int[4];
		using (var stream = new MemoryStream(input))
		{
			new EndianBinaryReader(stream, endianness: e).ReadInt32s(arr);
		}
		Assert.True(arr.SequenceEqual(_testArr));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteInt32(bool le)
	{
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[sizeof(int)];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteInt32(TEST_VAL);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteInt32s(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[4 * sizeof(int)];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteInt32s(_testArr);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
}
