using Kermalis.EndianBinaryIO;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Kermalis.EndianBinaryIOTests
{
    public sealed class EncodingTests
    {
        private interface ICharObj
        {
            string Str { get; set; }
        }
        private sealed class CharObj : ICharObj
        {
            public byte Len { get; set; }
            [BinaryStringVariableLength(nameof(Len))]
            public string Str { get; set; }
        }
        private sealed class CustomEncodingCharObj : ICharObj
        {
            public byte Len { get; set; }
            [BinaryEncoding("CHPOOSH")]
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
        private static readonly byte[] _testBytes_UTF16LE = new byte[]
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
        private static readonly byte[] _testBytes_UTF32LE = new byte[]
        {
            0x12, // Len
            0x4A, 0x00, 0x00, 0x00, 0x75, 0x00, 0x00, 0x00, 0x6D, 0x00, 0x00, 0x00, 0x6D, 0x00, 0x00, 0x00, 0x79, 0x00, 0x00, 0x00, // "Jummy"
            0x00, 0xF6, 0x01, 0x00, // "😀"
            0x0D, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, // "\r\n"
            0x33, 0xF6, 0x01, 0x00, // "😳"
            0x46, 0x00, 0x00, 0x00, 0x75, 0x00, 0x00, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x69, 0x00, 0x00, 0x00, 0x65, 0x00, 0x00, 0x00, 0x73, 0x00, 0x00, 0x00 // "Funnies"
        };
        private static readonly byte[] _testBytes_UTF32BE = new byte[]
        {
            0x12, // Len
            0x00, 0x00, 0x00, 0x4A, 0x00, 0x00, 0x00, 0x75, 0x00, 0x00, 0x00, 0x6D, 0x00, 0x00, 0x00, 0x6D, 0x00, 0x00, 0x00, 0x79, // "Jummy"
            0x00, 0x01, 0xF6, 0x00, // "😀"
            0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x0A, // "\r\n"
            0x00, 0x01, 0xF6, 0x33, // "😳"
            0x00, 0x00, 0x00, 0x46, 0x00, 0x00, 0x00, 0x75, 0x00, 0x00, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x6E, 0x00, 0x00, 0x00, 0x69, 0x00, 0x00, 0x00, 0x65, 0x00, 0x00, 0x00, 0x73 // "Funnies"
        };
        #endregion

        #region Chpoosh Encoding
        private sealed class ChpooshProvider : EncodingProvider
        {
            private readonly Encoding _encoding;
            public ChpooshProvider(Encoding encoding)
            {
                _encoding = encoding;
            }

            public override Encoding GetEncoding(int codepage)
            {
                return default;
            }
            public override Encoding GetEncoding(string name)
            {
                if (name == "CHPOOSH")
                {
                    return _encoding;
                }
                return default;
            }
        }
        private Encoding CreateChpoosh()
        {
            return new UTF32Encoding(true, false, true);
        }
        #endregion

        private void Get(string encodingType, out Encoding encoding, out byte[] input, out string str)
        {
            switch (encodingType)
            {
                case "ASCII": encoding = Encoding.ASCII; input = _testBytes_ASCII; str = TestString_ASCII; break;
                case "UTF7": encoding = Encoding.UTF7; input = _testBytes_UTF7; str = TestString_UTF; break;
                case "UTF8": encoding = Encoding.UTF8; input = _testBytes_UTF8; str = TestString_UTF; break;
                case "UTF16-LE": encoding = Encoding.Unicode; input = _testBytes_UTF16LE; str = TestString_UTF; break;
                case "UTF16-BE": encoding = Encoding.BigEndianUnicode; input = _testBytes_UTF16BE; str = TestString_UTF; break;
                case "UTF32-LE": encoding = Encoding.UTF32; input = _testBytes_UTF32LE; str = TestString_UTF; break;
                default: throw new Exception();
            }
        }
        private void TestRead<T>(Encoding encoding, byte[] input, string str) where T : ICharObj, new()
        {
            using (var stream = new MemoryStream(input))
            {
                T obj = new EndianBinaryReader(stream, encoding, endianness: Endianness.LittleEndian).ReadObject<T>();
                Assert.Equal(str, obj.Str);
            }
        }
        private void TestWrite(Encoding encoding, byte[] input, string str)
        {
            byte[] bytes = new byte[input.Length];
            using (var stream = new MemoryStream(bytes))
            {
                var obj = new CharObj
                {
                    Len = (byte)str.Length,
                    Str = str
                };
                new EndianBinaryWriter(stream, encoding, endianness: Endianness.LittleEndian).Write(obj);
            }
            Assert.True(bytes.SequenceEqual(input));
        }

        [Theory]
        [InlineData("ASCII")]
        [InlineData("UTF7")]
        [InlineData("UTF8")]
        [InlineData("UTF16-LE")]
        [InlineData("UTF16-BE")]
        [InlineData("UTF32-LE")]
        public void ReadDefaults(string encodingType)
        {
            Get(encodingType, out Encoding encoding, out byte[] input, out string str);
            TestRead<CharObj>(encoding, input, str);
        }

        [Fact]
        public void ReadCustom()
        {
            Encoding encoding = CreateChpoosh();
            Encoding.RegisterProvider(new ChpooshProvider(encoding));
            TestRead<CustomEncodingCharObj>(encoding, _testBytes_UTF32BE, TestString_UTF);
        }

        [Theory]
        [InlineData("ASCII")]
        [InlineData("UTF7")]
        [InlineData("UTF8")]
        [InlineData("UTF16-LE")]
        [InlineData("UTF16-BE")]
        [InlineData("UTF32-LE")]
        public void WriteDefaults(string encodingType)
        {
            Get(encodingType, out Encoding encoding, out byte[] input, out string str);
            TestWrite(encoding, input, str);
        }

        [Fact]
        public void WriteCustom()
        {
            Encoding encoding = CreateChpoosh();
            TestWrite(encoding, _testBytes_UTF32BE, TestString_UTF);
        }
    }
}
