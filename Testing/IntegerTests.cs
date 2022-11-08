using Kermalis.EndianBinaryIO;
using System.IO;
using System.Linq;
using Xunit;

namespace Kermalis.EndianBinaryIOTests;

public sealed class IntegerTests
{
	#region Constants

	private const sbyte TEST_VAL_SBYTE = -93;
	private static readonly byte[] _bigEndianBytes_SByte = new byte[1]
	{
		0xA3,
	};
	private static readonly byte[] _littleEndianBytes_SByte = new byte[1]
	{
		0xA3,
	};

	private const byte TEST_VAL_BYTE = 210;
	private static readonly byte[] _bigEndianBytes_Byte = new byte[1]
	{
		0xD2,
	};
	private static readonly byte[] _littleEndianBytes_Byte = new byte[1]
	{
		0xD2,
	};

	private const short TEST_VAL_INT16 = -6_969;
	private static readonly byte[] _bigEndianBytes_Int16 = new byte[2]
	{
		0xE4, 0xC7,
	};
	private static readonly byte[] _littleEndianBytes_Int16 = new byte[2]
	{
		0xC7, 0xE4,
	};

	#endregion

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadSByte(bool le)
	{
		byte[] input = le ? _littleEndianBytes_SByte : _bigEndianBytes_SByte;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;
		using (var stream = new MemoryStream(input))
		{
			Assert.Equal(TEST_VAL_SBYTE, new EndianBinaryReader(stream, endianness: e).ReadSByte());
		}
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteSByte(bool le)
	{
		byte[] input = le ? _littleEndianBytes_SByte : _bigEndianBytes_SByte;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;
		byte[] bytes = new byte[1];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteSByte(TEST_VAL_SBYTE);
		}
		Assert.True(bytes.SequenceEqual(input));
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadByte(bool le)
	{
		byte[] input = le ? _littleEndianBytes_Byte : _bigEndianBytes_Byte;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;
		using (var stream = new MemoryStream(input))
		{
			Assert.Equal(TEST_VAL_BYTE, new EndianBinaryReader(stream, endianness: e).ReadByte());
		}
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteByte(bool le)
	{
		byte[] input = le ? _littleEndianBytes_Byte : _bigEndianBytes_Byte;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;
		byte[] bytes = new byte[1];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteByte(TEST_VAL_BYTE);
		}
		Assert.True(bytes.SequenceEqual(input));
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void ReadInt16(bool le)
	{
		byte[] input = le ? _littleEndianBytes_Int16 : _bigEndianBytes_Int16;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;
		using (var stream = new MemoryStream(input))
		{
			Assert.Equal(TEST_VAL_INT16, new EndianBinaryReader(stream, endianness: e).ReadInt16());
		}
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void WriteInt16(bool le)
	{
		byte[] input = le ? _littleEndianBytes_Int16 : _bigEndianBytes_Int16;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;
		byte[] bytes = new byte[2];
		using (var stream = new MemoryStream(bytes))
		{
			new EndianBinaryWriter(stream, endianness: e).WriteInt16(TEST_VAL_INT16);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
}
