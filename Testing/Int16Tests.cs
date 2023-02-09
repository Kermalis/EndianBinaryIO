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
		NumTestUtils.ReadValue(le, TEST_VAL, _testValBytesLE, _testValBytesBE,
			(r) => r.ReadInt16());
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadInt16s(bool le)
	{
		NumTestUtils.ReadValues(le, _testArr, _testArrBytesLE, _testArrBytesBE,
			(r, v) => r.ReadInt16s(v));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteInt16(bool le)
	{
		NumTestUtils.WriteValue(le, TEST_VAL, _testValBytesLE, _testValBytesBE, sizeof(short),
			(w, v) => w.WriteInt16(v));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteInt16s(bool le)
	{
		NumTestUtils.WriteValues(le, _testArr, _testArrBytesLE, _testArrBytesBE, sizeof(short),
			(w, v) => w.WriteInt16s(v));
	}
}
