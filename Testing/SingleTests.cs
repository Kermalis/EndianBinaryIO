using Kermalis.EndianBinaryIO;
using System.IO;
using System.Linq;
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
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		float val;
		using (var stream = new MemoryStream(input))
		{
			val = new EndianBinaryReader(stream, endianness: e).ReadSingle();
		}
		Assert.Equal(TEST_VAL, val);
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadSingles(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		float[] arr = new float[4];
		using (var stream = new MemoryStream(input))
		{
			new EndianBinaryReader(stream, endianness: e).ReadSingles(arr);
		}
		Assert.True(arr.SequenceEqual(_testArr));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteSingle(bool le)
	{
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[sizeof(float)];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteSingle(TEST_VAL);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteSingles(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[4 * sizeof(float)];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteSingles(_testArr);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
}
