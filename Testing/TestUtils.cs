using System;

namespace Kermalis.EndianBinaryTesting
{
    class TestUtils
    {
        public static void PrintBytes(byte[] bytes, int perRow = 8)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                Console.Write("0x{0:X2}", bytes[i]);
                if (i != bytes.Length - 1)
                {
                    Console.Write(',');
                    if (i % perRow == perRow - 1)
                    {
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
            }
            Console.WriteLine();
        }
    }
}
