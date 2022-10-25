using System;
using System.Buffers.Binary;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kermalis.EndianBinaryIO;

public static class EndianBinaryPrimitives
{
	public static readonly Endianness SystemEndianness = BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;

	public static int GetBytesForBooleanSize(BooleanSize boolSize)
	{
		switch (boolSize)
		{
			case BooleanSize.U8: return 1;
			case BooleanSize.U16: return 2;
			case BooleanSize.U32: return 4;
			default: throw new ArgumentOutOfRangeException(nameof(boolSize), boolSize, null);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void TrimNullTerminators(ref char[] chars)
	{
		int i = Array.IndexOf(chars, '\0');
		if (i != -1)
		{
			Array.Resize(ref chars, i);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void TrimNullTerminators(ref Span<char> chars)
	{
		int i = chars.IndexOf('\0');
		if (i != -1)
		{
			chars = chars.Slice(0, i);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void TrimNullTerminators(ref ReadOnlySpan<char> chars)
	{
		int i = chars.IndexOf('\0');
		if (i != -1)
		{
			chars = chars.Slice(0, i);
		}
	}

	#region Read

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadSBytes(ReadOnlySpan<byte> src, Span<sbyte> dest)
	{
		src.CopyTo(MemoryMarshal.Cast<sbyte, byte>(dest));
	}

	public static short ReadInt16(ReadOnlySpan<byte> src, Endianness endianness)
	{
		return endianness == Endianness.LittleEndian
			? BinaryPrimitives.ReadInt16LittleEndian(src)
			: BinaryPrimitives.ReadInt16BigEndian(src);
	}
	public static void ReadInt16s(ReadOnlySpan<byte> src, Span<short> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadInt16(src.Slice(i * 2, 2), endianness);
		}
	}

	public static ushort ReadUInt16(ReadOnlySpan<byte> src, Endianness endianness)
	{
		return endianness == Endianness.LittleEndian
			? BinaryPrimitives.ReadUInt16LittleEndian(src)
			: BinaryPrimitives.ReadUInt16BigEndian(src);
	}
	public static void ReadUInt16s(ReadOnlySpan<byte> src, Span<ushort> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadUInt16(src.Slice(i * 2, 2), endianness);
		}
	}

	public static int ReadInt32(ReadOnlySpan<byte> src, Endianness endianness)
	{
		return endianness == Endianness.LittleEndian
			? BinaryPrimitives.ReadInt32LittleEndian(src)
			: BinaryPrimitives.ReadInt32BigEndian(src);
	}
	public static void ReadInt32s(ReadOnlySpan<byte> src, Span<int> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadInt32(src.Slice(i * 4, 4), endianness);
		}
	}

	public static uint ReadUInt32(ReadOnlySpan<byte> src, Endianness endianness)
	{
		return endianness == Endianness.LittleEndian
			? BinaryPrimitives.ReadUInt32LittleEndian(src)
			: BinaryPrimitives.ReadUInt32BigEndian(src);
	}
	public static void ReadUInt32s(ReadOnlySpan<byte> src, Span<uint> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadUInt32(src.Slice(i * 4, 4), endianness);
		}
	}

	public static long ReadInt64(ReadOnlySpan<byte> src, Endianness endianness)
	{
		return endianness == Endianness.LittleEndian
			? BinaryPrimitives.ReadInt64LittleEndian(src)
			: BinaryPrimitives.ReadInt64BigEndian(src);
	}
	public static void ReadInt64s(ReadOnlySpan<byte> src, Span<long> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadInt64(src.Slice(i * 8, 8), endianness);
		}
	}

	public static ulong ReadUInt64(ReadOnlySpan<byte> src, Endianness endianness)
	{
		return endianness == Endianness.LittleEndian
			? BinaryPrimitives.ReadUInt64LittleEndian(src)
			: BinaryPrimitives.ReadUInt64BigEndian(src);
	}
	public static void ReadUInt64s(ReadOnlySpan<byte> src, Span<ulong> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadUInt64(src.Slice(i * 8, 8), endianness);
		}
	}

	public static Half ReadHalf(ReadOnlySpan<byte> src, Endianness endianness)
	{
		return endianness == Endianness.LittleEndian
			? BinaryPrimitives.ReadHalfLittleEndian(src)
			: BinaryPrimitives.ReadHalfBigEndian(src);
	}
	public static void ReadHalves(ReadOnlySpan<byte> src, Span<Half> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadHalf(src.Slice(i * 2, 2), endianness);
		}
	}

	public static float ReadSingle(ReadOnlySpan<byte> src, Endianness endianness)
	{
		return endianness == Endianness.LittleEndian
			? BinaryPrimitives.ReadSingleLittleEndian(src)
			: BinaryPrimitives.ReadSingleBigEndian(src);
	}
	public static void ReadSingles(ReadOnlySpan<byte> src, Span<float> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadSingle(src.Slice(i * 4, 4), endianness);
		}
	}

	public static double ReadDouble(ReadOnlySpan<byte> src, Endianness endianness)
	{
		return endianness == Endianness.LittleEndian
			? BinaryPrimitives.ReadDoubleLittleEndian(src)
			: BinaryPrimitives.ReadDoubleBigEndian(src);
	}
	public static void ReadDoubles(ReadOnlySpan<byte> src, Span<double> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadDouble(src.Slice(i * 8, 8), endianness);
		}
	}

	public static decimal ReadDecimal(ReadOnlySpan<byte> src, Endianness endianness)
	{
		Span<int> buffer = stackalloc int[4];
		ReadInt32s(src, buffer, endianness);
		return new decimal(buffer);
	}
	public static void ReadDecimals(ReadOnlySpan<byte> src, Span<decimal> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadDecimal(src.Slice(i * 16, 16), endianness);
		}
	}

	public static bool ReadBoolean(ReadOnlySpan<byte> src, Endianness endianness, BooleanSize boolSize)
	{
		return ReadBoolean(src, endianness, GetBytesForBooleanSize(boolSize));
	}
	public static void ReadBooleans(ReadOnlySpan<byte> src, Span<bool> dest, Endianness endianness, BooleanSize boolSize)
	{
		ReadBooleans(src, dest, endianness, GetBytesForBooleanSize(boolSize));
	}
	public static bool ReadBoolean(ReadOnlySpan<byte> src, Endianness endianness, int boolSize)
	{
		switch (boolSize)
		{
			case 1:
			{
				return src[0] != 0;
			}
			case 2:
			{
				return ReadUInt16(src, endianness) != 0;
			}
			case 4:
			{
				return ReadUInt32(src, endianness) != 0;
			}
			default: throw new ArgumentOutOfRangeException(nameof(boolSize), boolSize, null);
		}
	}
	public static void ReadBooleans(ReadOnlySpan<byte> src, Span<bool> dest, Endianness endianness, int boolSize)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadBoolean(src.Slice(i * boolSize, boolSize), endianness, boolSize);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static TEnum ReadEnum<TEnum>(ReadOnlySpan<byte> src, Endianness endianness) where TEnum : unmanaged, Enum
	{
		int size = Unsafe.SizeOf<TEnum>();
		if (size == 1)
		{
			byte b = src[0];
			return Unsafe.As<byte, TEnum>(ref b);
		}
		if (size == 2)
		{
			ushort s = ReadUInt16(src, endianness);
			return Unsafe.As<ushort, TEnum>(ref s);
		}
		if (size == 4)
		{
			uint i = ReadUInt32(src, endianness);
			return Unsafe.As<uint, TEnum>(ref i);
		}
		ulong l = ReadUInt64(src, endianness);
		return Unsafe.As<ulong, TEnum>(ref l);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadEnums<TEnum>(ReadOnlySpan<byte> src, Span<TEnum> dest, Endianness endianness) where TEnum : unmanaged, Enum
	{
		int size = Unsafe.SizeOf<TEnum>();
		if (size == 1)
		{
			src.CopyTo(MemoryMarshal.Cast<TEnum, byte>(dest));
		}
		else if (size == 2)
		{
			ReadUInt16s(src, MemoryMarshal.Cast<TEnum, ushort>(dest), endianness);
		}
		else if (size == 4)
		{
			ReadUInt32s(src, MemoryMarshal.Cast<TEnum, uint>(dest), endianness);
		}
		else
		{
			ReadUInt64s(src, MemoryMarshal.Cast<TEnum, ulong>(dest), endianness);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static object ReadEnum(ReadOnlySpan<byte> src, Endianness endianness, Type enumType)
	{
		// Type.IsEnum is also false for base Enum type, so don't worry about it
		Type underlyingType = Enum.GetUnderlyingType(enumType);
		switch (Type.GetTypeCode(underlyingType))
		{
			case TypeCode.SByte: return Enum.ToObject(enumType, (sbyte)src[0]);
			case TypeCode.Byte: return Enum.ToObject(enumType, src[0]);
			case TypeCode.Int16: return Enum.ToObject(enumType, ReadInt16(src, endianness));
			case TypeCode.UInt16: return Enum.ToObject(enumType, ReadUInt16(src, endianness));
			case TypeCode.Int32: return Enum.ToObject(enumType, ReadInt32(src, endianness));
			case TypeCode.UInt32: return Enum.ToObject(enumType, ReadUInt32(src, endianness));
			case TypeCode.Int64: return Enum.ToObject(enumType, ReadInt64(src, endianness));
			case TypeCode.UInt64: return Enum.ToObject(enumType, ReadUInt64(src, endianness));
		}
		throw new ArgumentOutOfRangeException(nameof(enumType), enumType, null);
	}

	public static DateTime ReadDateTime(ReadOnlySpan<byte> src, Endianness endianness)
	{
		return DateTime.FromBinary(ReadInt64(src, endianness));
	}
	public static void ReadDateTimes(ReadOnlySpan<byte> src, Span<DateTime> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadDateTime(src.Slice(i * 8, 8), endianness);
		}
	}

	public static DateOnly ReadDateOnly(ReadOnlySpan<byte> src, Endianness endianness)
	{
		return DateOnly.FromDayNumber(ReadInt32(src, endianness));
	}
	public static void ReadDateOnlys(ReadOnlySpan<byte> src, Span<DateOnly> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadDateOnly(src.Slice(i * 4, 4), endianness);
		}
	}

	public static TimeOnly ReadTimeOnly(ReadOnlySpan<byte> src, Endianness endianness)
	{
		return new TimeOnly(ReadInt64(src, endianness));
	}
	public static void ReadTimeOnlys(ReadOnlySpan<byte> src, Span<TimeOnly> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadTimeOnly(src.Slice(i * 8, 8), endianness);
		}
	}

	public static Vector2 ReadVector2(ReadOnlySpan<byte> src, Endianness endianness)
	{
		Vector2 v;
		v.X = ReadSingle(src.Slice(0, 4), endianness);
		v.Y = ReadSingle(src.Slice(4, 4), endianness);
		return v;
	}
	public static void ReadVector2s(ReadOnlySpan<byte> src, Span<Vector2> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadVector2(src.Slice(i * 8, 8), endianness);
		}
	}

	public static Vector3 ReadVector3(ReadOnlySpan<byte> src, Endianness endianness)
	{
		Vector3 v;
		v.X = ReadSingle(src.Slice(0, 4), endianness);
		v.Y = ReadSingle(src.Slice(4, 4), endianness);
		v.Z = ReadSingle(src.Slice(8, 4), endianness);
		return v;
	}
	public static void ReadVector3s(ReadOnlySpan<byte> src, Span<Vector3> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadVector3(src.Slice(i * 12, 12), endianness);
		}
	}

	public static Vector4 ReadVector4(ReadOnlySpan<byte> src, Endianness endianness)
	{
		Vector4 v;
		v.W = ReadSingle(src.Slice(0, 4), endianness);
		v.X = ReadSingle(src.Slice(4, 4), endianness);
		v.Y = ReadSingle(src.Slice(8, 4), endianness);
		v.Z = ReadSingle(src.Slice(12, 4), endianness);
		return v;
	}
	public static void ReadVector4s(ReadOnlySpan<byte> src, Span<Vector4> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadVector4(src.Slice(i * 16, 16), endianness);
		}
	}

	public static Quaternion ReadQuaternion(ReadOnlySpan<byte> src, Endianness endianness)
	{
		Quaternion v;
		v.W = ReadSingle(src.Slice(0, 4), endianness);
		v.X = ReadSingle(src.Slice(4, 4), endianness);
		v.Y = ReadSingle(src.Slice(8, 4), endianness);
		v.Z = ReadSingle(src.Slice(12, 4), endianness);
		return v;
	}
	public static void ReadQuaternions(ReadOnlySpan<byte> src, Span<Quaternion> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadQuaternion(src.Slice(i * 16, 16), endianness);
		}
	}

	public static Matrix4x4 ReadMatrix4x4(ReadOnlySpan<byte> src, Endianness endianness)
	{
		Matrix4x4 v;
		v.M11 = ReadSingle(src.Slice(0, 4), endianness);
		v.M12 = ReadSingle(src.Slice(4, 4), endianness);
		v.M13 = ReadSingle(src.Slice(8, 4), endianness);
		v.M14 = ReadSingle(src.Slice(12, 4), endianness);
		v.M21 = ReadSingle(src.Slice(16, 4), endianness);
		v.M22 = ReadSingle(src.Slice(20, 4), endianness);
		v.M23 = ReadSingle(src.Slice(24, 4), endianness);
		v.M24 = ReadSingle(src.Slice(28, 4), endianness);
		v.M31 = ReadSingle(src.Slice(32, 4), endianness);
		v.M32 = ReadSingle(src.Slice(36, 4), endianness);
		v.M33 = ReadSingle(src.Slice(40, 4), endianness);
		v.M34 = ReadSingle(src.Slice(44, 4), endianness);
		v.M41 = ReadSingle(src.Slice(48, 4), endianness);
		v.M42 = ReadSingle(src.Slice(52, 4), endianness);
		v.M43 = ReadSingle(src.Slice(56, 4), endianness);
		v.M44 = ReadSingle(src.Slice(60, 4), endianness);
		return v;
	}
	public static void ReadMatrix4x4s(ReadOnlySpan<byte> src, Span<Matrix4x4> dest, Endianness endianness)
	{
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = ReadMatrix4x4(src.Slice(i * 64, 64), endianness);
		}
	}

	#endregion

	#region Write

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteSBytes(Span<byte> dest, ReadOnlySpan<sbyte> src)
	{
		MemoryMarshal.Cast<sbyte, byte>(src).CopyTo(dest);
	}

	public static void WriteInt16(Span<byte> dest, short value, Endianness endianness)
	{
		if (endianness == Endianness.LittleEndian)
		{
			BinaryPrimitives.WriteInt16LittleEndian(dest, value);
		}
		else
		{
			BinaryPrimitives.WriteInt16BigEndian(dest, value);
		}
	}
	public static void WriteInt16s(Span<byte> dest, ReadOnlySpan<short> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteInt16(dest.Slice(i * 2, 2), src[i], endianness);
		}
	}

	public static void WriteUInt16(Span<byte> dest, ushort value, Endianness endianness)
	{
		if (endianness == Endianness.LittleEndian)
		{
			BinaryPrimitives.WriteUInt16LittleEndian(dest, value);
		}
		else
		{
			BinaryPrimitives.WriteUInt16BigEndian(dest, value);
		}
	}
	public static void WriteUInt16s(Span<byte> dest, ReadOnlySpan<ushort> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteUInt16(dest.Slice(i * 2, 2), src[i], endianness);
		}
	}

	public static void WriteInt32(Span<byte> dest, int value, Endianness endianness)
	{
		if (endianness == Endianness.LittleEndian)
		{
			BinaryPrimitives.WriteInt32LittleEndian(dest, value);
		}
		else
		{
			BinaryPrimitives.WriteInt32BigEndian(dest, value);
		}
	}
	public static void WriteInt32s(Span<byte> dest, ReadOnlySpan<int> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteInt32(dest.Slice(i * 4, 4), src[i], endianness);
		}
	}

	public static void WriteUInt32(Span<byte> dest, uint value, Endianness endianness)
	{
		if (endianness == Endianness.LittleEndian)
		{
			BinaryPrimitives.WriteUInt32LittleEndian(dest, value);
		}
		else
		{
			BinaryPrimitives.WriteUInt32BigEndian(dest, value);
		}
	}
	public static void WriteUInt32s(Span<byte> dest, ReadOnlySpan<uint> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteUInt32(dest.Slice(i * 4, 4), src[i], endianness);
		}
	}

	public static void WriteInt64(Span<byte> dest, long value, Endianness endianness)
	{
		if (endianness == Endianness.LittleEndian)
		{
			BinaryPrimitives.WriteInt64LittleEndian(dest, value);
		}
		else
		{
			BinaryPrimitives.WriteInt64BigEndian(dest, value);
		}
	}
	public static void WriteInt64s(Span<byte> dest, ReadOnlySpan<long> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteInt64(dest.Slice(i * 8, 8), src[i], endianness);
		}
	}

	public static void WriteUInt64(Span<byte> dest, ulong value, Endianness endianness)
	{
		if (endianness == Endianness.LittleEndian)
		{
			BinaryPrimitives.WriteUInt64LittleEndian(dest, value);
		}
		else
		{
			BinaryPrimitives.WriteUInt64BigEndian(dest, value);
		}
	}
	public static void WriteUInt64s(Span<byte> dest, ReadOnlySpan<ulong> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteUInt64(dest.Slice(i * 8, 8), src[i], endianness);
		}
	}

	public static void WriteHalf(Span<byte> dest, Half value, Endianness endianness)
	{
		if (endianness == Endianness.LittleEndian)
		{
			BinaryPrimitives.WriteHalfLittleEndian(dest, value);
		}
		else
		{
			BinaryPrimitives.WriteHalfBigEndian(dest, value);
		}
	}
	public static void WriteHalves(Span<byte> dest, ReadOnlySpan<Half> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteHalf(dest.Slice(i * 2, 2), src[i], endianness);
		}
	}

	public static void WriteSingle(Span<byte> dest, float value, Endianness endianness)
	{
		if (endianness == Endianness.LittleEndian)
		{
			BinaryPrimitives.WriteSingleLittleEndian(dest, value);
		}
		else
		{
			BinaryPrimitives.WriteSingleBigEndian(dest, value);
		}
	}
	public static void WriteSingles(Span<byte> dest, ReadOnlySpan<float> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteSingle(dest.Slice(i * 4, 4), src[i], endianness);
		}
	}

	public static void WriteDouble(Span<byte> dest, double value, Endianness endianness)
	{
		if (endianness == Endianness.LittleEndian)
		{
			BinaryPrimitives.WriteDoubleLittleEndian(dest, value);
		}
		else
		{
			BinaryPrimitives.WriteDoubleBigEndian(dest, value);
		}
	}
	public static void WriteDoubles(Span<byte> dest, ReadOnlySpan<double> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteDouble(dest.Slice(i * 8, 8), src[i], endianness);
		}
	}

	public static void WriteDecimal(Span<byte> dest, in decimal value, Endianness endianness)
	{
		Span<int> buffer = stackalloc int[4];
		decimal.GetBits(value, buffer);
		WriteInt32s(dest, buffer, endianness);
	}
	public static void WriteDecimals(Span<byte> dest, ReadOnlySpan<decimal> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteDecimal(dest.Slice(i * 16, 16), src[i], endianness);
		}
	}

	public static void WriteBoolean(Span<byte> dest, bool value, Endianness endianness, BooleanSize boolSize)
	{
		WriteBoolean(dest, value, endianness, GetBytesForBooleanSize(boolSize));
	}
	public static void WriteBooleans(Span<byte> dest, ReadOnlySpan<bool> src, Endianness endianness, BooleanSize boolSize)
	{
		WriteBooleans(dest, src, endianness, GetBytesForBooleanSize(boolSize));
	}
	public static void WriteBoolean(Span<byte> dest, bool value, Endianness endianness, int boolSize)
	{
		switch (boolSize)
		{
			case 1:
			{
				dest[0] = (byte)(value ? 1 : 0);
				break;
			}
			case 2:
			{
				WriteUInt16(dest.Slice(0, 2), (ushort)(value ? 1 : 0), endianness);
				break;
			}
			case 4:
			{
				WriteUInt32(dest.Slice(0, 4), value ? 1u : 0, endianness);
				break;
			}
			default: throw new ArgumentOutOfRangeException(nameof(boolSize), boolSize, null);
		}
	}
	public static void WriteBooleans(Span<byte> dest, ReadOnlySpan<bool> src, Endianness endianness, int boolSize)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteBoolean(dest.Slice(i * boolSize, boolSize), src[i], endianness, boolSize);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteEnum<TEnum>(Span<byte> dest, TEnum value, Endianness endianness) where TEnum : unmanaged, Enum
	{
		int size = Unsafe.SizeOf<TEnum>();
		if (size == 1)
		{
			dest[0] = Unsafe.As<TEnum, byte>(ref value);
		}
		else if (size == 2)
		{
			WriteUInt16(dest, Unsafe.As<TEnum, ushort>(ref value), endianness);
		}
		else if (size == 4)
		{
			WriteUInt32(dest, Unsafe.As<TEnum, uint>(ref value), endianness);
		}
		else
		{
			WriteUInt64(dest, Unsafe.As<TEnum, ulong>(ref value), endianness);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteEnums<TEnum>(Span<byte> dest, ReadOnlySpan<TEnum> src, Endianness endianness) where TEnum : unmanaged, Enum
	{
		int size = Unsafe.SizeOf<TEnum>();
		if (size == 1)
		{
			MemoryMarshal.Cast<TEnum, byte>(src).CopyTo(dest);
		}
		else if (size == 2)
		{
			WriteUInt16s(dest, MemoryMarshal.Cast<TEnum, ushort>(src), endianness);
		}
		else if (size == 4)
		{
			WriteUInt32s(dest, MemoryMarshal.Cast<TEnum, uint>(src), endianness);
		}
		else
		{
			WriteUInt64s(dest, MemoryMarshal.Cast<TEnum, ulong>(src), endianness);
		}
	}
	// #13 - Allow writing the abstract "Enum" type
	// For example, EndianBinaryPrimitives.WriteEnum((Enum)Enum.Parse(enumType, value))
	// Don't allow writing Enum[] though, since there is no way to read that
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteEnum(Span<byte> dest, Enum value, Endianness endianness)
	{
		Type underlyingType = Enum.GetUnderlyingType(value.GetType());
		ref byte data = ref Utils.GetRawData(value); // Use memory tricks to skip object header of generic Enum
		switch (Type.GetTypeCode(underlyingType))
		{
			case TypeCode.SByte:
			case TypeCode.Byte: dest[0] = data; break;
			case TypeCode.Int16:
			case TypeCode.UInt16: WriteUInt16(dest, Unsafe.As<byte, ushort>(ref data), endianness); break;
			case TypeCode.Int32:
			case TypeCode.UInt32: WriteUInt32(dest, Unsafe.As<byte, uint>(ref data), endianness); break;
			case TypeCode.Int64:
			case TypeCode.UInt64: WriteUInt64(dest, Unsafe.As<byte, ulong>(ref data), endianness); break;
		}
	}

	public static void WriteDateTime(Span<byte> dest, DateTime value, Endianness endianness)
	{
		WriteInt64(dest, value.ToBinary(), endianness);
	}
	public static void WriteDateTimes(Span<byte> dest, ReadOnlySpan<DateTime> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteDateTime(dest.Slice(i * 8, 8), src[i], endianness);
		}
	}

	public static void WriteDateOnly(Span<byte> dest, DateOnly value, Endianness endianness)
	{
		WriteInt32(dest, value.DayNumber, endianness);
	}
	public static void WriteDateOnlys(Span<byte> dest, ReadOnlySpan<DateOnly> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteDateOnly(dest.Slice(i * 4, 4), src[i], endianness);
		}
	}

	public static void WriteTimeOnly(Span<byte> dest, TimeOnly value, Endianness endianness)
	{
		WriteInt64(dest, value.Ticks, endianness);
	}
	public static void WriteTimeOnlys(Span<byte> dest, ReadOnlySpan<TimeOnly> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteTimeOnly(dest.Slice(i * 8, 8), src[i], endianness);
		}
	}

	public static void WriteVector2(Span<byte> dest, Vector2 value, Endianness endianness)
	{
		WriteSingle(dest.Slice(0, 4), value.X, endianness);
		WriteSingle(dest.Slice(4, 4), value.Y, endianness);
	}
	public static void WriteVector2s(Span<byte> dest, ReadOnlySpan<Vector2> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteVector2(dest.Slice(i * 8, 8), src[i], endianness);
		}
	}

	public static void WriteVector3(Span<byte> dest, Vector3 value, Endianness endianness)
	{
		WriteSingle(dest.Slice(0, 4), value.X, endianness);
		WriteSingle(dest.Slice(4, 4), value.Y, endianness);
		WriteSingle(dest.Slice(8, 4), value.Z, endianness);
	}
	public static void WriteVector3s(Span<byte> dest, ReadOnlySpan<Vector3> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteVector3(dest.Slice(i * 12, 12), src[i], endianness);
		}
	}

	public static void WriteVector4(Span<byte> dest, in Vector4 value, Endianness endianness)
	{
		WriteSingle(dest.Slice(0, 4), value.W, endianness);
		WriteSingle(dest.Slice(4, 4), value.X, endianness);
		WriteSingle(dest.Slice(8, 4), value.Y, endianness);
		WriteSingle(dest.Slice(12, 4), value.Z, endianness);
	}
	public static void WriteVector4s(Span<byte> dest, ReadOnlySpan<Vector4> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteVector4(dest.Slice(i * 16, 16), src[i], endianness);
		}
	}

	public static void WriteQuaternion(Span<byte> dest, in Quaternion value, Endianness endianness)
	{
		WriteSingle(dest.Slice(0, 4), value.W, endianness);
		WriteSingle(dest.Slice(4, 4), value.X, endianness);
		WriteSingle(dest.Slice(8, 4), value.Y, endianness);
		WriteSingle(dest.Slice(12, 4), value.Z, endianness);
	}
	public static void WriteQuaternions(Span<byte> dest, ReadOnlySpan<Quaternion> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteQuaternion(dest.Slice(i * 16, 16), src[i], endianness);
		}
	}

	public static void WriteMatrix4x4(Span<byte> dest, in Matrix4x4 value, Endianness endianness)
	{
		WriteSingle(dest.Slice(0, 4), value.M11, endianness);
		WriteSingle(dest.Slice(4, 4), value.M12, endianness);
		WriteSingle(dest.Slice(8, 4), value.M13, endianness);
		WriteSingle(dest.Slice(12, 4), value.M14, endianness);
		WriteSingle(dest.Slice(16, 4), value.M21, endianness);
		WriteSingle(dest.Slice(20, 4), value.M22, endianness);
		WriteSingle(dest.Slice(24, 4), value.M23, endianness);
		WriteSingle(dest.Slice(28, 4), value.M24, endianness);
		WriteSingle(dest.Slice(32, 4), value.M31, endianness);
		WriteSingle(dest.Slice(36, 4), value.M32, endianness);
		WriteSingle(dest.Slice(40, 4), value.M33, endianness);
		WriteSingle(dest.Slice(44, 4), value.M34, endianness);
		WriteSingle(dest.Slice(48, 4), value.M41, endianness);
		WriteSingle(dest.Slice(52, 4), value.M42, endianness);
		WriteSingle(dest.Slice(56, 4), value.M43, endianness);
		WriteSingle(dest.Slice(60, 4), value.M44, endianness);
	}
	public static void WriteMatrix4x4s(Span<byte> dest, ReadOnlySpan<Matrix4x4> src, Endianness endianness)
	{
		for (int i = 0; i < src.Length; i++)
		{
			WriteMatrix4x4(dest.Slice(i * 64, 64), src[i], endianness);
		}
	}

	#endregion
}
