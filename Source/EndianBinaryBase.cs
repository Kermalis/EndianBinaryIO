using System;
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
            {
                throw new ArgumentNullException(nameof(baseStream));
            }
            if ((isReader && !baseStream.CanRead) || (!isReader && !baseStream.CanWrite))
            {
                throw new ArgumentException(nameof(baseStream));
            }
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
                {
                    if (BaseStream != null)
                    {
                        BaseStream.Close();
                    }
                }
                buffer = null;
                disposed = true;
            }
        }

        protected void Flip(int byteCount, int primitiveSize)
        {
            if (Utils.SystemEndianness != Endianness)
            {
                for (int i = 0; i < byteCount; i += primitiveSize)
                {
                    Array.Reverse(buffer, i, primitiveSize);
                }
            }
        }

        // Prevent external inheritance
        internal virtual void DoNotInheritOutsideOfThisAssembly()
        {
            throw new Exception("Do not inherit EndianBinaryBase.");
        }
    }
}
