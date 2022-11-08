using System;
using System.Numerics;
using System.Reflection;

namespace Kermalis.EndianBinaryIO;

public partial class EndianBinaryWriter
{
	public void WriteObject(object obj)
	{
		WriteObject(obj, false);
	}
	private void WriteObject(object obj, bool skipCheck)
	{
		if (!skipCheck && TryWriteSupportedObject(obj))
		{
			return;
		}

		Type objType = obj.GetType();
		Utils.ThrowIfCannotReadWriteType(objType);

		// Get public non-static properties
		foreach (PropertyInfo propertyInfo in objType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
		{
			if (Utils.AttributeValueOrDefault<BinaryIgnoreAttribute, bool>(propertyInfo, false))
			{
				continue; // Skip properties with BinaryIgnoreAttribute
			}

			object? value = propertyInfo.GetValue(obj);
			if (value is null)
			{
				throw new NullReferenceException($"Null property in {objType.FullName} ({propertyInfo.Name})");
			}
			if (value is Array arr)
			{
				int arrayLength = Utils.GetArrayLength(obj, objType, propertyInfo);
				if (arrayLength != 0) // Do not need to do anything for length 0
				{
					if (arrayLength > arr.Length)
					{
						throw new InvalidOperationException($"Expected too many elements for array in {objType.FullName} (expected {arrayLength}, have {arr.Length}).");
					}
					WritePropertyValue_Array(obj, objType, propertyInfo, arr, arrayLength);
				}
			}
			else
			{
				WritePropertyValue_NonArray(obj, objType, propertyInfo, value);
			}
		}
	}

	private bool TryWriteSupportedObject(object obj)
	{
		return obj is Array arr ? TryWriteSupportedObject_Array(arr, arr.Length) : TryWriteSupportedObject_NonArray(obj);
	}
	private bool TryWriteSupportedObject_NonArray(object obj)
	{
		switch (obj)
		{
			case sbyte v: WriteSByte(v); return true;
			case byte v: WriteByte(v); return true;
			case short v: WriteInt16(v); return true;
			case ushort v: WriteUInt16(v); return true;
			case int v: WriteInt32(v); return true;
			case uint v: WriteUInt32(v); return true;
			case long v: WriteInt64(v); return true;
			case ulong v: WriteUInt64(v); return true;
			case Half v: WriteHalf(v); return true;
			case float v: WriteSingle(v); return true;
			case double v: WriteDouble(v); return true;
			case decimal v: WriteDecimal(v); return true;
			case bool v: WriteBoolean(v); return true;
			case Enum v: WriteEnum(v); return true;
			case DateTime v: WriteDateTime(v); return true;
			case DateOnly v: WriteDateOnly(v); return true;
			case TimeOnly v: WriteTimeOnly(v); return true;
			case Vector2 v: WriteVector2(v); return true;
			case Vector3 v: WriteVector3(v); return true;
			case Vector4 v: WriteVector4(v); return true;
			case Quaternion v: WriteQuaternion(v); return true;
			case Matrix4x4 v: WriteMatrix4x4(v); return true;
			case char v: WriteChar(v); return true;
			case string v: WriteChars_NullTerminated(v); return true;
			case IBinarySerializable v: v.Write(this); return true;
		}
		return false;
	}
	private bool TryWriteSupportedObject_Array(Array obj, int length)
	{
		Type elementType = obj.GetType().GetElementType()!;
		if (elementType.IsEnum)
		{
			for (int i = 0; i < length; i++)
			{
				WriteObject(obj.GetValue(i)!);
			}
			return true;
		}

		switch (obj)
		{
			case sbyte[] v: WriteSBytes(v.AsSpan(0, length)); return true;
			case byte[] v: WriteBytes(v.AsSpan(0, length)); return true;
			case short[] v: WriteInt16s(v.AsSpan(0, length)); return true;
			case ushort[] v: WriteUInt16s(v.AsSpan(0, length)); return true;
			case int[] v: WriteInt32s(v.AsSpan(0, length)); return true;
			case uint[] v: WriteUInt32s(v.AsSpan(0, length)); return true;
			case long[] v: WriteInt64s(v.AsSpan(0, length)); return true;
			case ulong[] v: WriteUInt64s(v.AsSpan(0, length)); return true;
			case Half[] v: WriteHalves(v.AsSpan(0, length)); return true;
			case float[] v: WriteSingles(v.AsSpan(0, length)); return true;
			case double[] v: WriteDoubles(v.AsSpan(0, length)); return true;
			case decimal[] v: WriteDecimals(v.AsSpan(0, length)); return true;
			case bool[] v: WriteBooleans(v.AsSpan(0, length)); return true;
			case DateTime[] v: WriteDateTimes(v.AsSpan(0, length)); return true;
			case DateOnly[] v: WriteDateOnlys(v.AsSpan(0, length)); return true;
			case TimeOnly[] v: WriteTimeOnlys(v.AsSpan(0, length)); return true;
			case Vector2[] v: WriteVector2s(v.AsSpan(0, length)); return true;
			case Vector3[] v: WriteVector3s(v.AsSpan(0, length)); return true;
			case Vector4[] v: WriteVector4s(v.AsSpan(0, length)); return true;
			case Quaternion[] v: WriteQuaternions(v.AsSpan(0, length)); return true;
			case Matrix4x4[] v: WriteMatrix4x4s(v.AsSpan(0, length)); return true;
			case char[] v: WriteChars(v.AsSpan(0, length)); return true;
			case string[] v: WriteStrings_NullTerminated(v.AsSpan(0, length)); return true;
		}
		return false;
	}

	private void WritePropertyValue_NonArray(object obj, Type objType, PropertyInfo propertyInfo, object value)
	{
		switch (value)
		{
			case int v:
			{
				bool isInt24 = Utils.AttributeValueOrDefault<BinaryInt24Attribute, bool>(propertyInfo, false);
				if (isInt24)
				{
					WriteInt24(v);
				}
				else
				{
					WriteInt32(v);
				}
				break;
			}
			case uint v:
			{
				bool isInt24 = Utils.AttributeValueOrDefault<BinaryInt24Attribute, bool>(propertyInfo, false);
				if (isInt24)
				{
					WriteUInt24(v);
				}
				else
				{
					WriteUInt32(v);
				}
				break;
			}
			case bool v:
			{
				BooleanSize old = BooleanSize;
				BooleanSize = Utils.AttributeValueOrDefault<BinaryBooleanSizeAttribute, BooleanSize>(propertyInfo, old);
				WriteBoolean(v);
				BooleanSize = old;
				break;
			}
			case char v:
			{
				bool old = ASCII;
				ASCII = Utils.AttributeValueOrDefault<BinaryASCIIAttribute, bool>(propertyInfo, old);
				WriteChar(v);
				ASCII = old;
				break;
			}
			case string v:
			{
				Utils.GetStringLength(obj, objType, propertyInfo, false, out bool? nullTerminated, out int stringLength);

				bool old = ASCII;
				ASCII = Utils.AttributeValueOrDefault<BinaryASCIIAttribute, bool>(propertyInfo, old);
				if (nullTerminated == true)
				{
					WriteChars_NullTerminated(v);
				}
				else if (nullTerminated == false)
				{
					WriteChars(v);
				}
				else
				{
					WriteChars_Count(v, stringLength);
				}
				ASCII = old;
				break;
			}
			default:
			{
				WriteObject(value);
				break;
			}
		}
	}
	private void WritePropertyValue_Array(object obj, Type objType, PropertyInfo propertyInfo, Array value, int arrayLength)
	{
		switch (value)
		{
			case int[] v:
			{
				bool isInt24 = Utils.AttributeValueOrDefault<BinaryInt24Attribute, bool>(propertyInfo, false);
				Span<int> values = v.AsSpan(0, arrayLength);
				if (isInt24)
				{
					WriteInt24s(values);
				}
				else
				{
					WriteInt32s(values);
				}
				break;
			}
			case uint[] v:
			{
				bool isInt24 = Utils.AttributeValueOrDefault<BinaryInt24Attribute, bool>(propertyInfo, false);
				Span<uint> values = v.AsSpan(0, arrayLength);
				if (isInt24)
				{
					WriteUInt24s(values);
				}
				else
				{
					WriteUInt32s(values);
				}
				break;
			}
			case bool[] v:
			{
				BooleanSize old = BooleanSize;
				BooleanSize = Utils.AttributeValueOrDefault<BinaryBooleanSizeAttribute, BooleanSize>(propertyInfo, old);
				WriteBooleans(v.AsSpan(0, arrayLength));
				BooleanSize = old;
				break;
			}
			case char[] v:
			{
				bool old = ASCII;
				ASCII = Utils.AttributeValueOrDefault<BinaryASCIIAttribute, bool>(propertyInfo, old);
				WriteChars(v.AsSpan(0, arrayLength));
				ASCII = old;
				break;
			}
			case string[] v:
			{
				Utils.GetStringLength(obj, objType, propertyInfo, false, out bool? nullTerminated, out int stringLength);

				bool old = ASCII;
				ASCII = Utils.AttributeValueOrDefault<BinaryASCIIAttribute, bool>(propertyInfo, old);
				Span<string> values = v.AsSpan(0, arrayLength);
				if (nullTerminated == true)
				{
					WriteStrings_NullTerminated(values);
				}
				else if (nullTerminated == false)
				{
					WriteStrings(values);
				}
				else
				{
					WriteStrings_Count(values, stringLength);
				}
				ASCII = old;
				break;
			}
			default:
			{
				if (!TryWriteSupportedObject_Array(value, arrayLength))
				{
					for (int i = 0; i < arrayLength; i++)
					{
						object? val = value.GetValue(i);
						if (val is null)
						{
							throw new NullReferenceException("Array element was null.");
						}
						WriteObject(val, true);
					}
				}
				break;
			}
		}
	}
}
