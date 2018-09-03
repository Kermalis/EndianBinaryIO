using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EndianBinaryIO
{
    public class EndianBinaryReaderWriter : IDisposable
    {
        public Stream BaseStream { get; private set; }
        public Endianness Endianness;
        public Encoding Encoding;
        protected byte[] buffer;

        bool disposed;

        protected EndianBinaryReaderWriter(Stream baseStream, bool isReader) : this(baseStream, Endianness.LittleEndian, isReader) { }
        protected EndianBinaryReaderWriter(Stream baseStream, Endianness endianness, bool isReader) : this(baseStream, Endianness.LittleEndian, Encoding.ASCII, isReader) { }
        protected EndianBinaryReaderWriter(Stream baseStream, Endianness endianness, Encoding encoding, bool isReader)
        {
            DoNotInheritOutsideOfThisAssembly(); // Will throw an exception if inherited outside of this assembly
            if (baseStream == null)
                throw new ArgumentNullException("baseStream");
            if ((isReader && !baseStream.CanRead) || (!isReader && !baseStream.CanWrite))
                throw new ArgumentException("baseStream");
            BaseStream = baseStream;
            Endianness = endianness;
            Encoding = encoding;
        }
        ~EndianBinaryReaderWriter() => Dispose(false);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    if (BaseStream != null)
                        BaseStream.Close();
                buffer = null;
                disposed = true;
            }
        }

        public static Endianness SystemEndianness => BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;
        public static int EncodingSize(Encoding encoding) => (encoding == Encoding.Unicode || encoding == Encoding.BigEndianUnicode) ? 2 : 1;

        protected static Dictionary<string, int> supportedTypes = new Dictionary<string, int>()
        {
            { "Boolean", 0 },
            { "Byte", 1 },
            { "SByte", 2 },
            { "Char", 3 },
            { "Int16", 4 },
            { "UInt16", 5 },
            { "Int32", 6 },
            { "UInt32", 7 },
            { "Int64", 8 },
            { "UInt64", 9 },
            { "Single", 10 },
            { "Double", 11 }
        };
        protected void Flip(int byteAmount, int primitiveSize)
        {
            if (SystemEndianness != Endianness)
                for (int i = 0; i < byteAmount; i += primitiveSize)
                    Array.Reverse(buffer, i, primitiveSize);
        }

        // Prevent external inheritance
        internal virtual void DoNotInheritOutsideOfThisAssembly()
        {
            throw new Exception("Do not inherit EndianBinaryReaderWriter.");
        }
    }
}
