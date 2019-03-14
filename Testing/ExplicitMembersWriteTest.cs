using Kermalis.EndianBinaryIO;
using System;
using System.IO;

namespace Kermalis.EndianBinaryTesting
{
    class ExplicitMembersWriteTest
    {
        public static void Test()
        {
            Console.WriteLine("EndianBinaryIO Writer Test - Explicit Members");
            Console.WriteLine();

            var bytes = new byte[4];
            using (var stream = new MemoryStream(bytes))
            using (var writer = new EndianBinaryWriter(stream, Endianness.LittleEndian))
            {
                writer.WriteObject(new MyExplicitStruct
                {
                    Int1 = 0x7090D0F0
                });

                Console.WriteLine("Little endian bytes of a \"MyExplicitStruct\":");
                TestUtils.PrintBytes(bytes);
            }

            Console.ReadKey();
        }
    }
}
