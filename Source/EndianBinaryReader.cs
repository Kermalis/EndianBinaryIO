using System;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Kermalis.EndianBinaryIO
{
	public partial class EndianBinaryReader
	{
		protected const int BUF_LEN = 64; // Must be a multiple of 64

		public Stream Stream { get; }
		private Endianness _endianness;
		public Endianness Endianness
		{
			get => _endianness;
			set
			{
				if (value >= Endianness.MAX)
				{
					throw new ArgumentOutOfRangeException(nameof(value), value, null);
				}
				_endianness = value;
			}
		}
		private BooleanSize _booleanSize;
		public BooleanSize BooleanSize
		{
			get => _booleanSize;
			set
			{
				if (value >= BooleanSize.MAX)
				{
					throw new ArgumentOutOfRangeException(nameof(value), value, null);
				}
				_booleanSize = value;
			}
		}
		public bool ASCII { get; set; }

		protected readonly byte[] _buffer;

		public EndianBinaryReader(Stream stream, Endianness endianness = Endianness.LittleEndian, BooleanSize booleanSize = BooleanSize.U8, bool ascii = false)
		{
			if (!stream.CanRead)
			{
				throw new ArgumentOutOfRangeException(nameof(stream), "Stream is not open for reading.");
			}

			Stream = stream;
			Endianness = endianness;
			BooleanSize = booleanSize;
			ASCII = ascii;
			_buffer = new byte[BUF_LEN];
		}

		protected delegate void ReadArrayMethod<TDest>(ReadOnlySpan<byte> src, Span<TDest> dest, Endianness endianness);
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		protected void ReadArray<TDest>(Span<TDest> dest, int elementSize, ReadArrayMethod<TDest> readArray)
		{
			int numBytes = dest.Length * elementSize;
			int start = 0;
			while (numBytes != 0)
			{
				int consumeBytes = Math.Min(numBytes, BUF_LEN);

				Span<byte> buffer = _buffer.AsSpan(0, consumeBytes);
				ReadBytes(buffer);
				readArray(buffer, dest.Slice(start, consumeBytes / elementSize), Endianness);

				numBytes -= consumeBytes;
				start += consumeBytes / elementSize;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private void ReadBoolArray(Span<bool> dest, int boolSize)
		{
			int numBytes = dest.Length * boolSize;
			int start = 0;
			while (numBytes != 0)
			{
				int consumeBytes = Math.Min(numBytes, BUF_LEN);

				Span<byte> buffer = _buffer.AsSpan(0, consumeBytes);
				ReadBytes(buffer);
				EndianBinaryPrimitives.ReadBooleans(buffer, dest.Slice(start, consumeBytes / boolSize), Endianness, boolSize);

				numBytes -= consumeBytes;
				start += consumeBytes / boolSize;
			}
		}

		public byte PeekByte()
		{
			long offset = Stream.Position;

			Span<byte> buffer = _buffer.AsSpan(0, 1);
			ReadBytes(buffer);

			Stream.Position = offset;
			return buffer[0];
		}

		public sbyte ReadSByte()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 1);
			ReadBytes(buffer);
			return (sbyte)buffer[0];
		}
		public void ReadSBytes(Span<sbyte> dest)
		{
			Span<byte> buffer = MemoryMarshal.Cast<sbyte, byte>(dest);
			ReadBytes(buffer);
		}
		public byte ReadByte()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 1);
			ReadBytes(buffer);
			return buffer[0];
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void ReadBytes(Span<byte> dest)
		{
			if (Stream.Read(dest) != dest.Length)
			{
				throw new EndOfStreamException();
			}
		}
		public short ReadInt16()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 2);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadInt16(buffer, Endianness);
		}
		public void ReadInt16s(Span<short> dest)
		{
			ReadArray(dest, 2, EndianBinaryPrimitives.ReadInt16s);
		}
		public ushort ReadUInt16()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 2);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadUInt16(buffer, Endianness);
		}
		public void ReadUInt16s(Span<ushort> dest)
		{
			ReadArray(dest, 2, EndianBinaryPrimitives.ReadUInt16s);
		}
		public int ReadInt32()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 4);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadInt32(buffer, Endianness);
		}
		public void ReadInt32s(Span<int> dest)
		{
			ReadArray(dest, 4, EndianBinaryPrimitives.ReadInt32s);
		}
		public uint ReadUInt32()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 4);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadUInt32(buffer, Endianness);
		}
		public void ReadUInt32s(Span<uint> dest)
		{
			ReadArray(dest, 4, EndianBinaryPrimitives.ReadUInt32s);
		}
		public long ReadInt64()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 8);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadInt64(buffer, Endianness);
		}
		public void ReadInt64s(Span<long> dest)
		{
			ReadArray(dest, 8, EndianBinaryPrimitives.ReadInt64s);
		}
		public ulong ReadUInt64()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 8);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadUInt64(buffer, Endianness);
		}
		public void ReadUInt64s(Span<ulong> dest)
		{
			ReadArray(dest, 8, EndianBinaryPrimitives.ReadUInt64s);
		}

		public Half ReadHalf()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 2);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadHalf(buffer, Endianness);
		}
		public void ReadHalves(Span<Half> dest)
		{
			ReadArray(dest, 2, EndianBinaryPrimitives.ReadHalves);
		}
		public float ReadSingle()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 4);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadSingle(buffer, Endianness);
		}
		public void ReadSingles(Span<float> dest)
		{
			ReadArray(dest, 4, EndianBinaryPrimitives.ReadSingles);
		}
		public double ReadDouble()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 8);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadDouble(buffer, Endianness);
		}
		public void ReadDoubles(Span<double> dest)
		{
			ReadArray(dest, 8, EndianBinaryPrimitives.ReadDoubles);
		}
		public decimal ReadDecimal()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 16);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadDecimal(buffer, Endianness);
		}
		public void ReadDecimals(Span<decimal> dest)
		{
			ReadArray(dest, 16, EndianBinaryPrimitives.ReadDecimals);
		}

		public bool ReadBoolean()
		{
			int elementSize = EndianBinaryPrimitives.GetBytesForBooleanSize(BooleanSize);
			Span<byte> buffer = _buffer.AsSpan(0, elementSize);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadBoolean(buffer, Endianness, elementSize);
		}
		public void ReadBooleans(Span<bool> dest)
		{
			int elementSize = EndianBinaryPrimitives.GetBytesForBooleanSize(BooleanSize);
			ReadBoolArray(dest, elementSize);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public TEnum ReadEnum<TEnum>() where TEnum : unmanaged, Enum
		{
			int size = Unsafe.SizeOf<TEnum>();
			if (size == 1)
			{
				byte b = ReadByte();
				return Unsafe.As<byte, TEnum>(ref b);
			}
			if (size == 2)
			{
				ushort s = ReadUInt16();
				return Unsafe.As<ushort, TEnum>(ref s);
			}
			if (size == 4)
			{
				uint i = ReadUInt32();
				return Unsafe.As<uint, TEnum>(ref i);
			}
			ulong l = ReadUInt64();
			return Unsafe.As<ulong, TEnum>(ref l);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void ReadEnums<TEnum>(Span<TEnum> dest) where TEnum : unmanaged, Enum
		{
			int size = Unsafe.SizeOf<TEnum>();
			if (size == 1)
			{
				ReadBytes(MemoryMarshal.Cast<TEnum, byte>(dest));
			}
			else if (size == 2)
			{
				ReadUInt16s(MemoryMarshal.Cast<TEnum, ushort>(dest));
			}
			else if (size == 4)
			{
				ReadUInt32s(MemoryMarshal.Cast<TEnum, uint>(dest));
			}
			else
			{
				ReadUInt64s(MemoryMarshal.Cast<TEnum, ulong>(dest));
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public object ReadEnum(Type enumType)
		{
			// Type.IsEnum is also false for base Enum type, so don't worry about it
			Type underlyingType = Enum.GetUnderlyingType(enumType);
			switch (Type.GetTypeCode(underlyingType))
			{
				case TypeCode.SByte: return Enum.ToObject(enumType, ReadSByte());
				case TypeCode.Byte: return Enum.ToObject(enumType, ReadByte());
				case TypeCode.Int16: return Enum.ToObject(enumType, ReadInt16());
				case TypeCode.UInt16: return Enum.ToObject(enumType, ReadUInt16());
				case TypeCode.Int32: return Enum.ToObject(enumType, ReadInt32());
				case TypeCode.UInt32: return Enum.ToObject(enumType, ReadUInt32());
				case TypeCode.Int64: return Enum.ToObject(enumType, ReadInt64());
				case TypeCode.UInt64: return Enum.ToObject(enumType, ReadUInt64());
			}
			throw new ArgumentOutOfRangeException(nameof(enumType), enumType, null);
		}

		public DateTime ReadDateTime()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 8);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadDateTime(buffer, Endianness);
		}
		public void ReadDateTimes(Span<DateTime> dest)
		{
			ReadArray(dest, 8, EndianBinaryPrimitives.ReadDateTimes);
		}
		public DateOnly ReadDateOnly()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 4);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadDateOnly(buffer, Endianness);
		}
		public void ReadDateOnlys(Span<DateOnly> dest)
		{
			ReadArray(dest, 4, EndianBinaryPrimitives.ReadDateOnlys);
		}
		public TimeOnly ReadTimeOnly()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 8);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadTimeOnly(buffer, Endianness);
		}
		public void ReadTimeOnlys(Span<TimeOnly> dest)
		{
			ReadArray(dest, 8, EndianBinaryPrimitives.ReadTimeOnlys);
		}

		public Vector2 ReadVector2()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 8);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadVector2(buffer, Endianness);
		}
		public void ReadVector2s(Span<Vector2> dest)
		{
			ReadArray(dest, 8, EndianBinaryPrimitives.ReadVector2s);
		}
		public Vector3 ReadVector3()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 12);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadVector3(buffer, Endianness);
		}
		public void ReadVector3s(Span<Vector3> dest)
		{
			ReadArray(dest, 12, EndianBinaryPrimitives.ReadVector3s);
		}
		public Vector4 ReadVector4()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 16);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadVector4(buffer, Endianness);
		}
		public void ReadVector4s(Span<Vector4> dest)
		{
			ReadArray(dest, 16, EndianBinaryPrimitives.ReadVector4s);
		}
		public Quaternion ReadQuaternion()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 16);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadQuaternion(buffer, Endianness);
		}
		public void ReadQuaternions(Span<Quaternion> dest)
		{
			ReadArray(dest, 16, EndianBinaryPrimitives.ReadQuaternions);
		}
		public Matrix4x4 ReadMatrix4x4()
		{
			Span<byte> buffer = _buffer.AsSpan(0, 64);
			ReadBytes(buffer);
			return EndianBinaryPrimitives.ReadMatrix4x4(buffer, Endianness);
		}
		public void ReadMatrix4x4s(Span<Matrix4x4> dest)
		{
			ReadArray(dest, 64, EndianBinaryPrimitives.ReadMatrix4x4s);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public char ReadChar()
		{
			return ASCII ? (char)ReadByte() : (char)ReadUInt16();
		}
		public void ReadChars(Span<char> dest)
		{
			if (ASCII)
			{
				Span<byte> buffer = MemoryMarshal.Cast<char, byte>(dest).Slice(dest.Length);
				ReadBytes(buffer);
				for (int i = 0; i < dest.Length; i++)
				{
					dest[i] = (char)buffer[i];
				}
			}
			else
			{
				Span<ushort> buffer = MemoryMarshal.Cast<char, ushort>(dest);
				ReadUInt16s(buffer);
			}
		}
		public char[] ReadChars_TrimNullTerminators(int charCount)
		{
			char[] chars = new char[charCount];
			ReadChars(chars);
			Utils.TrimNullTerminators(ref chars);
			return chars;
		}
		public string ReadString_Count(int charCount)
		{
			void Create(Span<char> dest, byte _)
			{
				ReadChars(dest);
			}
			return string.Create(charCount, byte.MinValue, Create);
		}
		public void ReadStrings_Count(Span<string> dest, int charCount)
		{
			for (int i = 0; i < dest.Length; i++)
			{
				dest[i] = ReadString_Count(charCount);
			}
		}
		public string ReadString_Count_TrimNullTerminators(int charCount)
		{
			return new string(ReadChars_TrimNullTerminators(charCount));
		}
		public void ReadStrings_Count_TrimNullTerminators(Span<string> dest, int charCount)
		{
			for (int i = 0; i < dest.Length; i++)
			{
				dest[i] = ReadString_Count_TrimNullTerminators(charCount);
			}
		}
		public string ReadString_NullTerminated()
		{
			var v = new StringBuilder();
			while (true)
			{
				char c = ReadChar();
				if (c == '\0')
				{
					break;
				}
				v.Append(c);
			}
			return v.ToString(); // Returns string.Empty if length is 0
		}
		public void ReadStrings_NullTerminated(Span<string> dest)
		{
			for (int i = 0; i < dest.Length; i++)
			{
				dest[i] = ReadString_NullTerminated();
			}
		}
	}
}
