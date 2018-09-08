using Kermalis.EndianBinaryIO;
using System;
using System.IO;

namespace Kermalis.EndianBinaryTesting
{
    class LengthsReadTest
    {
        public static void Test()
        {
            byte[] bytes =
            {
                0x48, 0x69, 0x00,
                0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x00,
                0x48, 0x6F, 0x6C, 0x61, 0x00,

                0x53, 0x65, 0x65, 0x79, 0x61,
                0x42, 0x79, 0x65, 0x00, 0x00,
                0x41, 0x64, 0x69, 0x6F, 0x73,
            };
            var reader = new EndianBinaryReader(new MemoryStream(bytes));
            var obj = reader.ReadObject<MyLengthyStruct>();

            Console.WriteLine("EndianBinaryIO Reader Test - Lengths");
            Console.WriteLine();

            Console.WriteLine("Null Terminated String Array Length: {0}", obj.NullTerminatedStringArray.Length);
            Console.WriteLine("Array Elements:");
            foreach (var e in obj.NullTerminatedStringArray)
                Console.WriteLine("\t\"{0}\"", e);
            Console.WriteLine();
            Console.WriteLine("Sized String Array Length: {0}", obj.SizedStringArray.Length);
            Console.WriteLine("Array Elements:");
            foreach (var e in obj.SizedStringArray)
                Console.WriteLine("\t\"{0}\"", e);

            Console.ReadKey();
        }
    }
}
