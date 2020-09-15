using Kermalis.EndianBinaryIO;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Kermalis.EndianBinaryIOTests
{
    public sealed class EncodingTests
    {
        private sealed class CharObj
        {
            public byte Len { get; set; }
            [BinaryStringVariableLength(nameof(Len))]
            public string Str { get; set; }
        }

        #region Constants
        private const string TestString_ASCII = "Jummy\r\nFunnies";
        private static readonly byte[] _testBytes_ASCII = new byte[]
        {
            0x0E, // Len
            0x4A, 0x75, 0x6D, 0x6D, 0x79, // "Jummy"
            0x0D, 0x0A, // "\r\n"
            0x46, 0x75, 0x6E, 0x6E, 0x69, 0x65, 0x73 // "Funnies"
        };
        private const string TestString_UTF = "Jummy😀\r\n😳Funnies";
        private static readonly byte[] _testBytes_UTF7 = new byte[]
        {
            0x12, // Len
            0x4A, 0x75, 0x6D, 0x6D, 0x79, // "Jummy"
            0x2B, 0x32, 0x44, 0x33, 0x65, 0x41, 0x41, 0x2D, // "😀"
            0x0D, 0x0A, // "\r\n"
            0x2B, 0x32, 0x44, 0x33, 0x65, 0x4D, 0x77, 0x2D,
            0x46, 0x75, 0x6E, 0x6E, 0x69, 0x65, 0x73 // "Funnies"
        };
        private static readonly byte[] _testBytes_UTF8 = new byte[]
        {
            0x12, // Len
            0x4A, 0x75, 0x6D, 0x6D, 0x79, // "Jummy"
            0xF0, 0x9F, 0x98, 0x80, // "😀"
            0x0D, 0x0A, // "\r\n"
            0xF0, 0x9F, 0x98, 0xB3, // "😳"
            0x46, 0x75, 0x6E, 0x6E, 0x69, 0x65, 0x73 // "Funnies"
        };
        private static readonly byte[] _testBytes_UTF16 = new byte[]
        {
            0x12, // Len
            0x4A, 0x00, 0x75, 0x00, 0x6D, 0x00, 0x6D, 0x00, 0x79, 0x00, // "Jummy"
            0x3D, 0xD8, 0x00, 0xDE, // "😀"
            0x0D, 0x00, 0x0A, 0x00, // "\r\n"
            0x3D, 0xD8, 0x33, 0xDE, // "😳"
            0x46, 0x00, 0x75, 0x00, 0x6E, 0x00, 0x6E, 0x00, 0x69, 0x00, 0x65, 0x00, 0x73, 0x00 // "Funnies"
        };
        private static readonly byte[] _testBytes_UTF16BE = new byte[]
        {
            0x12, // Len
            0x00, 0x4A, 0x00, 0x75, 0x00, 0x6D, 0x00, 0x6D, 0x00, 0x79, // "Jummy"
            0xD8, 0x3D, 0xDE, 0x00, // "😀"
            0x00, 0x0D, 0x00, 0x0A, // "\r\n"
            0xD8, 0x3D, 0xDE, 0x33, // "😳"
            0x00, 0x46, 0x00, 0x75, 0x00, 0x6E, 0x00, 0x6E, 0x00, 0x69, 0x00, 0x65, 0x00, 0x73 // "Funnies"
        };
        private static readonly byte[] _testBytes_UTF32 = new byte[]
        {
            0x12, // Len
            0x4A, 0x00, 0x00, 0x00, 0x75, 0x00, 0x00, 0x00, 0x6D, 0x00, 0x00, 0x00, 0x6D, 0x00, 0x00, 0x00, 0x79, 0x00, 0x00, 0x00, // "Jummy"
            0x00, 0xF6, 0x01, 0x00, // "😀"
            0x0D, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, // "\r\n"
            0x33, 0xF6, 0x01, 0x00, // "😳"
            0x46, 0x00, 0x00, 0x00, 0x75, 0x00, 0x00, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x69, 0x00, 0x00, 0x00, 0x65, 0x00, 0x00, 0x00, 0x73, 0x00, 0x00, 0x00 // "Funnies"
        };
        #endregion

        private void Get(EncodingType encodingType, out byte[] input, out string str)
        {
            switch (encodingType)
            {
                case EncodingType.ASCII: input = _testBytes_ASCII; str = TestString_ASCII; break;
                case EncodingType.UTF7: input = _testBytes_UTF7; str = TestString_UTF; break;
                case EncodingType.UTF8: input = _testBytes_UTF8; str = TestString_UTF; break;
                case EncodingType.UTF16: input = _testBytes_UTF16; str = TestString_UTF; break;
                case EncodingType.BigEndianUTF16: input = _testBytes_UTF16BE; str = TestString_UTF; break;
                case EncodingType.UTF32: input = _testBytes_UTF32; str = TestString_UTF; break;
                default: throw new Exception();
            }
        }

        [Theory]
        [InlineData(EncodingType.ASCII)]
        [InlineData(EncodingType.UTF7)]
        [InlineData(EncodingType.UTF8)]
        [InlineData(EncodingType.UTF16)]
        [InlineData(EncodingType.BigEndianUTF16)]
        [InlineData(EncodingType.UTF32)]
        public void Read(EncodingType encodingType)
        {
            Get(encodingType, out byte[] input, out string str);
            using (var stream = new MemoryStream(input))
            using (var reader = new EndianBinaryReader(stream, endianness: Endianness.LittleEndian, encoding: encodingType))
            {
                CharObj obj = reader.ReadObject<CharObj>();
                Assert.Equal(str, obj.Str);
            }
        }

        [Theory]
        [InlineData(EncodingType.ASCII)]
        [InlineData(EncodingType.UTF7)]
        [InlineData(EncodingType.UTF8)]
        [InlineData(EncodingType.UTF16)]
        [InlineData(EncodingType.BigEndianUTF16)]
        [InlineData(EncodingType.UTF32)]
        public void Write(EncodingType encodingType)
        {
            Get(encodingType, out byte[] input, out string str);
            byte[] bytes = new byte[input.Length];
            using (var stream = new MemoryStream(bytes))
            using (var writer = new EndianBinaryWriter(stream, endianness: Endianness.LittleEndian, encoding: encodingType))
            {
                var obj = new CharObj
                {
                    Len = (byte)str.Length,
                    Str = str
                };
                writer.Write(obj);
            }
            Assert.True(bytes.SequenceEqual(input));
        }
    }
}
