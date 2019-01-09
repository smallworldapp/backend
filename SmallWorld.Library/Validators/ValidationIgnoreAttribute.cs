using System;

namespace SmallWorld.Library.Validators {
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidationIgnoreAttribute : Attribute { }
}