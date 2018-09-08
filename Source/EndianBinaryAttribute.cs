using System;

namespace Kermalis.EndianBinaryIO
{
    public abstract class EndianBinaryAttribute : Attribute
    {
        public object Value { get; protected set; }

        // Prevent external inheritance
        internal virtual void DoNotInheritOutsideOfThisAssembly()
        {
            throw new Exception("Do not inherit EndianBinaryAttribute.");
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class BinaryIgnoreAttribute : EndianBinaryAttribute
    {
        public BinaryIgnoreAttribute(bool ignore = true) => Value = ignore;

        internal override void DoNotInheritOutsideOfThisAssembly() { }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class BinaryBooleanSizeAttribute : EndianBinaryAttribute
    {
        public BinaryBooleanSizeAttribute(BooleanSize size = BooleanSize.U8) => Value = size;

        internal override void DoNotInheritOutsideOfThisAssembly() { }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class BinaryEncodingAttribute : EndianBinaryAttribute
    {
        public BinaryEncodingAttribute(EncodingType type = EncodingType.ASCII) => Value = type;

        internal override void DoNotInheritOutsideOfThisAssembly() { }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class BinaryStringNullTerminatedAttribute : EndianBinaryAttribute
    {
        public BinaryStringNullTerminatedAttribute(bool nullTerminated = true) => Value = nullTerminated;

        internal override void DoNotInheritOutsideOfThisAssembly() { }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class BinaryArrayFixedLengthAttribute : EndianBinaryAttribute
    {
        public BinaryArrayFixedLengthAttribute(int length)
        {
            if (length <= 0)
                throw new ArgumentException("BinaryArrayFixedLengthAttribute must be greater than 0.");
            Value = length;
        }

        internal override void DoNotInheritOutsideOfThisAssembly() { }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class BinaryArrayVariableLengthAttribute : EndianBinaryAttribute
    {
        public BinaryArrayVariableLengthAttribute(string anchor) => Value = anchor;

        internal override void DoNotInheritOutsideOfThisAssembly() { }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class BinaryStringFixedLengthAttribute : EndianBinaryAttribute
    {
        public BinaryStringFixedLengthAttribute(int length)
        {
            if (length <= 0)
                throw new ArgumentException("BinaryStringFixedLengthAttribute must be greater than 0.");
            Value = length;
        }

        internal override void DoNotInheritOutsideOfThisAssembly() { }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class BinaryStringVariableLengthAttribute : EndianBinaryAttribute
    {
        public BinaryStringVariableLengthAttribute(string anchor) => Value = anchor;

        internal override void DoNotInheritOutsideOfThisAssembly() { }
    }
}
