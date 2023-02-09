using Xunit;

namespace Kermalis.EndianBinaryIOTests;

public sealed class Int64Tests
{
	#region Constants

	private const long TEST_VAL = -8_574_924_182_750_059_124;
	private static readonly byte[] _testValBytesLE = new byte[sizeof(long)]
	{
		0x8C, 0xA5, 0x46, 0x3F, 0xE3, 0xBF, 0xFF, 0x88,
	};
	private static readonly byte[] _testValBytesBE = new byte[sizeof(long)]
	{
		0x88, 0xFF, 0xBF, 0xE3, 0x3F, 0x46, 0xA5, 0x8C,
	};

	private static readonly long[] _testArr = new long[4]
	{
		-6_378_121_417_350_903_592,
		-397_400_959_663_200_194,
		7_340_833_089_393_364_811,
		8_772_498_435_351_010_701,
	};
	private static readonly byte[] _testArrBytesLE = new byte[4 * sizeof(long)]
	{
		0xD8, 0x08, 0x7D, 0x18, 0x5D, 0x5C, 0x7C, 0xA7,
		0x3E, 0x28, 0xE0, 0xC5, 0xEE, 0x25, 0x7C, 0xFA,
		0x4B, 0x8F, 0xB9, 0x28, 0xCD, 0xE0, 0xDF, 0x65,
		0x8D, 0x5D, 0xC4, 0x27, 0xD9, 0x2C, 0xBE, 0x79,
	};
	private static readonly byte[] _testArrBytesBE = new byte[4 * sizeof(long)]
	{
		0xA7, 0x7C, 0x5C, 0x5D, 0x18, 0x7D, 0x08, 0xD8,
		0xFA, 0x7C, 0x25, 0xEE, 0xC5, 0xE0, 0x28, 0x3E,
		0x65, 0xDF, 0xE0, 0xCD, 0x28, 0xB9, 0x8F, 0x4B,
		0x79, 0xBE, 0x2C, 0xD9, 0x27, 0xC4, 0x5D, 0x8D,
	};

	#endregion

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadInt64(bool le)
	{
		NumTestUtils.ReadValue(le, TEST_VAL, _testValBytesLE, _testValBytesBE,
			(r) => r.ReadInt64());
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadInt64s(bool le)
	{
		NumTestUtils.ReadValues(le, _testArr, _testArrBytesLE, _testArrBytesBE,
			(r, v) => r.ReadInt64s(v));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteInt64(bool le)
	{
		NumTestUtils.WriteValue(le, TEST_VAL, _testValBytesLE, _testValBytesBE, sizeof(long),
			(w, v) => w.WriteInt64(v));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteInt64s(bool le)
	{
		NumTestUtils.WriteValues(le, _testArr, _testArrBytesLE, _testArrBytesBE, sizeof(long),
			(w, v) => w.WriteInt64s(v));
	}
}
