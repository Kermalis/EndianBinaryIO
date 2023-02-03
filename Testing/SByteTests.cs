using Kermalis.EndianBinaryIO;
using System;
using System.IO;
using System.Linq;
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
		sbyte val;
		using (var stream = new MemoryStream(_testValBytes))
		{
			val = new EndianBinaryReader(stream).ReadSByte();
		}
		Assert.Equal(TEST_VAL, val);
	}
	[Fact]
	public void ReadSBytes()
	{
		sbyte[] arr = new sbyte[4];
		using (var stream = new MemoryStream(_testArrBytes))
		{
			new EndianBinaryReader(stream).ReadSBytes(arr);
		}
		Assert.True(arr.SequenceEqual(_testArr));
	}
	[Fact]
	public void WriteSByte()
	{
		byte[] bytes = new byte[1];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream).WriteSByte(TEST_VAL);
		}
		Assert.True(bytes.SequenceEqual(_testValBytes));
	}
	[Fact]
	public void WriteSBytes()
	{
		byte[] bytes = new byte[4];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream).WriteSBytes(_testArr);
		}
		Assert.True(bytes.SequenceEqual(_testArrBytes));
	}
}
