using System;
using System.Collections.Generic;
using System.IO;

namespace Kermalis.EndianBinaryIO
{
    public class EndianBinaryBase : IDisposable
    {
        public Stream BaseStream { get; private set; }
        public Endianness Endianness;
        public EncodingType Encoding;
        protected byte[] buffer;

        bool disposed;
        
        protected EndianBinaryBase(Stream baseStream, Endianness endianness, EncodingType encoding, bool isReader)
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
        ~EndianBinaryBase() => Dispose(false);
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
            { "Double", 11 },
            { "Decimal", 12 },
        };
        protected void Flip(int byteAmount, int primitiveSize)
        {
            if (Utils.SystemEndianness != Endianness)
                for (int i = 0; i < byteAmount; i += primitiveSize)
                    Array.Reverse(buffer, i, primitiveSize);
        }

        // Prevent external inheritance
        internal virtual void DoNotInheritOutsideOfThisAssembly()
        {
            throw new Exception("Do not inherit EndianBinaryBase.");
        }
    }
}
