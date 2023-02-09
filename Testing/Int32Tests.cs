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
		NumTestUtils.ReadValue(le, TEST_VAL, _testValBytesLE, _testValBytesBE,
			(r) => r.ReadInt32());
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadInt32s(bool le)
	{
		NumTestUtils.ReadValues(le, _testArr, _testArrBytesLE, _testArrBytesBE,
			(r, v) => r.ReadInt32s(v));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteInt32(bool le)
	{
		NumTestUtils.WriteValue(le, TEST_VAL, _testValBytesLE, _testValBytesBE, sizeof(int),
			(w, v) => w.WriteInt32(v));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteInt32s(bool le)
	{
		NumTestUtils.WriteValues(le, _testArr, _testArrBytesLE, _testArrBytesBE, sizeof(int),
			(w, v) => w.WriteInt32s(v));
	}
}
