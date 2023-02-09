using System;
using System.Globalization;
using Xunit;

namespace Kermalis.EndianBinaryIOTests;

public sealed class HalfTests
{
	#region Constants

	private const int SIZEOF_HALF = sizeof(ushort);

	private static readonly Half _testVal = Parse("12.34");
	private static readonly byte[] _testValBytesLE = new byte[SIZEOF_HALF]
	{
		0x2C, 0x4A,
	};
	private static readonly byte[] _testValBytesBE = new byte[SIZEOF_HALF]
	{
		0x4A, 0x2C,
	};

	private static readonly Half[] _testArr = new Half[4]
	{
		Half.MinValue,
		Parse("-15.25"),
		Parse("1,234.5"),
		Half.MaxValue,
	};
	private static readonly byte[] _testArrBytesLE = new byte[4 * SIZEOF_HALF]
	{
		0xFF, 0xFB,
		0xA0, 0xCB,
		0xD2, 0x64,
		0xFF, 0x7B,
	};
	private static readonly byte[] _testArrBytesBE = new byte[4 * SIZEOF_HALF]
	{
		0xFB, 0xFF,
		0xCB, 0xA0,
		0x64, 0xD2,
		0x7B, 0xFF,
	};

	private static Half Parse(string num)
	{
		return Half.Parse(num, style: NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, provider: NumberFormatInfo.InvariantInfo);
	}

	#endregion

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadHalf(bool le)
	{
		NumTestUtils.ReadValue(le, _testVal, _testValBytesLE, _testValBytesBE,
			(r) => r.ReadHalf());
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadHalves(bool le)
	{
		NumTestUtils.ReadValues(le, _testArr, _testArrBytesLE, _testArrBytesBE,
			(r, v) => r.ReadHalves(v));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteHalf(bool le)
	{
		NumTestUtils.WriteValue(le, _testVal, _testValBytesLE, _testValBytesBE, SIZEOF_HALF,
			(w, v) => w.WriteHalf(v));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteHalves(bool le)
	{
		NumTestUtils.WriteValues(le, _testArr, _testArrBytesLE, _testArrBytesBE, SIZEOF_HALF,
			(w, v) => w.WriteHalves(v));
	}
}
