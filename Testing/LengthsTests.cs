using Kermalis.EndianBinaryIO;
using System.IO;
using System.Linq;
using Xunit;

namespace Kermalis.EndianBinaryIOTests
{
    public sealed class LengthsTests
    {
        private sealed class MyLengthyObj
        {
            [BinaryArrayFixedLength(3)]
            [BinaryEncoding(EncodingType.ASCII)]
            [BinaryStringNullTerminated(true)]
            public string[] NullTerminatedStringArray { get; set; }

            [BinaryArrayFixedLength(3)]
            [BinaryEncoding(EncodingType.ASCII)]
            [BinaryStringFixedLength(5)]
            public string[] SizedStringArray { get; set; }

            public byte VariableLengthProperty { get; set; }
            [BinaryArrayVariableLength(nameof(VariableLengthProperty))]
            public ShortSizedEnum[] VariableSizedArray { get; set; }
        }

        private static readonly byte[] _bytes = new byte[34]
        {
            0x48, 0x69, 0x00,
            0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x00,
            0x48, 0x6F, 0x6C, 0x61, 0x00,

            0x53, 0x65, 0x65, 0x79, 0x61,
            0x42, 0x79, 0x65, 0x00, 0x00,
            0x41, 0x64, 0x69, 0x6F, 0x73,

            0x02,
            0x40, 0x00,
            0x00, 0x08
        };

        [Fact]
        public void ReadObject()
        {
            MyLengthyObj obj;
            using (var stream = new MemoryStream(_bytes))
            using (var reader = new EndianBinaryReader(stream, Endianness.LittleEndian))
            {
                obj = reader.ReadObject<MyLengthyObj>();
            }

            Assert.True(obj.NullTerminatedStringArray.Length == 3); // Fixed size array works
            Assert.True(obj.NullTerminatedStringArray[0] == "Hi"); // Null terminated strings
            Assert.True(obj.NullTerminatedStringArray[1] == "Hello");
            Assert.True(obj.NullTerminatedStringArray[2] == "Hola");

            Assert.True(obj.SizedStringArray.Length == 3); // Fixed size array again
            Assert.True(obj.SizedStringArray[0] == "Seeya"); // Strings 5 chars long
            Assert.True(obj.SizedStringArray[1] == "Bye\0\0");
            Assert.True(obj.SizedStringArray[2] == "Adios");

            Assert.True(obj.VariableLengthProperty == 2); // This determines how long the following array is
            Assert.True(obj.VariableSizedArray.Length == 2); // Retrieves the proper size
            Assert.True(obj.VariableSizedArray[0] == ShortSizedEnum.Val1);
            Assert.True(obj.VariableSizedArray[1] == ShortSizedEnum.Val2);
        }

        [Fact]
        public void WriteObject()
        {
            byte[] bytes = new byte[34];
            using (var stream = new MemoryStream(bytes))
            using (var writer = new EndianBinaryWriter(stream, Endianness.LittleEndian))
            {
                writer.Write(new MyLengthyObj
                {
                    NullTerminatedStringArray = new string[3]
                    {
                        "Hi", "Hello", "Hola"
                    },

                    SizedStringArray = new string[3]
                    {
                        "Seeya", "Bye", "Adios"
                    },

                    VariableLengthProperty = 2,
                    VariableSizedArray = new ShortSizedEnum[2]
                    {
                        ShortSizedEnum.Val1, ShortSizedEnum.Val2
                    }
                });
            }
            Assert.True(bytes.SequenceEqual(_bytes));
        }
    }
}
