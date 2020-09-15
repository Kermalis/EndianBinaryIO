using Kermalis.EndianBinaryIO;
using System;
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
        private sealed class ZeroLenArrayObj
        {
            [BinaryArrayFixedLength(0)]
            public byte[] SizedArray { get; set; }

            public byte VariableLength { get; set; }
            [BinaryArrayVariableLength(nameof(VariableLength))]
            public byte[] VariableArray { get; set; }
        }

        #region Constants
        private static readonly byte[] _lengthyObjBytes = new byte[]
        {
            0x48, 0x69, 0x00, // "Hi\0"
            0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x00, // "Hello\0"
            0x48, 0x6F, 0x6C, 0x61, 0x00, // "Hola\0"

            0x53, 0x65, 0x65, 0x79, 0x61, // "Seeya"
            0x42, 0x79, 0x65, 0x00, 0x00, // "Bye\0\0"
            0x41, 0x64, 0x69, 0x6F, 0x73, // "Adios"

            0x02, // (byte)2
            0x40, 0x00, // ShortSizedEnum.Val1
            0x00, 0x08, // ShortSizedEnum.Val2
        };
        private static readonly byte[] _zeroLenArrayObjBytes = new byte[]
        {
            0x00, // (byte)0
        };
        #endregion

        [Fact]
        public void ReadLengthyObject()
        {
            MyLengthyObj obj;
            using (var stream = new MemoryStream(_lengthyObjBytes))
            using (var reader = new EndianBinaryReader(stream, Endianness.LittleEndian))
            {
                obj = reader.ReadObject<MyLengthyObj>();
            }

            Assert.Equal(3, obj.NullTerminatedStringArray.Length); // Fixed size array works
            Assert.Equal("Hi", obj.NullTerminatedStringArray[0]); // Null terminated strings
            Assert.Equal("Hello", obj.NullTerminatedStringArray[1]);
            Assert.Equal("Hola", obj.NullTerminatedStringArray[2]);

            Assert.Equal(3, obj.SizedStringArray.Length); // Fixed size array again
            Assert.Equal("Seeya", obj.SizedStringArray[0]); // Strings 5 chars long
            Assert.Equal("Bye\0\0", obj.SizedStringArray[1]);
            Assert.Equal("Adios", obj.SizedStringArray[2]);

            Assert.Equal(2, obj.VariableLengthProperty); // This determines how long the following array is
            Assert.Equal(2, obj.VariableSizedArray.Length); // Retrieves the proper size
            Assert.Equal(ShortSizedEnum.Val1, obj.VariableSizedArray[0]);
            Assert.Equal(ShortSizedEnum.Val2, obj.VariableSizedArray[1]);
        }

        [Fact]
        public void WriteLengthyObject()
        {
            byte[] bytes = new byte[_lengthyObjBytes.Length];
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
            Assert.True(bytes.SequenceEqual(_lengthyObjBytes));
        }

        [Fact]
        public void ReadZeroLenArrayObject()
        {
            ZeroLenArrayObj obj;
            using (var stream = new MemoryStream(_zeroLenArrayObjBytes))
            using (var reader = new EndianBinaryReader(stream, Endianness.LittleEndian))
            {
                obj = reader.ReadObject<ZeroLenArrayObj>();
            }

            Assert.Empty(obj.SizedArray); // Fixed size array works

            Assert.Equal(0, obj.VariableLength); // This determines how long the following array is
            Assert.Empty(obj.VariableArray); // Retrieves the proper size
        }

        [Fact]
        public void WriteZeroLenArrayObject()
        {
            byte[] bytes = new byte[_zeroLenArrayObjBytes.Length];
            using (var stream = new MemoryStream(bytes))
            using (var writer = new EndianBinaryWriter(stream, Endianness.LittleEndian))
            {
                writer.Write(new ZeroLenArrayObj
                {
                    SizedArray = Array.Empty<byte>(),

                    VariableLength = 0,
                    VariableArray = Array.Empty<byte>()
                });
            }
            Assert.True(bytes.SequenceEqual(_zeroLenArrayObjBytes));
        }
    }
}
