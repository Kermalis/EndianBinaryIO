using Kermalis.EndianBinaryIO;
using System;
using System.IO;

namespace Kermalis.EndianBinaryTesting
{
    class LengthsWriteTest
    {
        public static void Test()
        {
            var bytes = new byte[40];
            var writer = new EndianBinaryWriter(new MemoryStream(bytes));
            writer.WriteObject(new MyLengthyStruct()
            {
                NullTerminatedStringArray = new string[]
                {
                    "Hi", "Hello", "Hola"
                },

                SizedStringArray = new string[]
                {
                    "Seeya", "Bye", "Adios"
                }
            });

            Console.WriteLine("EndianBinaryIO Writer Test - Lengths");
            Console.WriteLine();

            Console.WriteLine("Little endian bytes of a \"MyLengthyStruct\":");
            TestUtils.PrintBytes(bytes);

            Console.ReadKey();
        }
    }
}
