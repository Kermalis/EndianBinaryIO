using Kermalis.EndianBinaryIO;
using System;
using System.IO;
using System.Linq;
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

		#region Constants

		private const string TEST_STR_ASCII = "Jummy\r\nFunnies";
		private static readonly byte[] _testBytes_ASCII = new byte[]
		{
			0x0E, // Len
			0x4A, 0x75, 0x6D, 0x6D, 0x79, // "Jummy"
			0x0D, 0x0A, // "\r\n"
			0x46, 0x75, 0x6E, 0x6E, 0x69, 0x65, 0x73 // "Funnies"
		};
		private const string TEST_STR_UTF = "Jummy😀\r\n😳Funnies";
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

		#endregion

		private static void Get(bool ascii, out byte[] input, out string str)
		{
			if (ascii)
			{
				input = _testBytes_ASCII; str = TEST_STR_ASCII;
			}
			else
			{
				input = _testBytes_UTF16LE; str = TEST_STR_UTF;
			}
		}
		private static void TestRead<T>(bool ascii, byte[] input, string str) where T : ICharObj, new()
		{
			using (var stream = new MemoryStream(input))
			{
				T obj = new EndianBinaryReader(stream, endianness: Endianness.LittleEndian, ascii: ascii).ReadObject<T>();
				Assert.Equal(str, obj.Str);
			}
		}
		private static void TestWrite(bool ascii, byte[] input, string str)
		{
			byte[] bytes = new byte[input.Length];
			using (var stream = new MemoryStream(bytes))
			{
				var obj = new CharObj
				{
					Len = (byte)str.Length,
					Str = str,
				};
				new EndianBinaryWriter(stream, endianness: Endianness.LittleEndian, ascii: ascii).WriteObject(obj);
			}
			Assert.True(bytes.SequenceEqual(input));
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ReadDefaults(bool ascii)
		{
			Get(ascii, out byte[] input, out string str);
			TestRead<CharObj>(ascii, input, str);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void WriteDefaults(bool ascii)
		{
			Get(ascii, out byte[] input, out string str);
			TestWrite(ascii, input, str);
		}
	}
}
