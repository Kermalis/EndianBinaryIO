using Kermalis.EndianBinaryIO;
using System;
using System.IO;

namespace Kermalis.EndianBinaryTesting
{
    class ExplicitMembersReadTest
    {
        public static void Test()
        {
            byte[] bytes =
            {
                0xF0, 0xD0, 0x90, 0x70,
            };
            var reader = new EndianBinaryReader(new MemoryStream(bytes));
            var obj = reader.ReadObject<MyExplicitStruct>();
            
            Console.WriteLine("EndianBinaryIO Reader Test - Explicit Members");
            Console.WriteLine();

            Console.WriteLine("Int 1: {0}", obj.Int1);
            Console.WriteLine();
            Console.WriteLine("Short 1 {0}", obj.Short1);
            Console.WriteLine("Short 2 {0}", obj.Short2);
            Console.WriteLine();
            Console.WriteLine("Byte 1: {0}", obj.Byte1);
            Console.WriteLine("Byte 2: {0}", obj.Byte2);
            Console.WriteLine("Byte 3: {0}", obj.Byte3);
            Console.WriteLine("Byte 4: {0}", obj.Byte4);

            Console.ReadKey();
        }
    }
}
