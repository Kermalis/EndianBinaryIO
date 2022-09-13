using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection;

namespace Kermalis.EndianBinaryIO;

public partial class EndianBinaryReader
{
	public void ReadIntoObject(object obj)
	{
		if (obj is IBinarySerializable bs)
		{
			bs.Read(this);
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

			Type propertyType = propertyInfo.PropertyType;
			object value = propertyType.IsArray
				? ReadPropertyValue_Array(obj, objType, propertyInfo, propertyType)
				: ReadPropertyValue_NonArray(obj, objType, propertyInfo, propertyType);

			propertyInfo.SetValue(obj, value); // Set the value into the property
		}
	}
	public T ReadObject<T>()
	{
		return (T)ReadObject(typeof(T));
	}
	public object ReadObject(Type objType)
	{
		if (!TryReadSupportedObject(objType, out object? obj))
		{
			Utils.ThrowIfCannotReadWriteType(objType);
			obj = Activator.CreateInstance(objType)!;
			ReadIntoObject(obj);
		}
		return obj;
	}

	private bool TryReadSupportedObject(Type objType, [NotNullWhen(true)] out object? obj)
	{
		switch (objType)
		{
			case Type t when t == typeof(sbyte): { obj = ReadSByte(); return true; }
			case Type t when t == typeof(byte): { obj = ReadByte(); return true; }
			case Type t when t == typeof(short): { obj = ReadInt16(); return true; }
			case Type t when t == typeof(ushort): { obj = ReadUInt16(); return true; }
			case Type t when t == typeof(int): { obj = ReadInt32(); return true; }
			case Type t when t == typeof(uint): { obj = ReadUInt32(); return true; }
			case Type t when t == typeof(long): { obj = ReadInt64(); return true; }
			case Type t when t == typeof(ulong): { obj = ReadUInt64(); return true; }
			case Type t when t == typeof(Half): { obj = ReadHalf(); return true; }
			case Type t when t == typeof(float): { obj = ReadSingle(); return true; }
			case Type t when t == typeof(double): { obj = ReadDouble(); return true; }
			case Type t when t == typeof(decimal): { obj = ReadDecimal(); return true; }
			case Type t when t == typeof(bool): { obj = ReadBoolean(); return true; }
			case Type t when t.IsEnum: { obj = ReadEnum(t); return true; }
			case Type t when t == typeof(DateTime): { obj = ReadDateTime(); return true; }
			case Type t when t == typeof(DateOnly): { obj = ReadDateOnly(); return true; }
			case Type t when t == typeof(TimeOnly): { obj = ReadTimeOnly(); return true; }
			case Type t when t == typeof(Vector2): { obj = ReadVector2(); return true; }
			case Type t when t == typeof(Vector3): { obj = ReadVector3(); return true; }
			case Type t when t == typeof(Vector4): { obj = ReadVector4(); return true; }
			case Type t when t == typeof(Quaternion): { obj = ReadQuaternion(); return true; }
			case Type t when t == typeof(Matrix4x4): { obj = ReadMatrix4x4(); return true; }
			case Type t when t == typeof(char): { obj = ReadChar(); return true; }
			case Type t when t == typeof(string): { obj = ReadString_NullTerminated(); return true; }
		}

		obj = default;
		return false;
	}

	private object ReadPropertyValue_NonArray(object obj, Type objType, PropertyInfo propertyInfo, Type propertyType)
	{
		switch (propertyType)
		{
			case Type t when t == typeof(bool):
			{
				BooleanSize old = BooleanSize;
				BooleanSize = Utils.AttributeValueOrDefault<BinaryBooleanSizeAttribute, BooleanSize>(propertyInfo, old);
				bool value = ReadBoolean();
				BooleanSize = old;
				return value;
			}
			case Type t when t == typeof(char):
			{
				bool old = ASCII;
				ASCII = Utils.AttributeValueOrDefault<BinaryASCIIAttribute, bool>(propertyInfo, old);
				char value = ReadChar();
				ASCII = old;
				return value;
			}
			case Type t when t == typeof(string):
			{
				Utils.GetStringLength(obj, objType, propertyInfo, true, out bool? nullTerminated, out int stringLength);

				bool old = ASCII;
				ASCII = Utils.AttributeValueOrDefault<BinaryASCIIAttribute, bool>(propertyInfo, old);
				string value;
				if (nullTerminated == true)
				{
					value = ReadString_NullTerminated();
				}
				else
				{
					bool trimNullTerminators = Utils.AttributeValueOrDefault<BinaryStringTrimNullTerminatorsAttribute, bool>(propertyInfo, false);
					if (trimNullTerminators)
					{
						value = ReadString_Count_TrimNullTerminators(stringLength);
					}
					else
					{
						value = ReadString_Count(stringLength);
					}
				}
				ASCII = old;
				return value;
			}
			default:
			{
				return ReadObject(propertyType);
			}
		}
	}
	private object ReadPropertyValue_Array(object obj, Type objType, PropertyInfo propertyInfo, Type propertyType)
	{
		int arrayLength = Utils.GetArrayLength(obj, objType, propertyInfo);
		Type elementType = propertyType.GetElementType()!;
		if (arrayLength == 0)
		{
			return Array.CreateInstance(elementType, 0); // Create 0 length array regardless of type
		}

		switch (elementType)
		{
			case Type t when t == typeof(sbyte):
			{
				sbyte[] value = new sbyte[arrayLength];
				ReadSBytes(value);
				return value;
			}
			case Type t when t == typeof(byte):
			{
				byte[] value = new byte[arrayLength];
				ReadBytes(value);
				return value;
			}
			case Type t when t == typeof(short):
			{
				short[] value = new short[arrayLength];
				ReadInt16s(value);
				return value;
			}
			case Type t when t == typeof(ushort):
			{
				ushort[] value = new ushort[arrayLength];
				ReadUInt16s(value);
				return value;
			}
			case Type t when t == typeof(int):
			{
				int[] value = new int[arrayLength];
				ReadInt32s(value);
				return value;
			}
			case Type t when t == typeof(uint):
			{
				uint[] value = new uint[arrayLength];
				ReadUInt32s(value);
				return value;
			}
			case Type t when t == typeof(long):
			{
				long[] value = new long[arrayLength];
				ReadInt64s(value);
				return value;
			}
			case Type t when t == typeof(ulong):
			{
				ulong[] value = new ulong[arrayLength];
				ReadUInt64s(value);
				return value;
			}
			case Type t when t == typeof(Half):
			{
				var value = new Half[arrayLength];
				ReadHalves(value);
				return value;
			}
			case Type t when t == typeof(float):
			{
				float[] value = new float[arrayLength];
				ReadSingles(value);
				return value;
			}
			case Type t when t == typeof(double):
			{
				double[] value = new double[arrayLength];
				ReadDoubles(value);
				return value;
			}
			case Type t when t == typeof(decimal):
			{
				decimal[] value = new decimal[arrayLength];
				ReadDecimals(value);
				return value;
			}
			case Type t when t == typeof(bool):
			{
				BooleanSize old = BooleanSize;
				BooleanSize = Utils.AttributeValueOrDefault<BinaryBooleanSizeAttribute, BooleanSize>(propertyInfo, old);
				bool[] value = new bool[arrayLength];
				ReadBooleans(value);
				BooleanSize = old;
				return value;
			}
			case Type t when t.IsEnum:
			{
				var value = Array.CreateInstance(elementType, arrayLength);
				for (int i = 0; i < arrayLength; i++)
				{
					value.SetValue(ReadEnum(elementType), i);
				}
				return value;
			}
			case Type t when t == typeof(DateTime):
			{
				var value = new DateTime[arrayLength];
				ReadDateTimes(value);
				return value;
			}
			case Type t when t == typeof(DateOnly):
			{
				var value = new DateOnly[arrayLength];
				ReadDateOnlys(value);
				return value;
			}
			case Type t when t == typeof(TimeOnly):
			{
				var value = new TimeOnly[arrayLength];
				ReadTimeOnlys(value);
				return value;
			}
			case Type t when t == typeof(Vector2):
			{
				var value = new Vector2[arrayLength];
				ReadVector2s(value);
				return value;
			}
			case Type t when t == typeof(Vector3):
			{
				var value = new Vector3[arrayLength];
				ReadVector3s(value);
				return value;
			}
			case Type t when t == typeof(Vector4):
			{
				var value = new Vector4[arrayLength];
				ReadVector4s(value);
				return value;
			}
			case Type t when t == typeof(Quaternion):
			{
				var value = new Quaternion[arrayLength];
				ReadQuaternions(value);
				return value;
			}
			case Type t when t == typeof(Matrix4x4):
			{
				var value = new Matrix4x4[arrayLength];
				ReadMatrix4x4s(value);
				return value;
			}
			case Type t when t == typeof(char):
			{
				bool old = ASCII;
				ASCII = Utils.AttributeValueOrDefault<BinaryASCIIAttribute, bool>(propertyInfo, old);
				bool trimNullTerminators = Utils.AttributeValueOrDefault<BinaryStringTrimNullTerminatorsAttribute, bool>(propertyInfo, false);
				char[] value;
				if (trimNullTerminators)
				{
					value = ReadChars_TrimNullTerminators(arrayLength);
				}
				else
				{
					value = new char[arrayLength];
					ReadChars(value);
				}
				ASCII = old;
				return value;
			}
			case Type t when t == typeof(string):
			{
				Utils.GetStringLength(obj, objType, propertyInfo, true, out bool? nullTerminated, out int stringLength);

				bool old = ASCII;
				ASCII = Utils.AttributeValueOrDefault<BinaryASCIIAttribute, bool>(propertyInfo, old);
				string[] value = new string[arrayLength];
				if (nullTerminated == true)
				{
					ReadStrings_NullTerminated(value);
				}
				else
				{
					bool trimNullTerminators = Utils.AttributeValueOrDefault<BinaryStringTrimNullTerminatorsAttribute, bool>(propertyInfo, false);
					if (trimNullTerminators)
					{
						ReadStrings_Count_TrimNullTerminators(value, stringLength);
					}
					else
					{
						ReadStrings_Count(value, stringLength);
					}
				}
				ASCII = old;
				return value;
			}
			default:
			{
				var value = Array.CreateInstance(elementType, arrayLength);
				for (int i = 0; i < arrayLength; i++)
				{
					value.SetValue(ReadObject(elementType), i);
				}
				return value;
			}
		}
	}
}
