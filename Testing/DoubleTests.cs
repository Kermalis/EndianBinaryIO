using Xunit;

namespace Kermalis.EndianBinaryIOTests;

public sealed class DoubleTests
{
	#region Constants

	private const double TEST_VAL = 12_345_678.12345678d;
	private static readonly byte[] _testValBytesLE = new byte[sizeof(double)]
	{
		0xA2, 0x5B, 0xF3, 0xC3, 0x29, 0x8C, 0x67, 0x41,
	};
	private static readonly byte[] _testValBytesBE = new byte[sizeof(double)]
	{
		0x41, 0x67, 0x8C, 0x29, 0xC3, 0xF3, 0x5B, 0xA2,
	};

	private static readonly double[] _testArr = new double[4]
	{
		-51_692_240.59455357d,
		-68_231_145.04473292d,
		98_110_687.70543043d,
		75_442_096.25828312d,
	};
	private static readonly byte[] _testArrBytesLE = new byte[4 * sizeof(double)]
	{
		0x4D, 0xA5, 0xC1, 0x84, 0x16, 0xA6, 0x88, 0xC1,
		0x77, 0xCE, 0x2D, 0xA4, 0x7F, 0x44, 0x90, 0xC1,
		0x5B, 0x5C, 0xD2, 0x7E, 0x33, 0x64, 0x97, 0x41,
		0x5F, 0x7B, 0x08, 0xC1, 0x9E, 0xFC, 0x91, 0x41,
	};
	private static readonly byte[] _testArrBytesBE = new byte[4 * sizeof(double)]
	{
		0xC1, 0x88, 0xA6, 0x16, 0x84, 0xC1, 0xA5, 0x4D,
		0xC1, 0x90, 0x44, 0x7F, 0xA4, 0x2D, 0xCE, 0x77,
		0x41, 0x97, 0x64, 0x33, 0x7E, 0xD2, 0x5C, 0x5B,
		0x41, 0x91, 0xFC, 0x9E, 0xC1, 0x08, 0x7B, 0x5F,
	};

	#endregion

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadDouble(bool le)
	{
		NumTestUtils.ReadValue(le, TEST_VAL, _testValBytesLE, _testValBytesBE,
			(r) => r.ReadDouble());
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadDoubles(bool le)
	{
		NumTestUtils.ReadValues(le, _testArr, _testArrBytesLE, _testArrBytesBE,
			(r, v) => r.ReadDoubles(v));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteDouble(bool le)
	{
		NumTestUtils.WriteValue(le, TEST_VAL, _testValBytesLE, _testValBytesBE, sizeof(double),
			(w, v) => w.WriteDouble(v));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteDoubles(bool le)
	{
		NumTestUtils.WriteValues(le, _testArr, _testArrBytesLE, _testArrBytesBE, sizeof(double),
			(w, v) => w.WriteDoubles(v));
	}
}
