using Xunit;

namespace Kermalis.EndianBinaryIOTests;

public sealed class SByteTests
{
	#region Constants

	private const sbyte TEST_VAL = -93;
	private static readonly byte[] _testValBytes = new byte[1]
	{
		0xA3,
	};

	private static readonly sbyte[] _testArr = new sbyte[4]
	{
		-21,
		6,
		17,
		-100,
	};
	private static readonly byte[] _testArrBytes = new byte[4]
	{
		0xEB,
		0x06,
		0x11,
		0x9C,
	};

	#endregion

	[Fact]
	public void ReadSByte()
	{
		NumTestUtils.ReadValue(TEST_VAL, _testValBytes,
			(r) => r.ReadSByte());
	}
	[Fact]
	public void ReadSBytes()
	{
		NumTestUtils.ReadValues(_testArr, _testArrBytes,
			(r, v) => r.ReadSBytes(v));
	}
	[Fact]
	public void WriteSByte()
	{
		NumTestUtils.WriteValue(TEST_VAL, _testValBytes,
			(w, v) => w.WriteSByte(v));
	}
	[Fact]
	public void WriteSBytes()
	{
		NumTestUtils.WriteValues(_testArr, _testArrBytes,
			(w, v) => w.WriteSBytes(v));
	}
}
