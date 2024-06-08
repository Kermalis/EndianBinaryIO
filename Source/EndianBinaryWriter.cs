using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Kermalis.EndianBinaryIO;

public partial class EndianBinaryWriter
{
	protected const int BUF_LEN = 64; // Must be a multiple of 64

	private Stream _stream;
	public Stream Stream
	{
		get => _stream;
		[MemberNotNull(nameof(_stream))]
		set
		{
			if (!value.CanWrite)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Stream is not open for writing.");
			}
			_stream = value;
		}
	}
	private Endianness _endianness;
	public Endianness Endianness
	{
		get => _endianness;
		set
		{
			Utils.ThrowIfInvalidEndianness(value);
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
		Stream = stream;
		Endianness = endianness;
		BooleanSize = booleanSize;
		ASCII = ascii;
		_buffer = new byte[BUF_LEN];
	}

	protected delegate void WriteArrayMethod<TSrc>(Span<byte> dest, ReadOnlySpan<TSrc> src, Endianness endianness);
	protected void WriteArray<TSrc>(ReadOnlySpan<TSrc> src, int elementSize, WriteArrayMethod<TSrc> writeArray)
	{
		int numBytes = src.Length * elementSize;
		int start = 0;
		while (numBytes != 0)
		{
			int consumeBytes = Math.Min(numBytes, BUF_LEN);

			Span<byte> buffer = _buffer.AsSpan(0, consumeBytes);
			writeArray(buffer, src.Slice(start, consumeBytes / elementSize), Endianness);
			_stream.Write(buffer);

			numBytes -= consumeBytes;
			start += consumeBytes / elementSize;
		}
	}
	private void WriteBoolArray(ReadOnlySpan<bool> src, int elementSize)
	{
		int numBytes = src.Length * elementSize;
		int start = 0;
		while (numBytes != 0)
		{
			int consumeBytes = Math.Min(numBytes, BUF_LEN);

			Span<byte> buffer = _buffer.AsSpan(0, consumeBytes);
			buffer.WriteBooleans_Unsafe(src.Slice(start, consumeBytes / elementSize), Endianness, elementSize);
			_stream.Write(buffer);

			numBytes -= consumeBytes;
			start += consumeBytes / elementSize;
		}
	}
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
			_stream.Write(buffer);

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
			_stream.Write(buffer);

			numBytes -= consumeBytes;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteSByte(sbyte value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 1);
		buffer[0] = (byte)value;
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteSBytes(ReadOnlySpan<sbyte> values)
	{
		ReadOnlySpan<byte> buffer = values.ReadCast<sbyte, byte>(values.Length);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteByte(byte value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 1);
		buffer[0] = value;
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteBytes(ReadOnlySpan<byte> values)
	{
		_stream.Write(values);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteInt16(short value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 2);
		buffer.WriteInt16_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteInt16s(ReadOnlySpan<short> values)
	{
		WriteArray(values, 2, EndianBinaryPrimitives.WriteInt16s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteUInt16(ushort value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 2);
		buffer.WriteUInt16_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteUInt16s(ReadOnlySpan<ushort> values)
	{
		WriteArray(values, 2, EndianBinaryPrimitives.WriteUInt16s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteInt32(int value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 4);
		buffer.WriteInt32_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteInt32s(ReadOnlySpan<int> values)
	{
		WriteArray(values, 4, EndianBinaryPrimitives.WriteInt32s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteUInt32(uint value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 4);
		buffer.WriteUInt32_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteUInt32s(ReadOnlySpan<uint> values)
	{
		WriteArray(values, 4, EndianBinaryPrimitives.WriteUInt32s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteInt64(long value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 8);
		buffer.WriteInt64_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteInt64s(ReadOnlySpan<long> values)
	{
		WriteArray(values, 8, EndianBinaryPrimitives.WriteInt64s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteUInt64(ulong value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 8);
		buffer.WriteUInt64_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteUInt64s(ReadOnlySpan<ulong> values)
	{
		WriteArray(values, 8, EndianBinaryPrimitives.WriteUInt64s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteInt128(in Int128 value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 16);
		buffer.WriteInt128_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteInt128s(ReadOnlySpan<Int128> values)
	{
		WriteArray(values, 16, EndianBinaryPrimitives.WriteInt128s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteUInt128(in UInt128 value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 16);
		buffer.WriteUInt128_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteUInt128s(ReadOnlySpan<UInt128> values)
	{
		WriteArray(values, 16, EndianBinaryPrimitives.WriteUInt128s_Unsafe);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteHalf(Half value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 2);
		buffer.WriteHalf_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteHalves(ReadOnlySpan<Half> values)
	{
		WriteArray(values, 2, EndianBinaryPrimitives.WriteHalves_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteSingle(float value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 4);
		buffer.WriteSingle_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteSingles(ReadOnlySpan<float> values)
	{
		WriteArray(values, 4, EndianBinaryPrimitives.WriteSingles_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteDouble(double value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 8);
		buffer.WriteDouble_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteDoubles(ReadOnlySpan<double> values)
	{
		WriteArray(values, 8, EndianBinaryPrimitives.WriteDoubles_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteDecimal(in decimal value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 16);
		buffer.WriteDecimal_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteDecimals(ReadOnlySpan<decimal> values)
	{
		WriteArray(values, 16, EndianBinaryPrimitives.WriteDecimals_Unsafe);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteBoolean8(bool value)
	{
		WriteByte((byte)(value ? 1 : 0));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteBoolean8s(ReadOnlySpan<bool> values)
	{
		WriteBoolArray(values, sizeof(byte));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteBoolean16(bool value)
	{
		WriteUInt16((ushort)(value ? 1 : 0));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteBoolean16s(ReadOnlySpan<bool> values)
	{
		WriteBoolArray(values, sizeof(ushort));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteBoolean32(bool value)
	{
		WriteUInt32(value ? 1u : 0);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteBoolean32s(ReadOnlySpan<bool> values)
	{
		WriteBoolArray(values, sizeof(uint));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteBoolean(bool value)
	{
		switch (BooleanSize)
		{
			case BooleanSize.U8: WriteByte((byte)(value ? 1 : 0)); break;
			case BooleanSize.U16: WriteUInt16((ushort)(value ? 1 : 0)); break;
			case BooleanSize.U32: WriteUInt32(value ? 1u : 0); break;
			default: throw new InvalidOperationException();
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteBooleans(ReadOnlySpan<bool> values)
	{
		WriteBoolArray(values, EndianBinaryPrimitives.GetBytesForBooleanSize(BooleanSize));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteEnum<TEnum>(TEnum value)
		where TEnum : unmanaged, Enum
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
	public void WriteEnums<TEnum>(ReadOnlySpan<TEnum> values)
		where TEnum : unmanaged, Enum
	{
		int size = Unsafe.SizeOf<TEnum>();
		if (size == 1)
		{
			WriteBytes(values.ReadCast<TEnum, byte>(values.Length));
		}
		else if (size == 2)
		{
			WriteUInt16s(values.ReadCast<TEnum, ushort>(values.Length * 2));
		}
		else if (size == 4)
		{
			WriteUInt32s(values.ReadCast<TEnum, uint>(values.Length * 4));
		}
		else
		{
			WriteUInt64s(values.ReadCast<TEnum, ulong>(values.Length * 8));
		}
	}
	// #13 - Allow writing the abstract "Enum" type
	// For example, writer.WriteEnum((Enum)Enum.Parse(enumType, value))
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

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteDateTime(DateTime value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 8);
		buffer.WriteDateTime_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteDateTimes(ReadOnlySpan<DateTime> values)
	{
		WriteArray(values, 8, EndianBinaryPrimitives.WriteDateTimes_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteDateOnly(DateOnly value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 4);
		buffer.WriteDateOnly_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteDateOnlys(ReadOnlySpan<DateOnly> values)
	{
		WriteArray(values, 4, EndianBinaryPrimitives.WriteDateOnlys_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteTimeOnly(TimeOnly value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 8);
		buffer.WriteTimeOnly_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteTimeOnlys(ReadOnlySpan<TimeOnly> values)
	{
		WriteArray(values, 8, EndianBinaryPrimitives.WriteTimeOnlys_Unsafe);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteVector2(Vector2 value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 8);
		buffer.WriteVector2_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteVector2s(ReadOnlySpan<Vector2> values)
	{
		WriteArray(values, 8, EndianBinaryPrimitives.WriteVector2s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteVector3(Vector3 value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 12);
		buffer.WriteVector3_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteVector3s(ReadOnlySpan<Vector3> values)
	{
		WriteArray(values, 12, EndianBinaryPrimitives.WriteVector3s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteVector4(in Vector4 value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 16);
		buffer.WriteVector4_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteVector4s(ReadOnlySpan<Vector4> values)
	{
		WriteArray(values, 16, EndianBinaryPrimitives.WriteVector4s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteQuaternion(in Quaternion value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 16);
		buffer.WriteQuaternion_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteQuaternions(ReadOnlySpan<Quaternion> values)
	{
		WriteArray(values, 16, EndianBinaryPrimitives.WriteQuaternions_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteMatrix4x4(in Matrix4x4 value)
	{
		Span<byte> buffer = _buffer.AsSpan(0, 64);
		buffer.WriteMatrix4x4_Unsafe(value, Endianness);
		_stream.Write(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void WriteMatrix4x4s(ReadOnlySpan<Matrix4x4> values)
	{
		WriteArray(values, 64, EndianBinaryPrimitives.WriteMatrix4x4s_Unsafe);
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
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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
			ReadOnlySpan<ushort> buffer = values.ReadCast<char, ushort>(values.Length);
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
