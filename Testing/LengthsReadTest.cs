using Kermalis.EndianBinaryIO;
using System;
using System.IO;

namespace Kermalis.EndianBinaryTesting
{
    class LengthsReadTest
    {
        public static void Test()
        {
            Console.WriteLine("EndianBinaryIO Reader Test - Lengths");
            Console.WriteLine();

            byte[] bytes = new byte[]
            {
                0x48, 0x69, 0x00,
                0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x00,
                0x48, 0x6F, 0x6C, 0x61, 0x00,

                0x53, 0x65, 0x65, 0x79, 0x61,
                0x42, 0x79, 0x65, 0x00, 0x00,
                0x41, 0x64, 0x69, 0x6F, 0x73,

                0x02,
                0x40, 0x00,
                0x00, 0x08,
            };
            using (var reader = new EndianBinaryReader(new MemoryStream(bytes)))
            {
                MyLengthyStruct obj = reader.ReadObject<MyLengthyStruct>();

                Console.WriteLine("Null Terminated String Array Length: {0}", obj.NullTerminatedStringArray.Length);
                Console.WriteLine("Array Elements:");
                foreach (string e in obj.NullTerminatedStringArray)
                {
                    Console.WriteLine("\t\"{0}\"", e);
                }
                Console.WriteLine();
                Console.WriteLine("Sized String Array Length: {0}", obj.SizedStringArray.Length);
                Console.WriteLine("Array Elements:");
                foreach (string e in obj.SizedStringArray)
                {
                    Console.WriteLine("\t\"{0}\"", e);
                }
                Console.WriteLine();
                Console.WriteLine("Variable Length: {0}", obj.VariableLengthField);
                Console.WriteLine("Variable Sized Array Length: {0}", obj.VariableSizedArray.Length);
                Console.WriteLine("Array Elements:");
                foreach (ShortSizedEnum e in obj.VariableSizedArray)
                {
                    Console.WriteLine("\t{0}", e);
                }
            }

            Console.ReadKey();
        }
    }
}
