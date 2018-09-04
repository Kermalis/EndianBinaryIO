using System;
using System.Reflection;
using System.Text;

namespace EndianBinaryIO
{
    public abstract class EndianBinaryAttribute : Attribute
    {
        public object Value { get; protected set; }
        
        internal static T ValueOrDefault<T>(MemberInfo field, Type attribute, T defaultValue)
        {
            object[] customAttributes = field.GetCustomAttributes(attribute, true);
            if (customAttributes.Length == 0)
                return defaultValue;
            else
                return (T)((EndianBinaryAttribute)customAttributes[0]).Value;
        }

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
    public sealed class BinaryFixedLengthAttribute : EndianBinaryAttribute
    {
        public BinaryFixedLengthAttribute(int length)
        {
            if (length <= 0)
                throw new ArgumentException("Length must be greater than 0.");
            Value = length;
        }

        internal override void DoNotInheritOutsideOfThisAssembly() { }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class BinaryStringEncodingAttribute : EndianBinaryAttribute
    {
        public BinaryStringEncodingAttribute(EncodingType type = EncodingType.ASCII) => Value = type;

        internal override void DoNotInheritOutsideOfThisAssembly() { }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class BinaryStringNullTerminatedAttribute : EndianBinaryAttribute
    {
        public BinaryStringNullTerminatedAttribute(bool nullTerminated = true) => Value = nullTerminated;

        internal override void DoNotInheritOutsideOfThisAssembly() { }
    }
}
