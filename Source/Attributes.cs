using System;

namespace Kermalis.EndianBinaryIO
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BinaryIgnoreAttribute : Attribute
    {
        public bool Value { get; }

        public BinaryIgnoreAttribute(bool ignore = true)
        {
            Value = ignore;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BinaryBooleanSizeAttribute : Attribute
    {
        public BooleanSize Value { get; }

        public BinaryBooleanSizeAttribute(BooleanSize booleanSize = BooleanSize.U8)
        {
            Value = booleanSize;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BinaryEncodingAttribute : Attribute
    {
        public EncodingType Value { get; }

        public BinaryEncodingAttribute(EncodingType encodingType = EncodingType.ASCII)
        {
            Value = encodingType;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BinaryStringNullTerminatedAttribute : Attribute
    {
        public bool Value { get; }

        public BinaryStringNullTerminatedAttribute(bool nullTerminated = true)
        {
            Value = nullTerminated;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BinaryArrayFixedLengthAttribute : Attribute
    {
        public int Value { get; }

        public BinaryArrayFixedLengthAttribute(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            Value = length;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BinaryArrayVariableLengthAttribute : Attribute
    {
        public string Value { get; }

        public BinaryArrayVariableLengthAttribute(string anchor)
        {
            Value = anchor;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BinaryStringFixedLengthAttribute : Attribute
    {
        public int Value { get; }

        public BinaryStringFixedLengthAttribute(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            Value = length;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BinaryStringVariableLengthAttribute : Attribute
    {
        public string Value { get; }

        public BinaryStringVariableLengthAttribute(string anchor)
        {
            Value = anchor;
        }
    }
}
