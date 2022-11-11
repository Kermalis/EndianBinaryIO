using System;
using System.Buffers.Binary;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Kermalis.EndianBinaryIO;

public static partial class EndianBinaryPrimitives
{
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static Span<TTo> ValueToSpan<TFrom, TTo>(ref TFrom value, int len)
	{
		return MemoryMarshal.CreateSpan(ref Unsafe.As<TFrom, TTo>(ref value), len);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal static ReadOnlySpan<TTo> ReadCast<TFrom, TTo>(this ReadOnlySpan<TFrom> src, int len)
	{
		return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<TFrom, TTo>(ref MemoryMarshal.GetReference(src)), len);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal static Span<TTo> WriteCast<TFrom, TTo>(this Span<TFrom> dest, int len)
	{
		return MemoryMarshal.CreateSpan(ref Unsafe.As<TFrom, TTo>(ref MemoryMarshal.GetReference(dest)), len);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static T Read<T>(this ReadOnlySpan<byte> src)
	{
		return Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(src));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static void Write<T>(this Span<byte> dest, T value)
	{
		Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(dest), value);
	}

	#region Read

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadSBytes_Unsafe(this ReadOnlySpan<byte> src, Span<sbyte> dest)
	{
		src.ReadCast<byte, sbyte>(dest.Length).CopyTo(dest);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static short ReadInt16_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		short v = src.Read<short>();
		if (endianness != SystemEndianness)
		{
			v = BinaryPrimitives.ReverseEndianness(v);
		}
		return v;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadInt16s_Unsafe(this ReadOnlySpan<byte> src, Span<short> dest, Endianness endianness)
	{
		ReadOnlySpan<short> srcI = src.ReadCast<byte, short>(dest.Length);
		if (endianness == SystemEndianness)
		{
			srcI.CopyTo(dest);
		}
		else
		{
			ReverseEndianness(srcI, dest);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static ushort ReadUInt16_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		ushort v = src.Read<ushort>();
		if (endianness != SystemEndianness)
		{
			v = BinaryPrimitives.ReverseEndianness(v);
		}
		return v;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadUInt16s_Unsafe(this ReadOnlySpan<byte> src, Span<ushort> dest, Endianness endianness)
	{
		ReadOnlySpan<ushort> srcI = src.ReadCast<byte, ushort>(dest.Length);
		if (endianness == SystemEndianness)
		{
			srcI.CopyTo(dest);
		}
		else
		{
			ReverseEndianness(srcI, dest);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static int ReadInt32_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		int v = src.Read<int>();
		if (endianness != SystemEndianness)
		{
			v = BinaryPrimitives.ReverseEndianness(v);
		}
		return v;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadInt32s_Unsafe(this ReadOnlySpan<byte> src, Span<int> dest, Endianness endianness)
	{
		ReadOnlySpan<int> srcI = src.ReadCast<byte, int>(dest.Length);
		if (endianness == SystemEndianness)
		{
			srcI.CopyTo(dest);
		}
		else
		{
			ReverseEndianness(srcI, dest);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static uint ReadUInt32_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		uint v = src.Read<uint>();
		if (endianness != SystemEndianness)
		{
			v = BinaryPrimitives.ReverseEndianness(v);
		}
		return v;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadUInt32s_Unsafe(this ReadOnlySpan<byte> src, Span<uint> dest, Endianness endianness)
	{
		ReadOnlySpan<uint> srcI = src.ReadCast<byte, uint>(dest.Length);
		if (endianness == SystemEndianness)
		{
			srcI.CopyTo(dest);
		}
		else
		{
			ReverseEndianness(srcI, dest);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static long ReadInt64_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		long v = src.Read<long>();
		if (endianness != SystemEndianness)
		{
			v = BinaryPrimitives.ReverseEndianness(v);
		}
		return v;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadInt64s_Unsafe(this ReadOnlySpan<byte> src, Span<long> dest, Endianness endianness)
	{
		ReadOnlySpan<long> srcI = src.ReadCast<byte, long>(dest.Length);
		if (endianness == SystemEndianness)
		{
			srcI.CopyTo(dest);
		}
		else
		{
			ReverseEndianness(srcI, dest);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static ulong ReadUInt64_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		ulong v = src.Read<ulong>();
		if (endianness != SystemEndianness)
		{
			v = BinaryPrimitives.ReverseEndianness(v);
		}
		return v;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadUInt64s_Unsafe(this ReadOnlySpan<byte> src, Span<ulong> dest, Endianness endianness)
	{
		ReadOnlySpan<ulong> srcI = src.ReadCast<byte, ulong>(dest.Length);
		if (endianness == SystemEndianness)
		{
			srcI.CopyTo(dest);
		}
		else
		{
			ReverseEndianness(srcI, dest);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Int128 ReadInt128_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Int128 v = src.Read<Int128>();
		if (endianness != SystemEndianness)
		{
			v = ReverseEndianness(v);
		}
		return v;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadInt128s_Unsafe(this ReadOnlySpan<byte> src, Span<Int128> dest, Endianness endianness)
	{
		ReadOnlySpan<Int128> srcI = src.ReadCast<byte, Int128>(dest.Length);
		if (endianness == SystemEndianness)
		{
			srcI.CopyTo(dest);
		}
		else
		{
			ReverseEndianness(srcI, dest);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static UInt128 ReadUInt128_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		UInt128 v = src.Read<UInt128>();
		if (endianness != SystemEndianness)
		{
			v = ReverseEndianness(v);
		}
		return v;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadUInt128s_Unsafe(this ReadOnlySpan<byte> src, Span<UInt128> dest, Endianness endianness)
	{
		ReadOnlySpan<UInt128> srcI = src.ReadCast<byte, UInt128>(dest.Length);
		if (endianness == SystemEndianness)
		{
			srcI.CopyTo(dest);
		}
		else
		{
			ReverseEndianness(srcI, dest);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Half ReadHalf_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			return src.Read<Half>();
		}
		else
		{
			ushort v = src.Read<ushort>();
			v = BinaryPrimitives.ReverseEndianness(v);
			return BitConverter.UInt16BitsToHalf(v);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadHalves_Unsafe(this ReadOnlySpan<byte> src, Span<Half> dest, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.ReadCast<byte, Half>(dest.Length).CopyTo(dest);
		}
		else
		{
			ReverseEndianness(src.ReadCast<byte, ushort>(dest.Length), dest.WriteCast<Half, ushort>(dest.Length));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static float ReadSingle_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			return src.Read<float>();
		}
		else
		{
			uint v = src.Read<uint>();
			v = BinaryPrimitives.ReverseEndianness(v);
			return BitConverter.UInt32BitsToSingle(v);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadSingles_Unsafe(this ReadOnlySpan<byte> src, Span<float> dest, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.ReadCast<byte, float>(dest.Length).CopyTo(dest);
		}
		else
		{
			ReverseEndianness(src.ReadCast<byte, uint>(dest.Length), dest.WriteCast<float, uint>(dest.Length));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static double ReadDouble_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			return src.Read<double>();
		}
		else
		{
			ulong v = src.Read<ulong>();
			v = BinaryPrimitives.ReverseEndianness(v);
			return BitConverter.UInt64BitsToDouble(v);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadDoubles_Unsafe(this ReadOnlySpan<byte> src, Span<double> dest, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.ReadCast<byte, double>(dest.Length).CopyTo(dest);
		}
		else
		{
			ReverseEndianness(src.ReadCast<byte, ulong>(dest.Length), dest.WriteCast<double, ulong>(dest.Length));
		}
	}

	// Decimal requires validation
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static decimal ReadDecimal_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		Span<int> buffer = stackalloc int[4];
		src.ReadInt32s_Unsafe(buffer, endianness);
		return new decimal(buffer);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadDecimals_Unsafe(this ReadOnlySpan<byte> src, Span<decimal> dest, Endianness endianness)
	{
		ReadOnlySpan<int> srcI = src.ReadCast<byte, int>(dest.Length * 4);
		ReadOnlySpan<int> data;
		if (endianness == SystemEndianness)
		{
			data = srcI;
		}
		else
		{
			Span<int> destI = dest.WriteCast<decimal, int>(dest.Length * 4);
			ReverseEndianness(srcI, destI);
			data = destI;
		}
		for (int i = 0; i < dest.Length; ++i)
		{
			dest[i] = new decimal(data.Slice(i * 4, 4));
		}
	}

	// Boolean requires validation
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool ReadBoolean8_Unsafe(this ReadOnlySpan<byte> src)
	{
		return src[0] != 0;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadBoolean8s_Unsafe(this ReadOnlySpan<byte> src, Span<bool> dest)
	{
		for (int i = 0; i < dest.Length; ++i)
		{
			dest[i] = src[i] != 0;
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool ReadBoolean16_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		return src.ReadUInt16_Unsafe(endianness) != 0;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadBoolean16s_Unsafe(this ReadOnlySpan<byte> src, Span<bool> dest, Endianness endianness)
	{
		ReadOnlySpan<ushort> srcI = src.ReadCast<byte, ushort>(dest.Length);
		for (int i = 0; i < dest.Length; ++i)
		{
			ushort v = srcI[i];
			if (endianness != SystemEndianness)
			{
				v = BinaryPrimitives.ReverseEndianness(v);
			}
			dest[i] = v != 0;
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool ReadBoolean32_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		return src.ReadUInt32_Unsafe(endianness) != 0;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadBoolean32s_Unsafe(this ReadOnlySpan<byte> src, Span<bool> dest, Endianness endianness)
	{
		ReadOnlySpan<uint> srcI = src.ReadCast<byte, uint>(dest.Length);
		for (int i = 0; i < dest.Length; ++i)
		{
			uint v = srcI[i];
			if (endianness != SystemEndianness)
			{
				v = BinaryPrimitives.ReverseEndianness(v);
			}
			dest[i] = v != 0;
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool ReadBoolean_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness, BooleanSize boolSize)
	{
		return src.ReadBoolean_Unsafe(endianness, GetBytesForBooleanSize(boolSize));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadBooleans_Unsafe(this ReadOnlySpan<byte> src, Span<bool> dest, Endianness endianness, BooleanSize boolSize)
	{
		src.ReadBooleans_Unsafe(dest, endianness, GetBytesForBooleanSize(boolSize));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static bool ReadBoolean_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness, int boolSize)
	{
		switch (boolSize)
		{
			case 1: return src[0] != 0;
			case 2: return src.ReadUInt16_Unsafe(endianness) != 0;
			case 4: return src.ReadUInt32_Unsafe(endianness) != 0;
			default: throw new ArgumentOutOfRangeException(nameof(boolSize), boolSize, null);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadBooleans_Unsafe(this ReadOnlySpan<byte> src, Span<bool> dest, Endianness endianness, int boolSize)
	{
		switch (boolSize)
		{
			case 1: src.ReadBoolean8s_Unsafe(dest); break;
			case 2: src.ReadBoolean16s_Unsafe(dest, endianness); break;
			case 4: src.ReadBoolean32s_Unsafe(dest, endianness); break;
			default: throw new ArgumentOutOfRangeException(nameof(boolSize), boolSize, null);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static TEnum ReadEnum_Unsafe<TEnum>(this ReadOnlySpan<byte> src, Endianness endianness)
		where TEnum : unmanaged, Enum
	{
		int size = Unsafe.SizeOf<TEnum>();
		if (size == 1)
		{
			byte b = src[0];
			return Unsafe.As<byte, TEnum>(ref b);
		}
		if (size == 2)
		{
			ushort s = src.ReadUInt16_Unsafe(endianness);
			return Unsafe.As<ushort, TEnum>(ref s);
		}
		if (size == 4)
		{
			uint i = src.ReadUInt32_Unsafe(endianness);
			return Unsafe.As<uint, TEnum>(ref i);
		}
		ulong l = src.ReadUInt64_Unsafe(endianness);
		return Unsafe.As<ulong, TEnum>(ref l);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadEnums_Unsafe<TEnum>(this ReadOnlySpan<byte> src, Span<TEnum> dest, Endianness endianness)
		where TEnum : unmanaged, Enum
	{
		src.ReadCast<byte, TEnum>(dest.Length).CopyTo(dest);

		if (endianness == SystemEndianness)
		{
			return; // All below are for flipping nonmatching endianness
		}

		int size = Unsafe.SizeOf<TEnum>();
		if (size == 1)
		{
			return;
		}
		else if (size == 2)
		{
			Span<ushort> destI = dest.WriteCast<TEnum, ushort>(dest.Length);
			ReverseEndianness(destI, destI);
		}
		else if (size == 4)
		{
			Span<uint> destI = dest.WriteCast<TEnum, uint>(dest.Length);
			ReverseEndianness(destI, destI);
		}
		else
		{
			Span<ulong> destI = dest.WriteCast<TEnum, ulong>(dest.Length);
			ReverseEndianness(destI, destI);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static object ReadEnum_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness, Type enumType)
	{
		// Type.IsEnum is also false for base Enum type, so don't worry about it
		Type underlyingType = Enum.GetUnderlyingType(enumType);
		switch (Type.GetTypeCode(underlyingType))
		{
			case TypeCode.SByte: return Enum.ToObject(enumType, (sbyte)src[0]);
			case TypeCode.Byte: return Enum.ToObject(enumType, src[0]);
			case TypeCode.Int16: return Enum.ToObject(enumType, src.ReadInt16_Unsafe(endianness));
			case TypeCode.UInt16: return Enum.ToObject(enumType, src.ReadUInt16_Unsafe(endianness));
			case TypeCode.Int32: return Enum.ToObject(enumType, src.ReadInt32_Unsafe(endianness));
			case TypeCode.UInt32: return Enum.ToObject(enumType, src.ReadUInt32_Unsafe(endianness));
			case TypeCode.Int64: return Enum.ToObject(enumType, src.ReadInt64_Unsafe(endianness));
			case TypeCode.UInt64: return Enum.ToObject(enumType, src.ReadUInt64_Unsafe(endianness));
		}
		throw new ArgumentOutOfRangeException(nameof(enumType), enumType, null);
	}

	// DateTime requires validation
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static DateTime ReadDateTime_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		return DateTime.FromBinary(src.ReadInt64_Unsafe(endianness));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadDateTimes_Unsafe(this ReadOnlySpan<byte> src, Span<DateTime> dest, Endianness endianness)
	{
		ReadOnlySpan<long> srcI = src.ReadCast<byte, long>(dest.Length);
		ReadOnlySpan<long> data;
		if (endianness == SystemEndianness)
		{
			data = srcI;
		}
		else
		{
			Span<long> destI = dest.WriteCast<DateTime, long>(dest.Length);
			ReverseEndianness(srcI, destI);
			data = destI;
		}
		for (int i = 0; i < dest.Length; ++i)
		{
			dest[i] = DateTime.FromBinary(data[i]);
		}
	}

	// DateOnly requires validation
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static DateOnly ReadDateOnly_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		return DateOnly.FromDayNumber(src.ReadInt32_Unsafe(endianness));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadDateOnlys_Unsafe(this ReadOnlySpan<byte> src, Span<DateOnly> dest, Endianness endianness)
	{
		ReadOnlySpan<int> srcI = src.ReadCast<byte, int>(dest.Length);
		ReadOnlySpan<int> data;
		if (endianness == SystemEndianness)
		{
			data = srcI;
		}
		else
		{
			Span<int> destI = dest.WriteCast<DateOnly, int>(dest.Length);
			ReverseEndianness(srcI, destI);
			data = destI;
		}
		for (int i = 0; i < dest.Length; ++i)
		{
			dest[i] = DateOnly.FromDayNumber(data[i]);
		}
	}

	// TimeOnly requires validation
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static TimeOnly ReadTimeOnly_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		return new TimeOnly(src.ReadInt64_Unsafe(endianness));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadTimeOnlys_Unsafe(this ReadOnlySpan<byte> src, Span<TimeOnly> dest, Endianness endianness)
	{
		ReadOnlySpan<long> srcI = src.ReadCast<byte, long>(dest.Length);
		ReadOnlySpan<long> data;
		if (endianness == SystemEndianness)
		{
			data = srcI;
		}
		else
		{
			Span<long> destI = dest.WriteCast<TimeOnly, long>(dest.Length);
			ReverseEndianness(srcI, destI);
			data = destI;
		}
		for (int i = 0; i < dest.Length; ++i)
		{
			dest[i] = new TimeOnly(data[i]);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Vector2 ReadVector2_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			return src.Read<Vector2>();
		}
		else
		{
			Vector2 v = default;
			ReverseEndianness(src.ReadCast<byte, uint>(2), ValueToSpan<Vector2, uint>(ref v, 2));
			return v;
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadVector2s_Unsafe(this ReadOnlySpan<byte> src, Span<Vector2> dest, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.ReadCast<byte, Vector2>(dest.Length).CopyTo(dest);
		}
		else
		{
			ReverseEndianness(src.ReadCast<byte, uint>(dest.Length * 2), dest.WriteCast<Vector2, uint>(dest.Length * 2));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Vector3 ReadVector3_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			return src.Read<Vector3>();
		}
		else
		{
			Vector3 v = default;
			ReverseEndianness(src.ReadCast<byte, uint>(3), ValueToSpan<Vector3, uint>(ref v, 3));
			return v;
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadVector3s_Unsafe(this ReadOnlySpan<byte> src, Span<Vector3> dest, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.ReadCast<byte, Vector3>(dest.Length).CopyTo(dest);
		}
		else
		{
			ReverseEndianness(src.ReadCast<byte, uint>(dest.Length * 3), dest.WriteCast<Vector3, uint>(dest.Length * 3));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Vector4 ReadVector4_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			return src.Read<Vector4>();
		}
		else
		{
			Vector4 v = default;
			ReverseEndianness(src.ReadCast<byte, uint>(4), ValueToSpan<Vector4, uint>(ref v, 4));
			return v;
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadVector4s_Unsafe(this ReadOnlySpan<byte> src, Span<Vector4> dest, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.ReadCast<byte, Vector4>(dest.Length).CopyTo(dest);
		}
		else
		{
			ReverseEndianness(src.ReadCast<byte, uint>(dest.Length * 4), dest.WriteCast<Vector4, uint>(dest.Length * 4));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Quaternion ReadQuaternion_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			return src.Read<Quaternion>();
		}
		else
		{
			Quaternion v = default;
			ReverseEndianness(src.ReadCast<byte, uint>(4), ValueToSpan<Quaternion, uint>(ref v, 4));
			return v;
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadQuaternions_Unsafe(this ReadOnlySpan<byte> src, Span<Quaternion> dest, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.ReadCast<byte, Quaternion>(dest.Length).CopyTo(dest);
		}
		else
		{
			ReverseEndianness(src.ReadCast<byte, uint>(dest.Length * 4), dest.WriteCast<Quaternion, uint>(dest.Length * 4));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static Matrix4x4 ReadMatrix4x4_Unsafe(this ReadOnlySpan<byte> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			return src.Read<Matrix4x4>();
		}
		else
		{
			Matrix4x4 v = default;
			ReverseEndianness(src.ReadCast<byte, uint>(16), ValueToSpan<Matrix4x4, uint>(ref v, 16));
			return v;
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void ReadMatrix4x4s_Unsafe(this ReadOnlySpan<byte> src, Span<Matrix4x4> dest, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.ReadCast<byte, Matrix4x4>(dest.Length).CopyTo(dest);
		}
		else
		{
			ReverseEndianness(src.ReadCast<byte, uint>(dest.Length * 16), dest.WriteCast<Matrix4x4, uint>(dest.Length * 16));
		}
	}

	#endregion

	#region Write

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteSBytes_Unsafe(this Span<byte> dest, ReadOnlySpan<sbyte> src)
	{
		src.CopyTo(dest.WriteCast<byte, sbyte>(src.Length));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteInt16_Unsafe(this Span<byte> dest, short value, Endianness endianness)
	{
		if (endianness != SystemEndianness)
		{
			value = BinaryPrimitives.ReverseEndianness(value);
		}
		dest.Write(value);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteInt16s_Unsafe(this Span<byte> dest, ReadOnlySpan<short> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.CopyTo(dest.WriteCast<byte, short>(src.Length));
		}
		else
		{
			ReverseEndianness(src, dest.WriteCast<byte, short>(src.Length));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteUInt16_Unsafe(this Span<byte> dest, ushort value, Endianness endianness)
	{
		if (endianness != SystemEndianness)
		{
			value = BinaryPrimitives.ReverseEndianness(value);
		}
		dest.Write(value);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteUInt16s_Unsafe(this Span<byte> dest, ReadOnlySpan<ushort> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.CopyTo(dest.WriteCast<byte, ushort>(src.Length));
		}
		else
		{
			ReverseEndianness(src, dest.WriteCast<byte, ushort>(src.Length));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteInt32_Unsafe(this Span<byte> dest, int value, Endianness endianness)
	{
		if (endianness != SystemEndianness)
		{
			value = BinaryPrimitives.ReverseEndianness(value);
		}
		dest.Write(value);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteInt32s_Unsafe(this Span<byte> dest, ReadOnlySpan<int> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.CopyTo(dest.WriteCast<byte, int>(src.Length));
		}
		else
		{
			ReverseEndianness(src, dest.WriteCast<byte, int>(src.Length));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteUInt32_Unsafe(this Span<byte> dest, uint value, Endianness endianness)
	{
		if (endianness != SystemEndianness)
		{
			value = BinaryPrimitives.ReverseEndianness(value);
		}
		dest.Write(value);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteUInt32s_Unsafe(this Span<byte> dest, ReadOnlySpan<uint> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.CopyTo(dest.WriteCast<byte, uint>(src.Length));
		}
		else
		{
			ReverseEndianness(src, dest.WriteCast<byte, uint>(src.Length));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteInt64_Unsafe(this Span<byte> dest, long value, Endianness endianness)
	{
		if (endianness != SystemEndianness)
		{
			value = BinaryPrimitives.ReverseEndianness(value);
		}
		dest.Write(value);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteInt64s_Unsafe(this Span<byte> dest, ReadOnlySpan<long> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.CopyTo(dest.WriteCast<byte, long>(src.Length));
		}
		else
		{
			ReverseEndianness(src, dest.WriteCast<byte, long>(src.Length));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteUInt64_Unsafe(this Span<byte> dest, ulong value, Endianness endianness)
	{
		if (endianness != SystemEndianness)
		{
			value = BinaryPrimitives.ReverseEndianness(value);
		}
		dest.Write(value);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteUInt64s_Unsafe(this Span<byte> dest, ReadOnlySpan<ulong> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.CopyTo(dest.WriteCast<byte, ulong>(src.Length));
		}
		else
		{
			ReverseEndianness(src, dest.WriteCast<byte, ulong>(src.Length));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteInt128_Unsafe(this Span<byte> dest, Int128 value, Endianness endianness)
	{
		if (endianness != SystemEndianness)
		{
			value = ReverseEndianness(value);
		}
		dest.Write(value);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteInt128s_Unsafe(this Span<byte> dest, ReadOnlySpan<Int128> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.CopyTo(dest.WriteCast<byte, Int128>(src.Length));
		}
		else
		{
			ReverseEndianness(src, dest.WriteCast<byte, Int128>(src.Length));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteUInt128_Unsafe(this Span<byte> dest, UInt128 value, Endianness endianness)
	{
		if (endianness != SystemEndianness)
		{
			value = ReverseEndianness(value);
		}
		dest.Write(value);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteUInt128s_Unsafe(this Span<byte> dest, ReadOnlySpan<UInt128> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.CopyTo(dest.WriteCast<byte, UInt128>(src.Length));
		}
		else
		{
			ReverseEndianness(src, dest.WriteCast<byte, UInt128>(src.Length));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteHalf_Unsafe(this Span<byte> dest, Half value, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			dest.Write(value);
		}
		else
		{
			ushort v = BitConverter.HalfToUInt16Bits(value);
			v = BinaryPrimitives.ReverseEndianness(v);
			dest.Write(v);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteHalves_Unsafe(this Span<byte> dest, ReadOnlySpan<Half> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.CopyTo(dest.WriteCast<byte, Half>(src.Length));
		}
		else
		{
			ReverseEndianness(src.ReadCast<Half, ushort>(src.Length), dest.WriteCast<byte, ushort>(src.Length));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteSingle_Unsafe(this Span<byte> dest, float value, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			dest.Write(value);
		}
		else
		{
			uint v = BitConverter.SingleToUInt32Bits(value);
			v = BinaryPrimitives.ReverseEndianness(v);
			dest.Write(v);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteSingles_Unsafe(this Span<byte> dest, ReadOnlySpan<float> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.CopyTo(dest.WriteCast<byte, float>(src.Length));
		}
		else
		{
			ReverseEndianness(src.ReadCast<float, uint>(src.Length), dest.WriteCast<byte, uint>(src.Length));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteDouble_Unsafe(this Span<byte> dest, double value, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			dest.Write(value);
		}
		else
		{
			ulong v = BitConverter.DoubleToUInt64Bits(value);
			v = BinaryPrimitives.ReverseEndianness(v);
			dest.Write(v);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteDoubles_Unsafe(this Span<byte> dest, ReadOnlySpan<double> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.CopyTo(dest.WriteCast<byte, double>(src.Length));
		}
		else
		{
			ReverseEndianness(src.ReadCast<double, ulong>(src.Length), dest.WriteCast<byte, ulong>(src.Length));
		}
	}

	// Decimal requires validation
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteDecimal_Unsafe(this Span<byte> dest, in decimal value, Endianness endianness)
	{
		Span<int> buffer = stackalloc int[4];
		decimal.GetBits(value, buffer);
		dest.WriteInt32s_Unsafe(buffer, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteDecimals_Unsafe(this Span<byte> dest, ReadOnlySpan<decimal> src, Endianness endianness)
	{
		Span<int> destI = dest.WriteCast<byte, int>(src.Length * 4);
		for (int i = 0; i < src.Length; ++i)
		{
			decimal.GetBits(src[i], destI.Slice(i * 4, 4));
		}
		if (endianness != SystemEndianness)
		{
			ReverseEndianness(destI, destI);
		}
	}

	// Boolean requires validation
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBoolean8_Unsafe(this Span<byte> dest, bool value)
	{
		dest[0] = (byte)(value ? 1 : 0);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBoolean8s_Unsafe(this Span<byte> dest, ReadOnlySpan<bool> src)
	{
		for (int i = 0; i < src.Length; ++i)
		{
			dest[i] = (byte)(src[i] ? 1 : 0);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBoolean16_Unsafe(this Span<byte> dest, bool value, Endianness endianness)
	{
		dest.WriteUInt16_Unsafe((ushort)(value ? 1 : 0), endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBoolean16s_Unsafe(this Span<byte> dest, ReadOnlySpan<bool> src, Endianness endianness)
	{
		Span<ushort> destI = dest.WriteCast<byte, ushort>(src.Length);
		for (int i = 0; i < src.Length; ++i)
		{
			destI[i] = (ushort)(src[i] ? 1 : 0);
		}
		if (endianness != SystemEndianness)
		{
			ReverseEndianness(destI, destI);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBoolean32_Unsafe(this Span<byte> dest, bool value, Endianness endianness)
	{
		dest.WriteUInt16_Unsafe((ushort)(value ? 1 : 0), endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBoolean32s_Unsafe(this Span<byte> dest, ReadOnlySpan<bool> src, Endianness endianness)
	{
		Span<uint> destI = dest.WriteCast<byte, uint>(src.Length);
		for (int i = 0; i < src.Length; ++i)
		{
			destI[i] = src[i] ? 1u : 0;
		}
		if (endianness != SystemEndianness)
		{
			ReverseEndianness(destI, destI);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBoolean_Unsafe(this Span<byte> dest, bool value, Endianness endianness, BooleanSize boolSize)
	{
		dest.WriteBoolean_Unsafe(value, endianness, GetBytesForBooleanSize(boolSize));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBooleans_Unsafe(this Span<byte> dest, ReadOnlySpan<bool> src, Endianness endianness, BooleanSize boolSize)
	{
		dest.WriteBooleans_Unsafe(src, endianness, GetBytesForBooleanSize(boolSize));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBoolean_Unsafe(this Span<byte> dest, bool value, Endianness endianness, int boolSize)
	{
		switch (boolSize)
		{
			case 1: dest[0] = (byte)(value ? 1 : 0); break;
			case 2: dest.WriteUInt16_Unsafe((ushort)(value ? 1 : 0), endianness); break;
			case 4: dest.WriteUInt32_Unsafe(value ? 1u : 0, endianness); break;
			default: throw new ArgumentOutOfRangeException(nameof(boolSize), boolSize, null);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteBooleans_Unsafe(this Span<byte> dest, ReadOnlySpan<bool> src, Endianness endianness, int boolSize)
	{
		switch (boolSize)
		{
			case 1: dest.WriteBoolean8s_Unsafe(src); break;
			case 2: dest.WriteBoolean16s_Unsafe(src, endianness); break;
			case 4: dest.WriteBoolean32s_Unsafe(src, endianness); break;
			default: throw new ArgumentOutOfRangeException(nameof(boolSize), boolSize, null);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteEnum_Unsafe<TEnum>(this Span<byte> dest, TEnum value, Endianness endianness)
		where TEnum : unmanaged, Enum
	{
		int size = Unsafe.SizeOf<TEnum>();
		if (size == sizeof(byte))
		{
			dest[0] = Unsafe.As<TEnum, byte>(ref value);
		}
		else if (size == sizeof(ushort))
		{
			dest.WriteUInt16_Unsafe(Unsafe.As<TEnum, ushort>(ref value), endianness);
		}
		else if (size == sizeof(uint))
		{
			dest.WriteUInt32_Unsafe(Unsafe.As<TEnum, uint>(ref value), endianness);
		}
		else
		{
			dest.WriteUInt64_Unsafe(Unsafe.As<TEnum, ulong>(ref value), endianness);
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteEnums_Unsafe<TEnum>(this Span<byte> dest, ReadOnlySpan<TEnum> src, Endianness endianness)
		where TEnum : unmanaged, Enum
	{
		int size = Unsafe.SizeOf<TEnum>();
		if (size == sizeof(byte))
		{
			src.CopyTo(dest.WriteCast<byte, TEnum>(src.Length));
		}
		else if (size == sizeof(ushort))
		{
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
	public static void WriteEnum_Unsafe(this Span<byte> dest, Enum value, Endianness endianness)
	{
		Type underlyingType = Enum.GetUnderlyingType(value.GetType());
		ref byte data = ref Utils.GetRawData(value); // Use memory tricks to skip object header of generic Enum
		switch (Type.GetTypeCode(underlyingType))
		{
			case TypeCode.SByte:
			case TypeCode.Byte:
			{
				dest[0] = data;
				break;
			}
			case TypeCode.Int16:
			case TypeCode.UInt16:
			{
				dest.WriteUInt16_Unsafe(Unsafe.As<byte, ushort>(ref data), endianness);
				break;
			}
			case TypeCode.Int32:
			case TypeCode.UInt32:
			{
				dest.WriteUInt32_Unsafe(Unsafe.As<byte, uint>(ref data), endianness);
				break;
			}
			case TypeCode.Int64:
			case TypeCode.UInt64:
			{
				dest.WriteUInt64_Unsafe(Unsafe.As<byte, ulong>(ref data), endianness);
				break;
			}
		}
	}

	// DateTime requires validation
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteDateTime_Unsafe(this Span<byte> dest, DateTime value, Endianness endianness)
	{
		dest.WriteInt64_Unsafe(value.ToBinary(), endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteDateTimes_Unsafe(this Span<byte> dest, ReadOnlySpan<DateTime> src, Endianness endianness)
	{
		Span<long> destI = dest.WriteCast<byte, long>(src.Length);
		for (int i = 0; i < src.Length; ++i)
		{
			destI[i] = src[i].ToBinary();
		}
		if (endianness != SystemEndianness)
		{
			ReverseEndianness(destI, destI);
		}
	}

	// DateOnly requires validation
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteDateOnly_Unsafe(this Span<byte> dest, DateOnly value, Endianness endianness)
	{
		dest.WriteInt32_Unsafe(value.DayNumber, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteDateOnlys_Unsafe(this Span<byte> dest, ReadOnlySpan<DateOnly> src, Endianness endianness)
	{
		Span<int> destI = dest.WriteCast<byte, int>(src.Length);
		for (int i = 0; i < src.Length; ++i)
		{
			destI[i] = src[i].DayNumber;
		}
		if (endianness != SystemEndianness)
		{
			ReverseEndianness(destI, destI);
		}
	}

	// TimeOnly requires validation
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteTimeOnly_Unsafe(this Span<byte> dest, TimeOnly value, Endianness endianness)
	{
		dest.WriteInt64_Unsafe(value.Ticks, endianness);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteTimeOnlys_Unsafe(this Span<byte> dest, ReadOnlySpan<TimeOnly> src, Endianness endianness)
	{
		Span<long> destI = dest.WriteCast<byte, long>(src.Length);
		for (int i = 0; i < src.Length; ++i)
		{
			destI[i] = src[i].Ticks;
		}
		if (endianness != SystemEndianness)
		{
			ReverseEndianness(destI, destI);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteVector2_Unsafe(this Span<byte> dest, Vector2 value, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			dest.Write(value);
		}
		else
		{
			ReverseEndianness(ValueToSpan<Vector2, uint>(ref value, 2), dest.WriteCast<byte, uint>(2));
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteVector2s_Unsafe(this Span<byte> dest, ReadOnlySpan<Vector2> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.CopyTo(dest.WriteCast<byte, Vector2>(src.Length));
		}
		else
		{
			ReverseEndianness(src.ReadCast<Vector2, uint>(src.Length * 2), dest.WriteCast<byte, uint>(src.Length * 2));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteVector3_Unsafe(this Span<byte> dest, Vector3 value, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			dest.Write(value);
		}
		else
		{
			ReverseEndianness(ValueToSpan<Vector3, uint>(ref value, 3), dest.WriteCast<byte, uint>(3));
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteVector3s_Unsafe(this Span<byte> dest, ReadOnlySpan<Vector3> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.CopyTo(dest.WriteCast<byte, Vector3>(src.Length));
		}
		else
		{
			ReverseEndianness(src.ReadCast<Vector3, uint>(src.Length * 3), dest.WriteCast<byte, uint>(src.Length * 3));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteVector4_Unsafe(this Span<byte> dest, Vector4 value, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			dest.Write(value);
		}
		else
		{
			ReverseEndianness(ValueToSpan<Vector4, uint>(ref value, 4), dest.WriteCast<byte, uint>(4));
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteVector4s_Unsafe(this Span<byte> dest, ReadOnlySpan<Vector4> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.CopyTo(dest.WriteCast<byte, Vector4>(src.Length));
		}
		else
		{
			ReverseEndianness(src.ReadCast<Vector4, uint>(src.Length * 4), dest.WriteCast<byte, uint>(src.Length * 4));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteQuaternion_Unsafe(this Span<byte> dest, Quaternion value, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			dest.Write(value);
		}
		else
		{
			ReverseEndianness(ValueToSpan<Quaternion, uint>(ref value, 4), dest.WriteCast<byte, uint>(4));
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteQuaternions_Unsafe(this Span<byte> dest, ReadOnlySpan<Quaternion> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.CopyTo(dest.WriteCast<byte, Quaternion>(src.Length));
		}
		else
		{
			ReverseEndianness(src.ReadCast<Quaternion, uint>(src.Length * 4), dest.WriteCast<byte, uint>(src.Length * 4));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteMatrix4x4_Unsafe(this Span<byte> dest, Matrix4x4 value, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			dest.Write(value);
		}
		else
		{
			ReverseEndianness(ValueToSpan<Matrix4x4, uint>(ref value, 16), dest.WriteCast<byte, uint>(16));
		}
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static void WriteMatrix4x4s_Unsafe(this Span<byte> dest, ReadOnlySpan<Matrix4x4> src, Endianness endianness)
	{
		if (endianness == SystemEndianness)
		{
			src.CopyTo(dest.WriteCast<byte, Matrix4x4>(src.Length));
		}
		else
		{
			ReverseEndianness(src.ReadCast<Matrix4x4, uint>(src.Length * 16), dest.WriteCast<byte, uint>(src.Length * 16));
		}
	}

	#endregion

	#region Endianness Reverse (Stolen from BinaryPrimitives)

	// Why are these internal in BinaryPrimitives?
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static Int128 ReverseEndianness(Int128 value)
	{
		return new Int128( // Don't use Marshal/Unsafe since upper and lower are swapped depending on system endianness
			BinaryPrimitives.ReverseEndianness((ulong)value), // _lower
			BinaryPrimitives.ReverseEndianness((ulong)(value >> 64)) // _upper
		);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static UInt128 ReverseEndianness(UInt128 value)
	{
		return new UInt128(
			BinaryPrimitives.ReverseEndianness((ulong)value), // _lower
			BinaryPrimitives.ReverseEndianness((ulong)(value >> 64)) // _upper
		);
	}

	private interface IEndiannessReverser<T>
		where T : struct
	{
		static abstract T Reverse(T value);
		static abstract Vector128<T> Reverse(Vector128<T> vector);
		static abstract Vector256<T> Reverse(Vector256<T> vector);
	}
	private readonly struct Int16EndiannessReverser : IEndiannessReverser<short>
	{
		public static short Reverse(short value)
		{
			return BinaryPrimitives.ReverseEndianness(value);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector128<short> Reverse(Vector128<short> vector)
		{
			return Vector128.ShiftLeft(vector, 8) | Vector128.ShiftRightLogical(vector, 8);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector256<short> Reverse(Vector256<short> vector)
		{
			return Vector256.ShiftLeft(vector, 8) | Vector256.ShiftRightLogical(vector, 8);
		}
	}
	private readonly struct UInt16EndiannessReverser : IEndiannessReverser<ushort>
	{
		public static ushort Reverse(ushort value)
		{
			return BinaryPrimitives.ReverseEndianness(value);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector128<ushort> Reverse(Vector128<ushort> vector)
		{
			return Vector128.ShiftLeft(vector, 8) | Vector128.ShiftRightLogical(vector, 8);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector256<ushort> Reverse(Vector256<ushort> vector)
		{
			return Vector256.ShiftLeft(vector, 8) | Vector256.ShiftRightLogical(vector, 8);
		}
	}
	private readonly struct Int32EndiannessReverser : IEndiannessReverser<int>
	{
		public static int Reverse(int value)
		{
			return BinaryPrimitives.ReverseEndianness(value);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector128<int> Reverse(Vector128<int> vector)
		{
			return Vector128.Shuffle(vector.AsByte(), Vector128.Create((byte)3, 2, 1, 0, 7, 6, 5, 4, 11, 10, 9, 8, 15, 14, 13, 12)).AsInt32();
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector256<int> Reverse(Vector256<int> vector)
		{
			return Vector256.Shuffle(vector.AsByte(), Vector256.Create((byte)3, 2, 1, 0, 7, 6, 5, 4, 11, 10, 9, 8, 15, 14, 13, 12, 19, 18, 17, 16, 23, 22, 21, 20, 27, 26, 25, 24, 31, 30, 29, 28)).AsInt32();
		}
	}
	private readonly struct UInt32EndiannessReverser : IEndiannessReverser<uint>
	{
		public static uint Reverse(uint value)
		{
			return BinaryPrimitives.ReverseEndianness(value);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector128<uint> Reverse(Vector128<uint> vector)
		{
			return Vector128.Shuffle(vector.AsByte(), Vector128.Create((byte)3, 2, 1, 0, 7, 6, 5, 4, 11, 10, 9, 8, 15, 14, 13, 12)).AsUInt32();
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector256<uint> Reverse(Vector256<uint> vector)
		{
			return Vector256.Shuffle(vector.AsByte(), Vector256.Create((byte)3, 2, 1, 0, 7, 6, 5, 4, 11, 10, 9, 8, 15, 14, 13, 12, 19, 18, 17, 16, 23, 22, 21, 20, 27, 26, 25, 24, 31, 30, 29, 28)).AsUInt32();
		}
	}
	private readonly struct Int64EndiannessReverser : IEndiannessReverser<long>
	{
		public static long Reverse(long value)
		{
			return BinaryPrimitives.ReverseEndianness(value);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector128<long> Reverse(Vector128<long> vector)
		{
			return Vector128.Shuffle(vector.AsByte(), Vector128.Create((byte)7, 6, 5, 4, 3, 2, 1, 0, 15, 14, 13, 12, 11, 10, 9, 8)).AsInt64();
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector256<long> Reverse(Vector256<long> vector)
		{
			return Vector256.Shuffle(vector.AsByte(), Vector256.Create((byte)7, 6, 5, 4, 3, 2, 1, 0, 15, 14, 13, 12, 11, 10, 9, 8, 23, 22, 21, 20, 19, 18, 17, 16, 31, 30, 29, 28, 27, 26, 25, 24)).AsInt64();
		}
	}
	private readonly struct UInt64EndiannessReverser : IEndiannessReverser<ulong>
	{
		public static ulong Reverse(ulong value)
		{
			return BinaryPrimitives.ReverseEndianness(value);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector128<ulong> Reverse(Vector128<ulong> vector)
		{
			return Vector128.Shuffle(vector.AsByte(), Vector128.Create((byte)7, 6, 5, 4, 3, 2, 1, 0, 15, 14, 13, 12, 11, 10, 9, 8)).AsUInt64();
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector256<ulong> Reverse(Vector256<ulong> vector)
		{
			return Vector256.Shuffle(vector.AsByte(), Vector256.Create((byte)7, 6, 5, 4, 3, 2, 1, 0, 15, 14, 13, 12, 11, 10, 9, 8, 23, 22, 21, 20, 19, 18, 17, 16, 31, 30, 29, 28, 27, 26, 25, 24)).AsUInt64();
		}
	}

	private static void ReverseEndianness(ReadOnlySpan<short> src, Span<short> dest)
	{
		ReverseEndianness<short, Int16EndiannessReverser>(src, dest);
	}
	private static void ReverseEndianness(ReadOnlySpan<ushort> src, Span<ushort> dest)
	{
		ReverseEndianness<ushort, UInt16EndiannessReverser>(src, dest);
	}
	private static void ReverseEndianness(ReadOnlySpan<int> src, Span<int> dest)
	{
		ReverseEndianness<int, Int32EndiannessReverser>(src, dest);
	}
	private static void ReverseEndianness(ReadOnlySpan<uint> src, Span<uint> dest)
	{
		ReverseEndianness<uint, UInt32EndiannessReverser>(src, dest);
	}
	private static void ReverseEndianness(ReadOnlySpan<long> src, Span<long> dest)
	{
		ReverseEndianness<long, Int64EndiannessReverser>(src, dest);
	}
	private static void ReverseEndianness(ReadOnlySpan<ulong> src, Span<ulong> dest)
	{
		ReverseEndianness<ulong, UInt64EndiannessReverser>(src, dest);
	}
	private static void ReverseEndianness<T, TReverser>(ReadOnlySpan<T> src, Span<T> dest)
		where T : struct
		where TReverser : IEndiannessReverser<T>
	{
		ref T srcRef = ref MemoryMarshal.GetReference(src);
		ref T destRef = ref MemoryMarshal.GetReference(dest);

		int i = 0;

		if (Vector256.IsHardwareAccelerated)
		{
			while (i <= src.Length - Vector256<T>.Count)
			{
				Vector256.StoreUnsafe(TReverser.Reverse(Vector256.LoadUnsafe(ref srcRef, (uint)i)), ref destRef, (uint)i);
				i += Vector256<T>.Count;
			}
		}
		if (Vector128.IsHardwareAccelerated)
		{
			while (i <= src.Length - Vector128<T>.Count)
			{
				Vector128.StoreUnsafe(TReverser.Reverse(Vector128.LoadUnsafe(ref srcRef, (uint)i)), ref destRef, (uint)i);
				i += Vector128<T>.Count;
			}
		}

		while (i < src.Length)
		{
			Unsafe.Add(ref destRef, i) = TReverser.Reverse(Unsafe.Add(ref srcRef, i));
			i++;
		}
	}
	private static void ReverseEndianness(ReadOnlySpan<Int128> src, Span<Int128> dest)
	{
		for (int i = 0; i < src.Length; ++i)
		{
			dest[i] = ReverseEndianness(src[i]);
		}
	}
	private static void ReverseEndianness(ReadOnlySpan<UInt128> src, Span<UInt128> dest)
	{
		for (int i = 0; i < src.Length; ++i)
		{
			dest[i] = ReverseEndianness(src[i]);
		}
	}

	#endregion
}
