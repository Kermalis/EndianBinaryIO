using Xunit;

namespace Kermalis.EndianBinaryIOTests;

public sealed class SingleTests
{
	#region Constants

	private const float TEST_VAL = 1_234.1234f;
	private static readonly byte[] _testValBytesLE = new byte[sizeof(float)]
	{
		0xF3, 0x43, 0x9A, 0x44,
	};
	private static readonly byte[] _testValBytesBE = new byte[sizeof(float)]
	{
		0x44, 0x9A, 0x43, 0xF3,
	};

	private static readonly float[] _testArr = new float[4]
	{
		-6_814.5127f,
		-3_391.6581f,
		8_710.1492f,
		3_065.2182f,
	};
	private static readonly byte[] _testArrBytesLE = new byte[4 * sizeof(float)]
	{
		0x1A, 0xF4, 0xD4, 0xC5,
		0x88, 0xFA, 0x53, 0xC5,
		0x99, 0x18, 0x08, 0x46,
		0x7E, 0x93, 0x3F, 0x45,
	};
	private static readonly byte[] _testArrBytesBE = new byte[4 * sizeof(float)]
	{
		0xC5, 0xD4, 0xF4, 0x1A,
		0xC5, 0x53, 0xFA, 0x88,
		0x46, 0x08, 0x18, 0x99,
		0x45, 0x3F, 0x93, 0x7E,
	};

	#endregion

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadSingle(bool le)
	{
		NumTestUtils.ReadValue(le, TEST_VAL, _testValBytesLE, _testValBytesBE,
			(r) => r.ReadSingle());
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadSingles(bool le)
	{
		NumTestUtils.ReadValues(le, _testArr, _testArrBytesLE, _testArrBytesBE,
			(r, v) => r.ReadSingles(v));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteSingle(bool le)
	{
		NumTestUtils.WriteValue(le, TEST_VAL, _testValBytesLE, _testValBytesBE, sizeof(float),
			(w, v) => w.WriteSingle(v));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteSingles(bool le)
	{
		NumTestUtils.WriteValues(le, _testArr, _testArrBytesLE, _testArrBytesBE, sizeof(float),
			(w, v) => w.WriteSingles(v));
	}
}
