using Kermalis.EndianBinaryIO;
using System;
using System.IO;

namespace Kermalis.EndianBinaryTesting
{
    class ExplicitMembersWriteTest
    {
        public static void Test()
        {
            var bytes = new byte[4];
            var writer = new EndianBinaryWriter(new MemoryStream(bytes));
            writer.WriteObject(new MyExplicitStruct()
            {
                Int1 = 0x7090D0F0
            });

            Console.WriteLine("EndianBinaryIO Writer Test - Explicit Members");
            Console.WriteLine();

            Console.WriteLine("Little endian bytes of a \"MyExplicitStruct\":");
            TestUtils.PrintBytes(bytes);

            Console.ReadKey();
        }
    }
}
