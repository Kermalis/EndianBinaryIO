using Kermalis.EndianBinaryIO;
using System.IO;
using System.Linq;
using Xunit;

namespace Kermalis.EndianBinaryIOTests;

public sealed class UInt16Tests
{
	#region Constants

	private const ushort TEST_VAL = 20_901;
	private static readonly byte[] _testValBytesLE = new byte[sizeof(ushort)]
	{
		0xA5, 0x51,
	};
	private static readonly byte[] _testValBytesBE = new byte[sizeof(ushort)]
	{
		0x51, 0xA5,
	};

	private static readonly ushort[] _testArr = new ushort[4]
	{
		6_861,
		37_712,
		09_515,
		46_233,
	};
	private static readonly byte[] _testArrBytesLE = new byte[4 * sizeof(ushort)]
	{
		0xCD, 0x1A,
		0x50, 0x93,
		0x2B, 0x25,
		0x99, 0xB4,
	};
	private static readonly byte[] _testArrBytesBE = new byte[4 * sizeof(ushort)]
	{
		0x1A, 0xCD,
		0x93, 0x50,
		0x25, 0x2B,
		0xB4, 0x99,
	};

	#endregion

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadUInt16(bool le)
	{
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		ushort val;
		using (var stream = new MemoryStream(input))
		{
			val = new EndianBinaryReader(stream, endianness: e).ReadUInt16();
		}
		Assert.Equal(TEST_VAL, val);
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadUInt16s(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		ushort[] arr = new ushort[4];
		using (var stream = new MemoryStream(input))
		{
			new EndianBinaryReader(stream, endianness: e).ReadUInt16s(arr);
		}
		Assert.True(arr.SequenceEqual(_testArr));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteUInt16(bool le)
	{
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[sizeof(ushort)];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteUInt16(TEST_VAL);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteUInt16s(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[4 * sizeof(ushort)];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteUInt16s(_testArr);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
}
