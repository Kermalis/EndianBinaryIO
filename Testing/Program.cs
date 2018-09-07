namespace Kermalis.EndianBinaryTesting
{
    class Program
    {
        private enum TestType
        {
            BasicRead,
            BasicWrite,
            ExplicitRead,
            ExplicitWrite
        }

        static void Main(string[] args)
        {
            TestType t = TestType.ExplicitWrite;

            switch (t)
            {
                case TestType.BasicRead: BasicReaderTest.Test(); break;
                case TestType.BasicWrite: BasicWriterTest.Test(); break;
                case TestType.ExplicitRead: ExplicitMembersReadTest.Test(); break;
                case TestType.ExplicitWrite: ExplicitMembersWriteTest.Test(); break;
            }
        }
    }
}
