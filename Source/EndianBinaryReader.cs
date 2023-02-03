using System;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Kermalis.EndianBinaryIO;

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
	private void ReadBoolArray(Span<bool> dest, int boolSize)
	{
		int numBytes = dest.Length * boolSize;
		int start = 0;
		while (numBytes != 0)
		{
			int consumeBytes = Math.Min(numBytes, BUF_LEN);

			Span<byte> buffer = _buffer.AsSpan(0, consumeBytes);
			ReadBytes(buffer);
			EndianBinaryPrimitives.ReadBooleans_Unsafe(buffer, dest.Slice(start, consumeBytes / boolSize), Endianness, boolSize);

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
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void PeekBytes(Span<byte> dest)
	{
		long offset = Stream.Position;

		ReadBytes(dest);

		Stream.Position = offset;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public sbyte ReadSByte()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 1);
		ReadBytes(buffer);
		return (sbyte)buffer[0];
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadSBytes(Span<sbyte> dest)
	{
		Span<byte> buffer = dest.WriteCast<sbyte, byte>(dest.Length);
		ReadBytes(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public short ReadInt16()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 2);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadInt16_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadInt16s(Span<short> dest)
	{
		ReadArray(dest, 2, EndianBinaryPrimitives.ReadInt16s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public ushort ReadUInt16()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 2);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadUInt16_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadUInt16s(Span<ushort> dest)
	{
		ReadArray(dest, 2, EndianBinaryPrimitives.ReadUInt16s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public int ReadInt32()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 4);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadInt32_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadInt32s(Span<int> dest)
	{
		ReadArray(dest, 4, EndianBinaryPrimitives.ReadInt32s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public uint ReadUInt32()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 4);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadUInt32_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadUInt32s(Span<uint> dest)
	{
		ReadArray(dest, 4, EndianBinaryPrimitives.ReadUInt32s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public long ReadInt64()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 8);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadInt64_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadInt64s(Span<long> dest)
	{
		ReadArray(dest, 8, EndianBinaryPrimitives.ReadInt64s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public ulong ReadUInt64()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 8);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadUInt64_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadUInt64s(Span<ulong> dest)
	{
		ReadArray(dest, 8, EndianBinaryPrimitives.ReadUInt64s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public Int128 ReadInt128()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 16);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadInt128_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadInt128s(Span<Int128> dest)
	{
		ReadArray(dest, 16, EndianBinaryPrimitives.ReadInt128s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public UInt128 ReadUInt128()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 16);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadUInt128_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadUInt128s(Span<UInt128> dest)
	{
		ReadArray(dest, 16, EndianBinaryPrimitives.ReadUInt128s_Unsafe);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public Half ReadHalf()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 2);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadHalf_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadHalves(Span<Half> dest)
	{
		ReadArray(dest, 2, EndianBinaryPrimitives.ReadHalves_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public float ReadSingle()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 4);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadSingle_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadSingles(Span<float> dest)
	{
		ReadArray(dest, 4, EndianBinaryPrimitives.ReadSingles_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public double ReadDouble()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 8);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadDouble_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadDoubles(Span<double> dest)
	{
		ReadArray(dest, 8, EndianBinaryPrimitives.ReadDoubles_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public decimal ReadDecimal()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 16);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadDecimal_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadDecimals(Span<decimal> dest)
	{
		ReadArray(dest, 16, EndianBinaryPrimitives.ReadDecimals_Unsafe);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public bool ReadBoolean8()
	{
		return ReadByte() != 0;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadBoolean8s(Span<bool> dest)
	{
		ReadBoolArray(dest, sizeof(byte));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public bool ReadBoolean16()
	{
		return ReadUInt16() != 0;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadBoolean16s(Span<bool> dest)
	{
		ReadBoolArray(dest, sizeof(ushort));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public bool ReadBoolean32()
	{
		return ReadUInt32() != 0;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadBoolean32s(Span<bool> dest)
	{
		ReadBoolArray(dest, sizeof(uint));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public bool ReadBoolean()
	{
		switch (BooleanSize)
		{
			case BooleanSize.U8: return ReadByte() != 0;
			case BooleanSize.U16: return ReadUInt16() != 0;
			case BooleanSize.U32: return ReadUInt32() != 0;
			default: throw new InvalidOperationException();
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadBooleans(Span<bool> dest)
	{
		ReadBoolArray(dest, EndianBinaryPrimitives.GetBytesForBooleanSize(BooleanSize));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public TEnum ReadEnum<TEnum>()
		where TEnum : unmanaged, Enum
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
	public void ReadEnums<TEnum>(Span<TEnum> dest)
		where TEnum : unmanaged, Enum
	{
		int size = Unsafe.SizeOf<TEnum>();
		if (size == 1)
		{
			ReadBytes(dest.WriteCast<TEnum, byte>(dest.Length));
		}
		else if (size == 2)
		{
			ReadUInt16s(dest.WriteCast<TEnum, ushort>(dest.Length * 2));
		}
		else if (size == 4)
		{
			ReadUInt32s(dest.WriteCast<TEnum, uint>(dest.Length * 4));
		}
		else
		{
			ReadUInt64s(dest.WriteCast<TEnum, ulong>(dest.Length * 8));
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

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public DateTime ReadDateTime()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 8);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadDateTime_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadDateTimes(Span<DateTime> dest)
	{
		ReadArray(dest, 8, EndianBinaryPrimitives.ReadDateTimes_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public DateOnly ReadDateOnly()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 4);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadDateOnly_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadDateOnlys(Span<DateOnly> dest)
	{
		ReadArray(dest, 4, EndianBinaryPrimitives.ReadDateOnlys_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public TimeOnly ReadTimeOnly()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 8);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadTimeOnly_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadTimeOnlys(Span<TimeOnly> dest)
	{
		ReadArray(dest, 8, EndianBinaryPrimitives.ReadTimeOnlys_Unsafe);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public Vector2 ReadVector2()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 8);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadVector2_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadVector2s(Span<Vector2> dest)
	{
		ReadArray(dest, 8, EndianBinaryPrimitives.ReadVector2s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public Vector3 ReadVector3()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 12);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadVector3_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadVector3s(Span<Vector3> dest)
	{
		ReadArray(dest, 12, EndianBinaryPrimitives.ReadVector3s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public Vector4 ReadVector4()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 16);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadVector4_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadVector4s(Span<Vector4> dest)
	{
		ReadArray(dest, 16, EndianBinaryPrimitives.ReadVector4s_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public Quaternion ReadQuaternion()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 16);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadQuaternion_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadQuaternions(Span<Quaternion> dest)
	{
		ReadArray(dest, 16, EndianBinaryPrimitives.ReadQuaternions_Unsafe);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public Matrix4x4 ReadMatrix4x4()
	{
		Span<byte> buffer = _buffer.AsSpan(0, 64);
		ReadBytes(buffer);
		return EndianBinaryPrimitives.ReadMatrix4x4_Unsafe(buffer, Endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadMatrix4x4s(Span<Matrix4x4> dest)
	{
		ReadArray(dest, 64, EndianBinaryPrimitives.ReadMatrix4x4s_Unsafe);
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
			// Read ASCII chars into 2nd half of dest, then populate first half with those buffered chars
			Span<byte> buffer = dest.WriteCast<char, byte>(dest.Length * 2).Slice(dest.Length);
			ReadBytes(buffer);
			for (int i = 0; i < dest.Length; i++)
			{
				dest[i] = (char)buffer[i];
			}
		}
		else
		{
			Span<ushort> buffer = dest.WriteCast<char, ushort>(dest.Length);
			ReadUInt16s(buffer);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public char[] ReadChars_TrimNullTerminators(int charCount)
	{
		char[] chars = new char[charCount];
		ReadChars(chars);
		EndianBinaryPrimitives.TrimNullTerminators(ref chars);
		return chars;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public string ReadString_Count(int charCount)
	{
		void Create(Span<char> dest, byte _)
		{
			ReadChars(dest);
		}
		return string.Create(charCount, byte.MinValue, Create);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadStrings_Count(Span<string> dest, int charCount)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadString_Count(charCount);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public string ReadString_Count_TrimNullTerminators(int charCount)
	{
		char[] chars = new char[charCount];
		ReadChars(chars);

		// Trim '\0's
		int i = Array.IndexOf(chars, '\0');
		if (i != -1)
		{
			return new string(chars, 0, i);
		}
		return new string(chars);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void ReadStrings_NullTerminated(Span<string> dest)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadString_NullTerminated();
		}
	}
}
