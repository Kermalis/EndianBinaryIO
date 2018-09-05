namespace Kermalis.EndianBinaryTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            bool readTest = false;
            if (readTest)
            {
                BasicReaderTest.Test();
            }
            else
            {
                BasicWriterTest.Test();
            }
        }
    }
}
