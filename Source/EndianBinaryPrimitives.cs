using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Kermalis.EndianBinaryIO;

public static partial class EndianBinaryPrimitives
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
	public static void ReadSBytes(this ReadOnlySpan<byte> src, Span<sbyte> dest)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, dest.Length);
		src.ReadSBytes_Unsafe(dest);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static short ReadInt16(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, sizeof(short));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadInt16_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadInt16s(this ReadOnlySpan<byte> src, Span<short> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(short));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadInt16s_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static ushort ReadUInt16(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, sizeof(ushort));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadUInt16_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadUInt16s(this ReadOnlySpan<byte> src, Span<ushort> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(ushort));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadUInt16s_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static int ReadInt32(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, sizeof(int));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadInt32_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadInt32s(this ReadOnlySpan<byte> src, Span<int> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(int));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadInt32s_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static uint ReadUInt32(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, sizeof(uint));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadUInt32_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadUInt32s(this ReadOnlySpan<byte> src, Span<uint> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(uint));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadUInt32s_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static long ReadInt64(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, sizeof(long));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadInt64_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadInt64s(this ReadOnlySpan<byte> src, Span<long> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(long));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadInt64s_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static ulong ReadUInt64(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, sizeof(ulong));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadUInt64_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadUInt64s(this ReadOnlySpan<byte> src, Span<ulong> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(ulong));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadUInt64s_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Int128 ReadInt128(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, 2 * sizeof(ulong));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadInt128_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadInt128s(this ReadOnlySpan<byte> src, Span<Int128> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * 2 * sizeof(ulong));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadInt128s_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static UInt128 ReadUInt128(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, 2 * sizeof(ulong));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadUInt128_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadUInt128s(this ReadOnlySpan<byte> src, Span<UInt128> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * 2 * sizeof(ulong));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadUInt128s_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Half ReadHalf(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, sizeof(ushort));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadHalf_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadHalves(this ReadOnlySpan<byte> src, Span<Half> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(ushort));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadHalves_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float ReadSingle(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, sizeof(float));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadSingle_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadSingles(this ReadOnlySpan<byte> src, Span<float> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(float));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadSingles_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double ReadDouble(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, sizeof(double));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadDouble_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadDoubles(this ReadOnlySpan<byte> src, Span<double> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(double));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadDoubles_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static decimal ReadDecimal(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, sizeof(decimal));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadDecimal_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadDecimals(this ReadOnlySpan<byte> src, Span<decimal> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(decimal));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadDecimals_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool ReadBoolean8(this ReadOnlySpan<byte> src)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, sizeof(byte));
		return src[0] != 0;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadBoolean8s(this ReadOnlySpan<byte> src, Span<bool> dest)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length);
			src.ReadBoolean8s_Unsafe(dest);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool ReadBoolean16(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		return src.ReadUInt16(endianness) != 0;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadBoolean16s(this ReadOnlySpan<byte> src, Span<bool> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(ushort));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadBoolean16s_Unsafe(dest, endianness);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool ReadBoolean32(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		return src.ReadUInt32(endianness) != 0;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadBoolean32s(this ReadOnlySpan<byte> src, Span<bool> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(uint));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadBoolean32s_Unsafe(dest, endianness);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool ReadBoolean(this ReadOnlySpan<byte> src, Endianness endianness, BooleanSize boolSize)
	{
		return src.ReadBoolean(endianness, GetBytesForBooleanSize(boolSize));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadBooleans(this ReadOnlySpan<byte> src, Span<bool> dest, Endianness endianness, BooleanSize boolSize)
	{
		src.ReadBooleans(dest, endianness, GetBytesForBooleanSize(boolSize));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool ReadBoolean(this ReadOnlySpan<byte> src, Endianness endianness, int boolSize)
	{
		switch (boolSize)
		{
			case 1: return src.ReadBoolean8();
			case 2: return src.ReadBoolean16(endianness);
			case 4: return src.ReadBoolean32(endianness);
			default: throw new ArgumentOutOfRangeException(nameof(boolSize), boolSize, null);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadBooleans(this ReadOnlySpan<byte> src, Span<bool> dest, Endianness endianness, int boolSize)
	{
		switch (boolSize)
		{
			case 1: src.ReadBoolean8s(dest); break;
			case 2: src.ReadBoolean16s(dest, endianness); break;
			case 4: src.ReadBoolean32s(dest, endianness); break;
			default: throw new ArgumentOutOfRangeException(nameof(boolSize), boolSize, null);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static TEnum ReadEnum<TEnum>(this ReadOnlySpan<byte> src, Endianness endianness)
		where TEnum : unmanaged, Enum
	{
		int size = Unsafe.SizeOf<TEnum>();
		if (size == 1)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, sizeof(byte));
			byte b = src[0];
			return Unsafe.As<byte, TEnum>(ref b);
		}
		if (size == 2)
		{
			ushort s = src.ReadUInt16(endianness);
			return Unsafe.As<ushort, TEnum>(ref s);
		}
		if (size == 4)
		{
			uint i = src.ReadUInt32(endianness);
			return Unsafe.As<uint, TEnum>(ref i);
		}
		ulong l = src.ReadUInt64(endianness);
		return Unsafe.As<ulong, TEnum>(ref l);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadEnums<TEnum>(this ReadOnlySpan<byte> src, Span<TEnum> dest, Endianness endianness)
		where TEnum : unmanaged, Enum
	{
		if (dest.Length == 0)
		{
			return;
		}

		int size = Unsafe.SizeOf<TEnum>();
		if (size == sizeof(byte))
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length);
			src.ReadCast<byte, TEnum>(dest.Length).CopyTo(dest);
		}
		else if (size == sizeof(ushort))
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(ushort));
			Utils.ThrowIfInvalidEndianness(endianness);
			if (endianness == SystemEndianness)
			{
				src.ReadCast<byte, TEnum>(dest.Length).CopyTo(dest);
			}
			else
			{
				ReverseEndianness(src.ReadCast<byte, ushort>(dest.Length), dest.WriteCast<TEnum, ushort>(dest.Length));
			}
		}
		else if (size == sizeof(uint))
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(uint));
			Utils.ThrowIfInvalidEndianness(endianness);
			if (endianness == SystemEndianness)
			{
				src.ReadCast<byte, TEnum>(dest.Length).CopyTo(dest);
			}
			else
			{
				ReverseEndianness(src.ReadCast<byte, uint>(dest.Length), dest.WriteCast<TEnum, uint>(dest.Length));
			}
		}
		else
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(ulong));
			Utils.ThrowIfInvalidEndianness(endianness);
			if (endianness == SystemEndianness)
			{
				src.ReadCast<byte, TEnum>(dest.Length).CopyTo(dest);
			}
			else
			{
				ReverseEndianness(src.ReadCast<byte, ulong>(dest.Length), dest.WriteCast<TEnum, ulong>(dest.Length));
			}
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static object ReadEnum(this ReadOnlySpan<byte> src, Endianness endianness, Type enumType)
	{
		// Type.IsEnum is also false for base Enum type, so don't worry about it
		Type underlyingType = Enum.GetUnderlyingType(enumType);
		switch (Type.GetTypeCode(underlyingType))
		{
			case TypeCode.SByte:
			{
				Utils.ThrowIfSrcTooSmall(src.Length, sizeof(sbyte));
				return Enum.ToObject(enumType, (sbyte)src[0]);
			}
			case TypeCode.Byte:
			{
				Utils.ThrowIfSrcTooSmall(src.Length, sizeof(byte));
				return Enum.ToObject(enumType, src[0]);
			}
			case TypeCode.Int16:
			{
				return Enum.ToObject(enumType, src.ReadInt16(endianness));
			}
			case TypeCode.UInt16:
			{
				return Enum.ToObject(enumType, src.ReadUInt16(endianness));
			}
			case TypeCode.Int32:
			{
				return Enum.ToObject(enumType, src.ReadInt32(endianness));
			}
			case TypeCode.UInt32:
			{
				return Enum.ToObject(enumType, src.ReadUInt32(endianness));
			}
			case TypeCode.Int64:
			{
				return Enum.ToObject(enumType, src.ReadInt64(endianness));
			}
			case TypeCode.UInt64:
			{
				return Enum.ToObject(enumType, src.ReadUInt64(endianness));
			}
		}
		throw new ArgumentOutOfRangeException(nameof(enumType), enumType, null);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static DateTime ReadDateTime(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, sizeof(long));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadDateTime_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadDateTimes(this ReadOnlySpan<byte> src, Span<DateTime> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(long));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadDateTimes_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static DateOnly ReadDateOnly(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, sizeof(int));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadDateOnly_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadDateOnlys(this ReadOnlySpan<byte> src, Span<DateOnly> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(int));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadDateOnlys_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static TimeOnly ReadTimeOnly(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, sizeof(long));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadTimeOnly_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadTimeOnlys(this ReadOnlySpan<byte> src, Span<TimeOnly> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * sizeof(long));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadTimeOnlys_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Vector2 ReadVector2(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, 2 * sizeof(float));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadVector2_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadVector2s(this ReadOnlySpan<byte> src, Span<Vector2> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * 2 * sizeof(float));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadVector2s_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Vector3 ReadVector3(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, 3 * sizeof(float));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadVector3_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadVector3s(this ReadOnlySpan<byte> src, Span<Vector3> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * 3 * sizeof(float));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadVector3s_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Vector4 ReadVector4(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, 4 * sizeof(float));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadVector4_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadVector4s(this ReadOnlySpan<byte> src, Span<Vector4> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * 4 * sizeof(float));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadVector4s_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Quaternion ReadQuaternion(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, 4 * sizeof(float));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadQuaternion_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadQuaternions(this ReadOnlySpan<byte> src, Span<Quaternion> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * 4 * sizeof(float));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadQuaternions_Unsafe(dest, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Matrix4x4 ReadMatrix4x4(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Utils.ThrowIfSrcTooSmall(src.Length, 16 * sizeof(float));
		Utils.ThrowIfInvalidEndianness(endianness);
		return src.ReadMatrix4x4_Unsafe(endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadMatrix4x4s(this ReadOnlySpan<byte> src, Span<Matrix4x4> dest, Endianness endianness)
	{
		if (dest.Length != 0)
		{
			Utils.ThrowIfSrcTooSmall(src.Length, dest.Length * 16 * sizeof(float));
			Utils.ThrowIfInvalidEndianness(endianness);
			src.ReadMatrix4x4s_Unsafe(dest, endianness);
		}
	}

	#endregion

	#region Write

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteSBytes(this Span<byte> dest, ReadOnlySpan<sbyte> src)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, src.Length);
		dest.WriteSBytes_Unsafe(src);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteInt16(this Span<byte> dest, short value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, sizeof(short));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteInt16_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteInt16s(this Span<byte> dest, ReadOnlySpan<short> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(short));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteInt16s_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteUInt16(this Span<byte> dest, ushort value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, sizeof(ushort));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteUInt16_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteUInt16s(this Span<byte> dest, ReadOnlySpan<ushort> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(ushort));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteUInt16s_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteInt32(this Span<byte> dest, int value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, sizeof(int));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteInt32_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteInt32s(this Span<byte> dest, ReadOnlySpan<int> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(int));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteInt32s_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteUInt32(this Span<byte> dest, uint value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, sizeof(uint));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteUInt32_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteUInt32s(this Span<byte> dest, ReadOnlySpan<uint> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(uint));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteUInt32s_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteInt64(this Span<byte> dest, long value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, sizeof(long));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteInt64_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteInt64s(this Span<byte> dest, ReadOnlySpan<long> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(long));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteInt64s_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteUInt64(this Span<byte> dest, ulong value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, sizeof(ulong));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteUInt64_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteUInt64s(this Span<byte> dest, ReadOnlySpan<ulong> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(ulong));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteUInt64s_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteInt128(this Span<byte> dest, Int128 value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, 2 * sizeof(ulong));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteInt128_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteInt128s(this Span<byte> dest, ReadOnlySpan<Int128> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * 2 * sizeof(ulong));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteInt128s_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteUInt128(this Span<byte> dest, UInt128 value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, 2 * sizeof(ulong));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteUInt128_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteUInt128s(this Span<byte> dest, ReadOnlySpan<UInt128> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * 2 * sizeof(ulong));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteUInt128s_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteHalf(this Span<byte> dest, Half value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, sizeof(ushort));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteHalf_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteHalves(this Span<byte> dest, ReadOnlySpan<Half> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(ushort));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteHalves_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteSingle(this Span<byte> dest, float value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, sizeof(float));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteSingle_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteSingles(this Span<byte> dest, ReadOnlySpan<float> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(float));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteSingles_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteDouble(this Span<byte> dest, double value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, sizeof(double));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteDouble_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteDoubles(this Span<byte> dest, ReadOnlySpan<double> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(double));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteDoubles_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteDecimal(this Span<byte> dest, in decimal value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, sizeof(decimal));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteDecimal_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteDecimals(this Span<byte> dest, ReadOnlySpan<decimal> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(decimal));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteDecimals_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBoolean8(this Span<byte> dest, bool value)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, sizeof(byte));
		dest[0] = (byte)(value ? 1 : 0);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBoolean8s(this Span<byte> dest, ReadOnlySpan<bool> src)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length);
			dest.WriteBoolean8s_Unsafe(src);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBoolean16(this Span<byte> dest, bool value, Endianness endianness)
	{
		dest.WriteUInt16((ushort)(value ? 1 : 0), endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBoolean16s(this Span<byte> dest, ReadOnlySpan<bool> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(ushort));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteBoolean16s_Unsafe(src, endianness);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBoolean32(this Span<byte> dest, bool value, Endianness endianness)
	{
		dest.WriteUInt32(value ? 1u : 0, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBoolean32s(this Span<byte> dest, ReadOnlySpan<bool> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(uint));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteBoolean32s_Unsafe(src, endianness);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBoolean(this Span<byte> dest, bool value, Endianness endianness, BooleanSize boolSize)
	{
		dest.WriteBoolean(value, endianness, GetBytesForBooleanSize(boolSize));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBooleans(this Span<byte> dest, ReadOnlySpan<bool> src, Endianness endianness, BooleanSize boolSize)
	{
		dest.WriteBooleans(src, endianness, GetBytesForBooleanSize(boolSize));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBoolean(this Span<byte> dest, bool value, Endianness endianness, int boolSize)
	{
		switch (boolSize)
		{
			case 1: dest.WriteBoolean8(value); break;
			case 2: dest.WriteBoolean16(value, endianness); break;
			case 4: dest.WriteBoolean32(value, endianness); break;
			default: throw new ArgumentOutOfRangeException(nameof(boolSize), boolSize, null);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBooleans(this Span<byte> dest, ReadOnlySpan<bool> src, Endianness endianness, int boolSize)
	{
		switch (boolSize)
		{
			case 1: dest.WriteBoolean8s(src); break;
			case 2: dest.WriteBoolean16s(src, endianness); break;
			case 4: dest.WriteBoolean32s(src, endianness); break;
			default: throw new ArgumentOutOfRangeException(nameof(boolSize), boolSize, null);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteEnum<TEnum>(this Span<byte> dest, TEnum value, Endianness endianness)
		where TEnum : unmanaged, Enum
	{
		int size = Unsafe.SizeOf<TEnum>();
		if (size == 1)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, sizeof(byte));
			dest[0] = Unsafe.As<TEnum, byte>(ref value);
		}
		else if (size == 2)
		{
			dest.WriteUInt16(Unsafe.As<TEnum, ushort>(ref value), endianness);
		}
		else if (size == 4)
		{
			dest.WriteUInt32(Unsafe.As<TEnum, uint>(ref value), endianness);
		}
		else
		{
			dest.WriteUInt64(Unsafe.As<TEnum, ulong>(ref value), endianness);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteEnums<TEnum>(this Span<byte> dest, ReadOnlySpan<TEnum> src, Endianness endianness)
		where TEnum : unmanaged, Enum
	{
		if (src.Length == 0)
		{
			return;
		}

		int size = Unsafe.SizeOf<TEnum>();
		if (size == sizeof(byte))
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length);
			src.CopyTo(dest.WriteCast<byte, TEnum>(src.Length));
		}
		else if (size == sizeof(ushort))
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(ushort));
			Utils.ThrowIfInvalidEndianness(endianness);
			if (endianness == SystemEndianness)
			{
				src.CopyTo(dest.WriteCast<byte, TEnum>(src.Length));
			}
			else
			{
				ReverseEndianness(src.ReadCast<TEnum, ushort>(src.Length), dest.WriteCast<byte, ushort>(src.Length));
			}
		}
		else if (size == sizeof(uint))
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(uint));
			Utils.ThrowIfInvalidEndianness(endianness);
			if (endianness == SystemEndianness)
			{
				src.CopyTo(dest.WriteCast<byte, TEnum>(src.Length));
			}
			else
			{
				ReverseEndianness(src.ReadCast<TEnum, uint>(src.Length), dest.WriteCast<byte, uint>(src.Length));
			}
		}
		else
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(ulong));
			Utils.ThrowIfInvalidEndianness(endianness);
			if (endianness == SystemEndianness)
			{
				src.CopyTo(dest.WriteCast<byte, TEnum>(src.Length));
			}
			else
			{
				ReverseEndianness(src.ReadCast<TEnum, ulong>(src.Length), dest.WriteCast<byte, ulong>(src.Length));
			}
		}
	}
	// #13 - Allow writing the abstract "Enum" type
	// For example, EndianBinaryPrimitives.WriteEnum((Enum)Enum.Parse(enumType, value))
	// Don't allow writing Enum[] though, since there is no way to read that
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteEnum(this Span<byte> dest, Enum value, Endianness endianness)
	{
		Type underlyingType = Enum.GetUnderlyingType(value.GetType());
		ref byte data = ref Utils.GetRawData(value); // Use memory tricks to skip object header of generic Enum
		switch (Type.GetTypeCode(underlyingType))
		{
			case TypeCode.SByte:
			case TypeCode.Byte:
			{
				Utils.ThrowIfDestTooSmall(dest.Length, sizeof(byte));
				dest[0] = data;
				break;
			}
			case TypeCode.Int16:
			case TypeCode.UInt16:
			{
				dest.WriteUInt16(Unsafe.As<byte, ushort>(ref data), endianness);
				break;
			}
			case TypeCode.Int32:
			case TypeCode.UInt32:
			{
				dest.WriteUInt32(Unsafe.As<byte, uint>(ref data), endianness);
				break;
			}
			case TypeCode.Int64:
			case TypeCode.UInt64:
			{
				dest.WriteUInt64(Unsafe.As<byte, ulong>(ref data), endianness);
				break;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteDateTime(this Span<byte> dest, DateTime value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, sizeof(long));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteDateTime_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteDateTimes(this Span<byte> dest, ReadOnlySpan<DateTime> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(long));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteDateTimes_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteDateOnly(this Span<byte> dest, DateOnly value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, sizeof(int));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteDateOnly_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteDateOnlys(this Span<byte> dest, ReadOnlySpan<DateOnly> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(int));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteDateOnlys_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteTimeOnly(this Span<byte> dest, TimeOnly value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, sizeof(long));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteTimeOnly_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteTimeOnlys(this Span<byte> dest, ReadOnlySpan<TimeOnly> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * sizeof(long));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteTimeOnlys_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteVector2(this Span<byte> dest, Vector2 value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, 2 * sizeof(float));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteVector2_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteVector2s(this Span<byte> dest, ReadOnlySpan<Vector2> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * 2 * sizeof(float));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteVector2s_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteVector3(this Span<byte> dest, Vector3 value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, 3 * sizeof(float));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteVector3_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteVector3s(this Span<byte> dest, ReadOnlySpan<Vector3> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * 3 * sizeof(float));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteVector3s_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteVector4(this Span<byte> dest, in Vector4 value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, 4 * sizeof(float));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteVector4_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteVector4s(this Span<byte> dest, ReadOnlySpan<Vector4> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * 4 * sizeof(float));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteVector4s_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteQuaternion(this Span<byte> dest, in Quaternion value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, 4 * sizeof(float));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteQuaternion_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteQuaternions(this Span<byte> dest, ReadOnlySpan<Quaternion> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * 4 * sizeof(float));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteQuaternions_Unsafe(src, endianness);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteMatrix4x4(this Span<byte> dest, in Matrix4x4 value, Endianness endianness)
	{
		Utils.ThrowIfDestTooSmall(dest.Length, 16 * sizeof(float));
		Utils.ThrowIfInvalidEndianness(endianness);
		dest.WriteMatrix4x4_Unsafe(value, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteMatrix4x4s(this Span<byte> dest, ReadOnlySpan<Matrix4x4> src, Endianness endianness)
	{
		if (src.Length != 0)
		{
			Utils.ThrowIfDestTooSmall(dest.Length, src.Length * 16 * sizeof(float));
			Utils.ThrowIfInvalidEndianness(endianness);
			dest.WriteMatrix4x4s_Unsafe(src, endianness);
		}
	}

	#endregion
}
