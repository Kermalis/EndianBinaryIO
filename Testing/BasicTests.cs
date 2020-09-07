using Kermalis.EndianBinaryIO;
using System.IO;
using System.Linq;
using Xunit;

namespace Kermalis.EndianBinaryIOTests
{
    public sealed class BasicTests
    {
        private sealed class MyBasicObj
        {
            // Properties
            public ShortSizedEnum Type { get; set; }
            public short Version { get; set; }

            // Property that is ignored when reading and writing
            [BinaryIgnore(true)]
            public ByteSizedEnum DoNotReadOrWrite { get; set; }

            // Arrays work as well
            [BinaryArrayFixedLength(16)]
            public uint[] ArrayWith16Elements { get; set; }

            // Boolean that occupies 4 bytes instead of one
            [BinaryBooleanSize(BooleanSize.U32)]
            public bool Bool32 { get; set; }

            // String encoded in ASCII
            // Reads chars until the stream encounters a '\0'
            // Writing will append a '\0' at the end of the string
            [BinaryEncoding(EncodingType.ASCII)]
            [BinaryStringNullTerminated(true)]
            public string NullTerminatedASCIIString { get; set; }

            // String encoded in UTF-16 that will only read/write 10 chars
            [BinaryEncoding(EncodingType.UTF16)]
            [BinaryStringFixedLength(10)]
            public string UTF16String { get; set; }
        }

        private static readonly byte[] _bytes = new byte[107]
        {
            0x00, 0x08,
            0xFF, 0x01,

            0x00, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00,
            0x02, 0x00, 0x00, 0x00,
            0x03, 0x00, 0x00, 0x00,
            0x04, 0x00, 0x00, 0x00,
            0x05, 0x00, 0x00, 0x00,
            0x06, 0x00, 0x00, 0x00,
            0x07, 0x00, 0x00, 0x00,
            0x08, 0x00, 0x00, 0x00,
            0x09, 0x00, 0x00, 0x00,
            0x0A, 0x00, 0x00, 0x00,
            0x0B, 0x00, 0x00, 0x00,
            0x0C, 0x00, 0x00, 0x00,
            0x0D, 0x00, 0x00, 0x00,
            0x0E, 0x00, 0x00, 0x00,
            0x0F, 0x00, 0x00, 0x00,

            0x00, 0x00, 0x00, 0x00,

            0x45, 0x6E, 0x64, 0x69, 0x61, 0x6E, 0x42, 0x69, 0x6E, 0x61, 0x72, 0x79, 0x49, 0x4F, 0x00,

            0x4B, 0x00, 0x65, 0x00, 0x72, 0x00, 0x6D, 0x00, 0x61, 0x00, 0x6C, 0x00, 0x69, 0x00, 0x73, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        [Fact]
        public void ReadObject()
        {
            MyBasicObj obj;
            using (var stream = new MemoryStream(_bytes))
            using (var reader = new EndianBinaryReader(stream, endianness: Endianness.LittleEndian))
            {
                obj = reader.ReadObject<MyBasicObj>();
            }

            Assert.True(obj.Type == ShortSizedEnum.Val2); // Enum works
            Assert.True(obj.Version == 0x1FF); // short works

            Assert.True(obj.DoNotReadOrWrite == default); // Ignored

            Assert.True(obj.ArrayWith16Elements.Length == 16); // Fixed size array works
            for (uint i = 0; i < 16; i++)
            {
                Assert.True(obj.ArrayWith16Elements[i] == i); // Array works
            }

            Assert.False(obj.Bool32); // bool32 works

            Assert.True(obj.NullTerminatedASCIIString == "EndianBinaryIO"); // Stops reading at null terminator
            Assert.True(obj.UTF16String == "Kermalis\0\0"); // Fixed size (10 chars) utf16
        }

        [Fact]
        public void ReadManually()
        {
            MyBasicObj obj;
            using (var stream = new MemoryStream(_bytes))
            using (var reader = new EndianBinaryReader(stream, endianness: Endianness.LittleEndian, booleanSize: BooleanSize.U32))
            {
                obj = new MyBasicObj();

                obj.Type = reader.ReadEnum<ShortSizedEnum>();
                Assert.True(obj.Type == ShortSizedEnum.Val2); // Enum works
                obj.Version = reader.ReadInt16();
                Assert.True(obj.Version == 0x1FF); // short works

                obj.ArrayWith16Elements = reader.ReadUInt32s(16);
                Assert.True(obj.ArrayWith16Elements.Length == 16); // Fixed size array works
                for (uint i = 0; i < 16; i++)
                {
                    Assert.True(obj.ArrayWith16Elements[i] == i); // Array works
                }

                obj.Bool32 = reader.ReadBoolean();
                Assert.False(obj.Bool32); // bool32 works

                obj.NullTerminatedASCIIString = reader.ReadStringNullTerminated(EncodingType.ASCII);
                Assert.True(obj.NullTerminatedASCIIString == "EndianBinaryIO"); // Stops reading at null terminator
                obj.UTF16String = reader.ReadString(10, EncodingType.UTF16);
                Assert.True(obj.UTF16String == "Kermalis\0\0"); // Fixed size (10 chars) utf16
            }
        }

        [Fact]
        public void WriteObject()
        {
            byte[] bytes = new byte[107];
            using (var stream = new MemoryStream(bytes))
            using (var writer = new EndianBinaryWriter(stream, endianness: Endianness.LittleEndian))
            {
                var obj = new MyBasicObj
                {
                    Type = ShortSizedEnum.Val2,
                    Version = 511,

                    DoNotReadOrWrite = ByteSizedEnum.Val1,

                    ArrayWith16Elements = new uint[16]
                    {
                        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
                    },

                    Bool32 = false,

                    NullTerminatedASCIIString = "EndianBinaryIO",

                    UTF16String = "Kermalis"
                };
                writer.Write(obj);
            }

            Assert.True(bytes.SequenceEqual(_bytes));
        }

        [Fact]
        public void WriteManually()
        {
            byte[] bytes = new byte[107];
            using (var stream = new MemoryStream(bytes))
            using (var writer = new EndianBinaryWriter(stream, endianness: Endianness.LittleEndian, booleanSize: BooleanSize.U32))
            {
                var obj = new MyBasicObj
                {
                    Type = ShortSizedEnum.Val2,
                    Version = 511,

                    DoNotReadOrWrite = ByteSizedEnum.Val1,

                    ArrayWith16Elements = new uint[16]
                    {
                        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
                    },

                    Bool32 = false,

                    NullTerminatedASCIIString = "EndianBinaryIO",

                    UTF16String = "Kermalis\0\0"
                };

                writer.Write(obj.Type);
                writer.Write(obj.Version);
                writer.Write(obj.ArrayWith16Elements);
                writer.Write(obj.Bool32);
                writer.Write(obj.NullTerminatedASCIIString, true, EncodingType.ASCII);
                writer.Write(obj.UTF16String, 10, EncodingType.UTF16);
            }

            Assert.True(bytes.SequenceEqual(_bytes));
        }
    }
}
