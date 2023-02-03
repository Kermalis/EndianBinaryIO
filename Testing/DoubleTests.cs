using Kermalis.EndianBinaryIO;
using System.IO;
using System.Linq;
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
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		double val;
		using (var stream = new MemoryStream(input))
		{
			val = new EndianBinaryReader(stream, endianness: e).ReadDouble();
		}
		Assert.Equal(TEST_VAL, val);
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadDoubles(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		double[] arr = new double[4];
		using (var stream = new MemoryStream(input))
		{
			new EndianBinaryReader(stream, endianness: e).ReadDoubles(arr);
		}
		Assert.True(arr.SequenceEqual(_testArr));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteDouble(bool le)
	{
		byte[] input = le ? _testValBytesLE : _testValBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[sizeof(double)];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteDouble(TEST_VAL);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteDoubles(bool le)
	{
		byte[] input = le ? _testArrBytesLE : _testArrBytesBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[4 * sizeof(double)];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteDoubles(_testArr);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
}
