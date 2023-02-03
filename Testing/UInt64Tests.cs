using Kermalis.EndianBinaryIO;
using System.IO;
using System.Linq;
using Xunit;

namespace Kermalis.EndianBinaryIOTests;

public sealed class UInt64Tests
{
	#region Constants

	private const ulong TEST_VAL = 14_793_741_306_955_655_192;
	private static readonly byte[] _testValBytesLE = new byte[sizeof(ulong)]
	{
		0x18, 0x68, 0x1D, 0x8C, 0x56, 0xED, 0x4D, 0xCD,
	};
	private static readonly byte[] _testValBytesBE = new byte[sizeof(ulong)]
	{
		0xCD, 0x4D, 0xED, 0x56, 0x8C, 0x1D, 0x68, 0x18,
	};

	private static readonly ulong[] _testArr = new ulong[4]
	{
		16_488_541_351_461_240_347,
		4_889_897_707_926_465_544,
		13_989_148_393_676_279_722,
		13_184_186_537_684_656_338,
	};
	private static readonly byte[] _testArrBytesLE = new byte[4 * sizeof(ulong)]
	{
		0x1B, 0xBE, 0x2F, 0xC6, 0xFF, 0x10, 0xD3, 0xE4,
		0x08, 0x2C, 0xF4, 0xBC, 0x0E, 0x68, 0xDC, 0x43,
		0xAA, 0x0F, 0x4D, 0xAB, 0x58, 0x70, 0x23, 0xC2,
		0xD2, 0x60, 0x2E, 0x9F, 0xCD, 0xA3, 0xF7, 0xB6,
	};
	private static readonly byte[] _testArrBytesBE = new byte[4 * sizeof(ulong)]
	{
		0xE4, 0xD3, 0x10, 0xFF, 0xC6, 0x2F, 0xBE, 0x1B,
		0x43, 0xDC, 0x68, 0x0E, 0xBC, 0xF4, 0x2C, 0x08,
		0xC2, 0x23, 0x70, 0x58, 0xAB, 0x4D, 0x0F, 0xAA,
		0xB6, 0xF7, 0xA3, 0xCD, 0x9F, 0x2E, 0x60, 0xD2,
	};

	#endregion

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadUInt64(bool le)
	{
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		ulong val;
		using (var stream = new MemoryStream(input))
		{
			val = new EndianBinaryReader(stream, endianness: e).ReadUInt64();
		}
		Assert.Equal(TEST_VAL, val);
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadUInt64s(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		ulong[] arr = new ulong[4];
		using (var stream = new MemoryStream(input))
		{
			new EndianBinaryReader(stream, endianness: e).ReadUInt64s(arr);
		}
		Assert.True(arr.SequenceEqual(_testArr));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteUInt64(bool le)
	{
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[sizeof(ulong)];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteUInt64(TEST_VAL);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteUInt64s(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[4 * sizeof(ulong)];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteUInt64s(_testArr);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
}
