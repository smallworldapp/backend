using System.Collections.Generic;
using SmallWorld.Library.Validation.Abstractions;

namespace SmallWorld.Library.Validation.Impl
{
    public class ValidationTarget<T> : IValidationTarget<T>
    {
        public IValidationTarget Parent { get; }

        public T Value { get; }
        public string Name { get; }
        public bool Continue { get; set; } = true;

        private readonly List<string> errors = new List<string>();

        public ValidationTarget(T value)
        {
            Value = value;
            Name = typeof(T).Name;
        }

        private ValidationTarget(IValidationTarget parent, string name, T value)
        {
            Parent = parent;
            Name = name;
            Value = value;
        }

        public void AddError(string message)
        {
            AddError("", message);
        }

        public void AddError(string name, string message)
        {
            errors.Add(Name + name + ": " + message);
            Parent?.AddError(Name + name, message);
        }

        public IValidationTarget<TChild> Child<TChild>(TChild childValue)
        {
            return new ValidationTarget<TChild>(this, "[]", childValue);
        }

        public IValidationTarget<TChild> Child<TChild>(string name, TChild childValue)
        {
            return new ValidationTarget<TChild>(this, "." + name, childValue);
        }

        public IValidationResult GetResult()
        {
            return new ValidationResult(errors);
        }
    }
}
