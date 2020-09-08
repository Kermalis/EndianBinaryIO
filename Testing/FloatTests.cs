using Kermalis.EndianBinaryIO;
using System.IO;
using System.Linq;
using Xunit;

namespace Kermalis.EndianBinaryIOTests
{
    public sealed class FloatTests
    {
        private const float TestValue_Single = 1234.1234f;
        private const double TestValue_Double = 12345678.12345678d;
        private const decimal TestValue_Decimal = 12345678909876543210.123456789M;

        private static readonly byte[] _bigEndianBytes_Single = new byte[4]
        {
            0x44, 0x9A, 0x43, 0xF3
        };
        private static readonly byte[] _littleEndianBytes_Single = new byte[4]
        {
            0xF3, 0x43, 0x9A, 0x44
        };
        private static readonly byte[] _bigEndianBytes_Double = new byte[8]
        {
            0x41, 0x67, 0x8C, 0x29, 0xC3, 0xF3, 0x5B, 0xA2
        };
        private static readonly byte[] _littleEndianBytes_Double = new byte[8]
        {
            0xA2, 0x5B, 0xF3, 0xC3, 0x29, 0x8C, 0x67, 0x41
        };
        private static readonly byte[] _bigEndianBytes_Decimal = new byte[16]
        {
            0xBE, 0xAD, 0x40, 0x75, 0xA0, 0x84, 0x71, 0x15, 0x27, 0xE4, 0x1B, 0x32, 0x00, 0x09, 0x00, 0x00
        };
        private static readonly byte[] _littleEndianBytes_Decimal = new byte[16]
        {
            0x00, 0x00, 0x09, 0x00, 0x32, 0x1B, 0xE4, 0x27, 0x15, 0x71, 0x84, 0xA0, 0x75, 0x40, 0xAD, 0xBE
        };

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ReadSingle(bool le)
        {
            byte[] input = le ? _littleEndianBytes_Single : _bigEndianBytes_Single;
            Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;
            using (var stream = new MemoryStream(input))
            using (var reader = new EndianBinaryReader(stream, endianness: e))
            {
                Assert.Equal(reader.ReadSingle(), TestValue_Single);
            }
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void WriteSingle(bool le)
        {
            byte[] input = le ? _littleEndianBytes_Single : _bigEndianBytes_Single;
            Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;
            byte[] bytes = new byte[4];
            using (var stream = new MemoryStream(bytes))
            using (var writer = new EndianBinaryWriter(stream, endianness: e))
            {
                writer.Write(TestValue_Single);
            }
            Assert.True(bytes.SequenceEqual(input));
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ReadDouble(bool le)
        {
            byte[] input = le ? _littleEndianBytes_Double : _bigEndianBytes_Double;
            Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;
            using (var stream = new MemoryStream(input))
            using (var reader = new EndianBinaryReader(stream, endianness: e))
            {
                Assert.Equal(reader.ReadDouble(), TestValue_Double);
            }
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void WriteDouble(bool le)
        {
            byte[] input = le ? _littleEndianBytes_Double : _bigEndianBytes_Double;
            Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;
            byte[] bytes = new byte[8];
            using (var stream = new MemoryStream(bytes))
            using (var writer = new EndianBinaryWriter(stream, endianness: e))
            {
                writer.Write(TestValue_Double);
            }
            Assert.True(bytes.SequenceEqual(input));
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ReadDecimal(bool le)
        {
            byte[] input = le ? _littleEndianBytes_Decimal : _bigEndianBytes_Decimal;
            Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;
            using (var stream = new MemoryStream(input))
            using (var reader = new EndianBinaryReader(stream, endianness: e))
            {
                Assert.Equal(reader.ReadDecimal(), TestValue_Decimal);
            }
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void WriteDecimal(bool le)
        {
            byte[] input = le ? _littleEndianBytes_Decimal : _bigEndianBytes_Decimal;
            Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;
            byte[] bytes = new byte[16];
            using (var stream = new MemoryStream(bytes))
            using (var writer = new EndianBinaryWriter(stream, endianness: e))
            {
                writer.Write(TestValue_Decimal);
            }
            Assert.True(bytes.SequenceEqual(input));
        }
    }
}
