using System;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kermalis.EndianBinaryIO
{
	public partial class EndianBinaryWriter
	{
		protected const int BUF_LEN = 64; // Must be a power of 64

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

		public EndianBinaryWriter(Stream stream, Endianness endianness = Endianness.LittleEndian, BooleanSize booleanSize = BooleanSize.U8, bool ascii = false)
		{
			if (!stream.CanWrite)
			{
				throw new ArgumentOutOfRangeException(nameof(stream), "Stream is not open for writing.");
			}

			Stream = stream;
			Endianness = endianness;
			BooleanSize = booleanSize;
			ASCII = ascii;
			_buffer = new byte[BUF_LEN];
		}

		protected delegate void WriteArrayMethod<TSrc>(Span<byte> dest, ReadOnlySpan<TSrc> src, Endianness endianness);
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		protected void WriteArray<TSrc>(ReadOnlySpan<TSrc> src, int elementSize, WriteArrayMethod<TSrc> writeArray)
		{
			int numBytes = src.Length * elementSize;
			int start = 0;
			while (numBytes != 0)
			{
				int consumeBytes = Math.Min(numBytes, BUF_LEN);

				Span<byte> buffer = _buffer.AsSpan(0, consumeBytes);
				writeArray(buffer, src.Slice(start, consumeBytes / elementSize), Endianness);
				Stream.Write(buffer);

				numBytes -= consumeBytes;
				start += consumeBytes / elementSize;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private void WriteBoolArray(ReadOnlySpan<bool> src, int elementSize)
		{
			int numBytes = src.Length * elementSize;
			int start = 0;
			while (numBytes != 0)
			{
				int consumeBytes = Math.Min(numBytes, BUF_LEN);

				Span<byte> buffer = _buffer.AsSpan(0, consumeBytes);
				EndianBinaryPrimitives.WriteBooleans(buffer, src.Slice(start, consumeBytes / elementSize), Endianness, elementSize);
				Stream.Write(buffer);

				numBytes -= consumeBytes;
				start += consumeBytes / elementSize;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private void WriteASCIIArray(ReadOnlySpan<char> src)
		{
			int numBytes = src.Length;
			int start = 0;
			while (numBytes != 0)
			{
				int consumeBytes = Math.Min(numBytes, BUF_LEN);

				Span<byte> buffer = _buffer.AsSpan(0, consumeBytes);
				for (int i = 0; i < consumeBytes; i++)
				{
					buffer[i] = (byte)src[i + start];
				}
				Stream.Write(buffer);

				numBytes -= consumeBytes;
				start += consumeBytes;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void WriteZeroes(int numBytes)
		{
			bool cleared = false;
			while (numBytes != 0)
			{
				int consumeBytes = Math.Min(numBytes, BUF_LEN);

				Span<byte> buffer = _buffer.AsSpan(0, consumeBytes);
				if (!cleared)
				{
					buffer.Clear();
					cleared = true;
				}
				Stream.Write(buffer);

				numBytes -= consumeBytes;
			}
		}

		public void WriteSByte(sbyte value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 1);
			buffer[0] = (byte)value;
			Stream.Write(buffer);
		}
		public void WriteSBytes(ReadOnlySpan<sbyte> values)
		{
			ReadOnlySpan<byte> buffer = MemoryMarshal.Cast<sbyte, byte>(values);
			Stream.Write(buffer);
		}
		public void WriteByte(byte value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 1);
			buffer[0] = value;
			Stream.Write(buffer);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void WriteBytes(ReadOnlySpan<byte> values)
		{
			Stream.Write(values);
		}
		public void WriteInt16(short value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 2);
			EndianBinaryPrimitives.WriteInt16(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteInt16s(ReadOnlySpan<short> values)
		{
			WriteArray(values, 2, EndianBinaryPrimitives.WriteInt16s);
		}
		public void WriteUInt16(ushort value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 2);
			EndianBinaryPrimitives.WriteUInt16(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteUInt16s(ReadOnlySpan<ushort> values)
		{
			WriteArray(values, 2, EndianBinaryPrimitives.WriteUInt16s);
		}
		public void WriteInt32(int value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 4);
			EndianBinaryPrimitives.WriteInt32(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteInt32s(ReadOnlySpan<int> values)
		{
			WriteArray(values, 4, EndianBinaryPrimitives.WriteInt32s);
		}
		public void WriteUInt32(uint value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 4);
			EndianBinaryPrimitives.WriteUInt32(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteUInt32s(ReadOnlySpan<uint> values)
		{
			WriteArray(values, 4, EndianBinaryPrimitives.WriteUInt32s);
		}
		public void WriteInt64(long value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 8);
			EndianBinaryPrimitives.WriteInt64(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteInt64s(ReadOnlySpan<long> values)
		{
			WriteArray(values, 8, EndianBinaryPrimitives.WriteInt64s);
		}
		public void WriteUInt64(ulong value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 8);
			EndianBinaryPrimitives.WriteUInt64(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteUInt64s(ReadOnlySpan<ulong> values)
		{
			WriteArray(values, 8, EndianBinaryPrimitives.WriteUInt64s);
		}

		public void WriteHalf(Half value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 2);
			EndianBinaryPrimitives.WriteHalf(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteHalves(ReadOnlySpan<Half> values)
		{
			WriteArray(values, 2, EndianBinaryPrimitives.WriteHalves);
		}
		public void WriteSingle(float value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 4);
			EndianBinaryPrimitives.WriteSingle(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteSingles(ReadOnlySpan<float> values)
		{
			WriteArray(values, 4, EndianBinaryPrimitives.WriteSingles);
		}
		public void WriteDouble(double value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 8);
			EndianBinaryPrimitives.WriteDouble(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteDoubles(ReadOnlySpan<double> values)
		{
			WriteArray(values, 8, EndianBinaryPrimitives.WriteDoubles);
		}
		public void WriteDecimal(in decimal value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 16);
			EndianBinaryPrimitives.WriteDecimal(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteDecimals(ReadOnlySpan<decimal> values)
		{
			WriteArray(values, 16, EndianBinaryPrimitives.WriteDecimals);
		}

		public void WriteBoolean(bool value)
		{
			int elementSize = EndianBinaryPrimitives.GetBytesForBooleanSize(BooleanSize);
			Span<byte> buffer = _buffer.AsSpan(0, elementSize);
			EndianBinaryPrimitives.WriteBoolean(buffer, value, Endianness, elementSize);
			Stream.Write(buffer);
		}
		public void WriteBooleans(ReadOnlySpan<bool> values)
		{
			int elementSize = EndianBinaryPrimitives.GetBytesForBooleanSize(BooleanSize);
			WriteBoolArray(values, elementSize);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void WriteEnum<TEnum>(TEnum value) where TEnum : unmanaged, Enum
		{
			int size = Unsafe.SizeOf<TEnum>();
			if (size == 1)
			{
				WriteByte(Unsafe.As<TEnum, byte>(ref value));
			}
			else if (size == 2)
			{
				WriteUInt16(Unsafe.As<TEnum, ushort>(ref value));
			}
			else if (size == 4)
			{
				WriteUInt32(Unsafe.As<TEnum, uint>(ref value));
			}
			else
			{
				WriteUInt64(Unsafe.As<TEnum, ulong>(ref value));
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void WriteEnums<TEnum>(ReadOnlySpan<TEnum> values) where TEnum : unmanaged, Enum
		{
			int size = Unsafe.SizeOf<TEnum>();
			if (size == 1)
			{
				WriteBytes(MemoryMarshal.Cast<TEnum, byte>(values));
			}
			else if (size == 2)
			{
				WriteUInt16s(MemoryMarshal.Cast<TEnum, ushort>(values));
			}
			else if (size == 4)
			{
				WriteUInt32s(MemoryMarshal.Cast<TEnum, uint>(values));
			}
			else
			{
				WriteUInt64s(MemoryMarshal.Cast<TEnum, ulong>(values));
			}
		}
		// #13 - Allow writing the abstract "Enum" type
		// For example, writer.Write((Enum)Enum.Parse(enumType, value))
		// Don't allow writing Enum[] though, since there is no way to read that
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void WriteEnum(Enum value)
		{
			Type underlyingType = Enum.GetUnderlyingType(value.GetType());
			ref byte data = ref Utils.GetRawData(value); // Use memory tricks to skip object header of generic Enum
			switch (Type.GetTypeCode(underlyingType))
			{
				case TypeCode.SByte:
				case TypeCode.Byte: WriteByte(data); break;
				case TypeCode.Int16:
				case TypeCode.UInt16: WriteUInt16(Unsafe.As<byte, ushort>(ref data)); break;
				case TypeCode.Int32:
				case TypeCode.UInt32: WriteUInt32(Unsafe.As<byte, uint>(ref data)); break;
				case TypeCode.Int64:
				case TypeCode.UInt64: WriteUInt64(Unsafe.As<byte, ulong>(ref data)); break;
			}
		}

		public void WriteDateTime(DateTime value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 8);
			EndianBinaryPrimitives.WriteDateTime(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteDateTimes(ReadOnlySpan<DateTime> values)
		{
			WriteArray(values, 8, EndianBinaryPrimitives.WriteDateTimes);
		}
		public void WriteDateOnly(DateOnly value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 4);
			EndianBinaryPrimitives.WriteDateOnly(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteDateOnlys(ReadOnlySpan<DateOnly> values)
		{
			WriteArray(values, 4, EndianBinaryPrimitives.WriteDateOnlys);
		}
		public void WriteTimeOnly(TimeOnly value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 8);
			EndianBinaryPrimitives.WriteTimeOnly(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteTimeOnlys(ReadOnlySpan<TimeOnly> values)
		{
			WriteArray(values, 8, EndianBinaryPrimitives.WriteTimeOnlys);
		}

		public void WriteVector2(Vector2 value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 8);
			EndianBinaryPrimitives.WriteVector2(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteVector2s(ReadOnlySpan<Vector2> values)
		{
			WriteArray(values, 8, EndianBinaryPrimitives.WriteVector2s);
		}
		public void WriteVector3(Vector3 value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 12);
			EndianBinaryPrimitives.WriteVector3(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteVector3s(ReadOnlySpan<Vector3> values)
		{
			WriteArray(values, 12, EndianBinaryPrimitives.WriteVector3s);
		}
		public void WriteVector4(in Vector4 value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 16);
			EndianBinaryPrimitives.WriteVector4(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteVector4s(ReadOnlySpan<Vector4> values)
		{
			WriteArray(values, 16, EndianBinaryPrimitives.WriteVector4s);
		}
		public void WriteQuaternion(in Quaternion value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 16);
			EndianBinaryPrimitives.WriteQuaternion(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteQuaternions(ReadOnlySpan<Quaternion> values)
		{
			WriteArray(values, 16, EndianBinaryPrimitives.WriteQuaternions);
		}
		public void WriteMatrix4x4(in Matrix4x4 value)
		{
			Span<byte> buffer = _buffer.AsSpan(0, 64);
			EndianBinaryPrimitives.WriteMatrix4x4(buffer, value, Endianness);
			Stream.Write(buffer);
		}
		public void WriteMatrix4x4s(ReadOnlySpan<Matrix4x4> values)
		{
			WriteArray(values, 64, EndianBinaryPrimitives.WriteMatrix4x4s);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void WriteChar(char v)
		{
			if (ASCII)
			{
				WriteByte((byte)v);
			}
			else
			{
				WriteUInt16(v);
			}
		}
		public void WriteChars(ReadOnlySpan<char> values)
		{
			if (values.Length == 0)
			{
				return;
			}
			if (ASCII)
			{
				WriteASCIIArray(values);
			}
			else
			{
				ReadOnlySpan<ushort> buffer = MemoryMarshal.Cast<char, ushort>(values);
				WriteUInt16s(buffer);
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void WriteChars_Count(ReadOnlySpan<char> values, int charCount)
		{
			if (charCount == 0)
			{
				return;
			}
			if (values.Length >= charCount)
			{
				WriteChars(values.Slice(0, charCount));
			}
			else // Less
			{
				WriteChars(values);

				// Append '\0'
				int emptyBytes = charCount - values.Length;
				if (!ASCII)
				{
					emptyBytes *= 2;
				}
				WriteZeroes(emptyBytes);
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void WriteChars_NullTerminated(ReadOnlySpan<char> values)
		{
			WriteChars(values);
			WriteChar('\0');
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void WriteStrings(ReadOnlySpan<string> values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				WriteChars(values[i]);
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void WriteStrings_Count(ReadOnlySpan<string> values, int charCount)
		{
			for (int i = 0; i < values.Length; i++)
			{
				WriteChars_Count(values[i], charCount);
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public void WriteStrings_NullTerminated(ReadOnlySpan<string> values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				WriteChars_NullTerminated(values[i]);
			}
		}
	}
}
