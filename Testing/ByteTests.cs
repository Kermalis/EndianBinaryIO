using Xunit;

namespace Kermalis.EndianBinaryIOTests;

public sealed class ByteTests
{
	#region Constants

	private const byte TEST_VAL = 210;
	private static readonly byte[] _testValBytes = new byte[1]
	{
		0xD2,
	};

	private static readonly byte[] _testArr = new byte[4]
	{
		99,
		209,
		3,
		64,
	};
	private static readonly byte[] _testArrBytes = new byte[4]
	{
		0x63,
		0xD1,
		0x03,
		0x40,
	};

	#endregion

	[Fact]
	public void ReadByte()
	{
		NumTestUtils.ReadValue(TEST_VAL, _testValBytes,
			(r) => r.ReadByte());
	}
	[Fact]
	public void ReadBytes()
	{
		NumTestUtils.ReadValues(_testArr, _testArrBytes,
			(r, v) => r.ReadBytes(v));
	}
	[Fact]
	public void WriteByte()
	{
		NumTestUtils.WriteValue(TEST_VAL, _testValBytes,
			(w, v) => w.WriteByte(v));
	}
	[Fact]
	public void WriteBytes()
	{
		NumTestUtils.WriteValues(_testArr, _testArrBytes,
			(w, v) => w.WriteBytes(v));
	}
}
