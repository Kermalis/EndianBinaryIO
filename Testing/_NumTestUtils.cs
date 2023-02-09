using Kermalis.EndianBinaryIO;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Kermalis.EndianBinaryIOTests;

internal static class NumTestUtils
{
	public static void ReadValue<T>(bool le, T value, byte[] arrLE, byte[] arrBE,
		Func<EndianBinaryReader, T> read)
	{
		byte[] input = le ? arrLE : arrBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		T val;
		using (var stream = new MemoryStream(input))
		{
			val = read(new EndianBinaryReader(stream, endianness: e));
		}
		Assert.Equal(value, val);
	}
	public static void ReadValues<T>(bool le, T[] values, byte[] arrLE, byte[] arrBE,
		Action<EndianBinaryReader, T[]> read)
	{
		byte[] input = le ? arrLE : arrBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		var arr = new T[values.Length];
		using (var stream = new MemoryStream(input))
		{
			read(new EndianBinaryReader(stream, endianness: e), arr);
		}
		Assert.True(arr.SequenceEqual(values));
	}
	public static void WriteValue<T>(bool le, T value, byte[] arrLE, byte[] arrBE, int sizeOf,
		Action<EndianBinaryWriter, T> write)
	{
		byte[] input = le ? arrLE : arrBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[sizeOf];
		using (var stream = new MemoryStream(bytes))
		{
			write(new EndianBinaryWriter(stream, endianness: e), value);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
	public static void WriteValues<T>(bool le, T[] values, byte[] arrLE, byte[] arrBE, int sizeOf,
		Action<EndianBinaryWriter, T[]> write)
	{
		byte[] input = le ? arrLE : arrBE;
		Endianness e = le ? Endianness.LittleEndian : Endianness.BigEndian;

		byte[] bytes = new byte[values.Length * sizeOf];
		using (var stream = new MemoryStream(bytes))
		{
			write(new EndianBinaryWriter(stream, endianness: e), values);
		}
		Assert.True(bytes.SequenceEqual(input));
	}

	#region SByte/Byte

	public static void ReadValue<T>(T value, byte[] input,
		Func<EndianBinaryReader, T> read)
	{
		T val;
		using (var stream = new MemoryStream(input))
		{
			val = read(new EndianBinaryReader(stream));
		}
		Assert.Equal(value, val);
	}
	public static void ReadValues<T>(T[] values, byte[] input,
		Action<EndianBinaryReader, T[]> read)
	{
		var arr = new T[values.Length];
		using (var stream = new MemoryStream(input))
		{
			read(new EndianBinaryReader(stream), arr);
		}
		Assert.True(arr.SequenceEqual(values));
	}
	public static void WriteValue<T>(T value, byte[] input,
		Action<EndianBinaryWriter, T> write)
	{
		byte[] bytes = new byte[1];
		using (var stream = new MemoryStream(bytes))
		{
			write(new EndianBinaryWriter(stream), value);
		}
		Assert.True(bytes.SequenceEqual(input));
	}
	public static void WriteValues<T>(T[] values, byte[] input,
		Action<EndianBinaryWriter, T[]> write)
	{
		byte[] bytes = new byte[values.Length];
		using (var stream = new MemoryStream(bytes))
		{
			write(new EndianBinaryWriter(stream), values);
		}
		Assert.True(bytes.SequenceEqual(input));
	}

	#endregion
}
