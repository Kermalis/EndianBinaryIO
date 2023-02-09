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
		NumTestUtils.ReadValue(le, TEST_VAL, _testValBytesLE, _testValBytesBE,
			(r) => r.ReadUInt16());
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadUInt16s(bool le)
	{
		NumTestUtils.ReadValues(le, _testArr, _testArrBytesLE, _testArrBytesBE,
			(r, v) => r.ReadUInt16s(v));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteUInt16(bool le)
	{
		NumTestUtils.WriteValue(le, TEST_VAL, _testValBytesLE, _testValBytesBE, sizeof(ushort),
			(w, v) => w.WriteUInt16(v));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteUInt16s(bool le)
	{
		NumTestUtils.WriteValues(le, _testArr, _testArrBytesLE, _testArrBytesBE, sizeof(ushort),
			(w, v) => w.WriteUInt16s(v));
	}
}
