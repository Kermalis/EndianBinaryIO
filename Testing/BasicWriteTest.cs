using Kermalis.EndianBinaryIO;
using System;
using System.IO;

namespace Kermalis.EndianBinaryTesting
{
    internal sealed class BasicWriterTest
    {
        public static void Test()
        {
            Console.WriteLine("EndianBinaryIO Writer Test - Basic");
            Console.WriteLine();

            byte[] bytes = new byte[108];
            using (var stream = new MemoryStream(bytes))
            using (var writer = new EndianBinaryWriter(stream, Endianness.LittleEndian))
            {
                var obj = new MyBasicStruct
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
                writer.Write(obj.DoNotReadOrWrite);

                Console.WriteLine("Little endian bytes of a \"{0}\":", nameof(MyBasicStruct));
                TestUtils.PrintBytes(bytes);
            }

            Console.ReadKey();
        }
    }
}
