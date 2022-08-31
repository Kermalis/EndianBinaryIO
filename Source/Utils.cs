using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Kermalis.EndianBinaryIO
{
	internal static class Utils
	{
		private sealed class RawData
		{
			public byte Data;
		}
		// This is a copy of what Enum uses internally
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static ref byte GetRawData<T>(T value) where T : class
		{
			return ref Unsafe.As<RawData>(value).Data; // Skip object header
		}

		private static bool TryConvertToInt32(object? obj, out int value)
		{
			try
			{
				value = Convert.ToInt32(obj);
				return true;
			}
			catch
			{
				value = -1;
				return false;
			}
		}

		public static bool TryGetAttribute<TAttribute>(PropertyInfo propertyInfo, [NotNullWhen(true)] out TAttribute? attribute)
			where TAttribute : Attribute
		{
			object[] attributes = propertyInfo.GetCustomAttributes(typeof(TAttribute), true);
			if (attributes.Length == 1)
			{
				attribute = (TAttribute)attributes[0];
				return true;
			}
			attribute = default;
			return false;
		}
		public static TValue GetAttributeValue<TAttribute, TValue>(TAttribute attribute)
			where TAttribute : Attribute, IBinaryAttribute<TValue>
		{
			return (TValue)typeof(TAttribute).GetProperty(nameof(IBinaryAttribute<TValue>.Value))!.GetValue(attribute)!;
		}
		public static TValue AttributeValueOrDefault<TAttribute, TValue>(PropertyInfo propertyInfo, TValue defaultValue)
			where TAttribute : Attribute, IBinaryAttribute<TValue>
		{
			if (TryGetAttribute(propertyInfo, out TAttribute? attribute))
			{
				return GetAttributeValue<TAttribute, TValue>(attribute);
			}
			return defaultValue;
		}

		public static void ThrowIfCannotReadWriteType(Type type)
		{
			if (type.IsArray || type.IsEnum || type.IsInterface || type.IsPointer || type.IsPrimitive)
			{
				throw new ArgumentException($"Cannot read/write \"{type.FullName}\" objects.", nameof(type));
			}
		}

		public static int GetArrayLength(object obj, Type objType, PropertyInfo propertyInfo)
		{
			if (TryGetAttribute(propertyInfo, out BinaryArrayFixedLengthAttribute? fixedLenAttribute))
			{
				if (propertyInfo.IsDefined(typeof(BinaryArrayVariableLengthAttribute)))
				{
					throw new ArgumentException($"An array property in \"{objType.FullName}\" has two array length attributes. Only one should be provided.");
				}
				return GetAttributeValue<BinaryArrayFixedLengthAttribute, int>(fixedLenAttribute);
			}

			if (TryGetAttribute(propertyInfo, out BinaryArrayVariableLengthAttribute? varLenAttribute))
			{
				string anchorName = GetAttributeValue<BinaryArrayVariableLengthAttribute, string>(varLenAttribute);
				PropertyInfo? anchor = objType.GetProperty(anchorName, BindingFlags.Instance | BindingFlags.Public);
				if (anchor is null)
				{
					throw new MissingMemberException($"An array property in \"{objType.FullName}\" has an invalid {nameof(BinaryArrayVariableLengthAttribute)} ({anchorName}).");
				}

				object? anchorValue = anchor.GetValue(obj);
				if (!TryConvertToInt32(anchorValue, out int length) || length < 0)
				{
					throw new InvalidOperationException($"An array property in \"{objType.FullName}\" has an invalid length attribute ({anchorName} = {anchorValue}).");
				}
				return length;
			}

			throw new MissingMemberException($"An array property in \"{objType.FullName}\" is missing an array length attribute. One should be provided.");
		}
		public static void GetStringLength(object obj, Type objType, PropertyInfo propertyInfo, bool forReads, out bool? nullTerminated, out int stringLength)
		{
			if (TryGetAttribute(propertyInfo, out BinaryStringNullTerminatedAttribute? nullTermAttribute))
			{
				if (propertyInfo.IsDefined(typeof(BinaryStringFixedLengthAttribute)) || propertyInfo.IsDefined(typeof(BinaryStringVariableLengthAttribute)))
				{
					throw new ArgumentException($"A string property in \"{objType.FullName}\" has a string length attribute and a {nameof(BinaryStringNullTerminatedAttribute)}; cannot use both.");
				}
				if (propertyInfo.IsDefined(typeof(BinaryStringTrimNullTerminatorsAttribute)))
				{
					throw new ArgumentException($"A string property in \"{objType.FullName}\" has a {nameof(BinaryStringNullTerminatedAttribute)} and a {nameof(BinaryStringTrimNullTerminatorsAttribute)}; cannot use both.");
				}

				bool nt = GetAttributeValue<BinaryStringNullTerminatedAttribute, bool>(nullTermAttribute);
				if (forReads && !nt) // Not forcing BinaryStringNullTerminatedAttribute to be treated as true since you may only write objects without reading them.
				{
					throw new ArgumentException($"A string property in \"{objType.FullName}\" has a {nameof(BinaryStringNullTerminatedAttribute)} that's set to false." +
						$" Must use null termination or provide a string length when reading.");
				}

				nullTerminated = nt;
				stringLength = -1;
				return;
			}

			if (TryGetAttribute(propertyInfo, out BinaryStringFixedLengthAttribute? fixedLenAttribute))
			{
				if (propertyInfo.IsDefined(typeof(BinaryStringVariableLengthAttribute)))
				{
					throw new ArgumentException($"A string property in \"{objType.FullName}\" has two string length attributes. Only one should be provided.");
				}

				nullTerminated = null;
				stringLength = GetAttributeValue<BinaryStringFixedLengthAttribute, int>(fixedLenAttribute);
				return;
			}

			if (TryGetAttribute(propertyInfo, out BinaryStringVariableLengthAttribute? varLenAttribute))
			{
				string anchorName = GetAttributeValue<BinaryStringVariableLengthAttribute, string>(varLenAttribute);
				PropertyInfo? anchor = objType.GetProperty(anchorName, BindingFlags.Instance | BindingFlags.Public);
				if (anchor is null)
				{
					throw new MissingMemberException($"A string property in \"{objType.FullName}\" has an invalid {nameof(BinaryStringVariableLengthAttribute)} ({anchorName}).");
				}

				nullTerminated = null;
				object? anchorValue = anchor.GetValue(obj);
				if (!TryConvertToInt32(anchorValue, out stringLength) || stringLength < 0)
				{
					throw new InvalidOperationException($"A string property in \"{objType.FullName}\" has an invalid length attribute ({anchorName} = {stringLength}).");
				}
				return;
			}

			throw new MissingMemberException($"A string property in \"{objType.FullName}\" is missing a string length attribute and has no {nameof(BinaryStringNullTerminatedAttribute)}. One should be provided.");
		}
	}
}
