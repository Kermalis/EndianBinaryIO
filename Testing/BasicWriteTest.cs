using Kermalis.EndianBinaryIO;
using System;
using System.IO;

namespace Kermalis.EndianBinaryTesting
{
    class BasicWriterTest
    {
        public static void Test()
        {
            var bytes = new byte[150];
            var writer = new EndianBinaryWriter(new MemoryStream(bytes));
            writer.WriteObject(new MyStruct()
            {
                VersionMajor = 2,
                VersionMinor = 511,

                ArrayWith16Elements = new uint[16]
                {
                    0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
                },

                LongBool = false,

                NullTerminatedASCIIString = "EndianBinaryIO",

                UTF16String = "Binary"
            });

            Console.WriteLine("EndianBinaryIO Writer Test - Basic");
            Console.WriteLine();

            Console.WriteLine("Little endian bytes of a \"MyStruct\":");
            TestUtils.PrintBytes(bytes);

            Console.ReadKey();
        }
    }
}
