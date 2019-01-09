using System;

namespace SmallWorld.Library.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValidationAttribute : Attribute
    {
        public abstract Type Validator { get; }
    }
}
