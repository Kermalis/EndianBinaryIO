using Kermalis.EndianBinaryIO;
using System.IO;
using System.Linq;
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
		byte val;
		using (var stream = new MemoryStream(_testValBytes))
		{
			val = new EndianBinaryReader(stream).ReadByte();
		}
		Assert.Equal(TEST_VAL, val);
	}
	[Fact]
	public void ReadBytes()
	{
		byte[] arr = new byte[4];
		using (var stream = new MemoryStream(_testArrBytes))
		{
			new EndianBinaryReader(stream).ReadBytes(arr);
		}
		Assert.True(arr.SequenceEqual(_testArr));
	}
	[Fact]
	public void WriteByte()
	{
		byte[] bytes = new byte[1];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream).WriteByte(TEST_VAL);
		}
		Assert.True(bytes.SequenceEqual(_testValBytes));
	}
	[Fact]
	public void WriteBytes()
	{
		byte[] bytes = new byte[4];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream).WriteBytes(_testArr);
		}
		Assert.True(bytes.SequenceEqual(_testArrBytes));
	}
}
