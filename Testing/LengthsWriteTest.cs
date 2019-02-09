using Kermalis.EndianBinaryIO;
using System;
using System.IO;

namespace Kermalis.EndianBinaryTesting
{
    class LengthsWriteTest
    {
        public static void Test()
        {
            Console.WriteLine("EndianBinaryIO Writer Test - Lengths");
            Console.WriteLine();

            var bytes = new byte[40];
            using (var writer = new EndianBinaryWriter(new MemoryStream(bytes)))
            {
                writer.WriteObject(new MyLengthyStruct()
                {
                    NullTerminatedStringArray = new string[]
                    {
                        "Hi", "Hello", "Hola"
                    },

                    SizedStringArray = new string[]
                    {
                        "Seeya", "Bye", "Adios"
                    },

                    VariableLengthField = 2,
                    VariableSizedArray = new ShortSizedEnum[]
                    {
                        ShortSizedEnum.Val1, ShortSizedEnum.Val2
                    }
                });

                Console.WriteLine("Little endian bytes of a \"MyLengthyStruct\":");
                TestUtils.PrintBytes(bytes);
            }

            Console.ReadKey();
        }
    }
}
