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

            var bytes = new byte[34];
            using (var stream = new MemoryStream(bytes))
            using (var writer = new EndianBinaryWriter(stream, Endianness.LittleEndian))
            {
                writer.WriteObject(new MyLengthyStruct
                {
                    NullTerminatedStringArray = new string[3]
                    {
                        "Hi", "Hello", "Hola"
                    },

                    SizedStringArray = new string[3]
                    {
                        "Seeya", "Bye", "Adios"
                    },

                    VariableLengthProperty = 2,
                    VariableSizedArray = new ShortSizedEnum[2]
                    {
                        ShortSizedEnum.Val1, ShortSizedEnum.Val2
                    }
                });

                Console.WriteLine("Little endian bytes of a \"{0}\":", nameof(MyLengthyStruct));
                TestUtils.PrintBytes(bytes);
            }

            Console.ReadKey();
        }
    }
}
