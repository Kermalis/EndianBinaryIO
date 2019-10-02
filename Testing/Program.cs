namespace Kermalis.EndianBinaryTesting
{
    internal sealed class Program
    {
        private enum TestType
        {
            BasicRead,
            BasicWrite,
            LengthsRead,
            LengthsWrite
        }

        private static void Main()
        {
            TestType t = TestType.BasicRead;

            switch (t)
            {
                case TestType.BasicRead: BasicReaderTest.Test(); break;
                case TestType.BasicWrite: BasicWriterTest.Test(); break;
                case TestType.LengthsRead: LengthsReadTest.Test(); break;
                case TestType.LengthsWrite: LengthsWriteTest.Test(); break;
            }
        }
    }
}
