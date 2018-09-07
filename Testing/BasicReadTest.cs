using Kermalis.EndianBinaryIO;
using System;
using System.IO;

namespace Kermalis.EndianBinaryTesting
{
    class BasicReaderTest
    {
        public static void Test()
        {
            byte[] bytes =
            {
                0x02,
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

                0x42, 0x00, 0x69, 0x00, 0x6E, 0x00, 0x61, 0x00, 0x72, 0x00, 0x79, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            };
            var reader = new EndianBinaryReader(new MemoryStream(bytes));
            var obj = reader.ReadObject<MyStruct>();

            Console.WindowHeight = 32;
            Console.WriteLine("EndianBinaryIO Reader Test - Basic");
            Console.WriteLine();

            Console.WriteLine("Major Version: {0}", obj.VersionMajor);
            Console.WriteLine("Minor Version: {0}", obj.VersionMinor);
            Console.WriteLine();
            Console.WriteLine("Ignored Member: {0}", obj.DoNotReadOrWrite);
            Console.WriteLine();
            Console.WriteLine("Array Length: {0}", obj.ArrayWith16Elements.Length);
            Console.WriteLine("Array elements:");
            foreach (var e in obj.ArrayWith16Elements)
                Console.WriteLine("\t{0}", e);
            Console.WriteLine();
            Console.WriteLine("Long Bool: {0}", obj.LongBool);
            Console.WriteLine();
            Console.WriteLine("Null-Terminated String: \"{0}\"", obj.NullTerminatedASCIIString);
            Console.WriteLine();
            Console.WriteLine("UTF-16 String: \"{0}\"", obj.UTF16String);

            Console.ReadKey();
        }
    }
}
