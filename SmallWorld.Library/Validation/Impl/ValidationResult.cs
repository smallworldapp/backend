using System;
using System.Collections.Generic;
using System.Linq;
using SmallWorld.Library.Validation.Abstractions;

namespace SmallWorld.Library.Validation.Impl
{
    public class ValidationResult : IValidationResult, IEquatable<ValidationResult>
    {
        public bool HasErrors { get; }
        public int ErrorCount { get; }

        private readonly List<string> errors;

        public ValidationResult(params string[] errors) : this(errors.AsEnumerable()) { }

        public ValidationResult(IEnumerable<string> errors)
        {
            this.errors = errors.ToList();

            ErrorCount = this.errors.Count;
            HasErrors = ErrorCount != 0;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, errors);
        }

        public static IValidationResult Success { get; } = new ValidationResult();

        public bool Equals(ValidationResult other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            // Two success results are equal
            return ErrorCount == 0 && other.ErrorCount == 0;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != GetType())
                return false;

            return Equals((ValidationResult)obj);
        }

        public override int GetHashCode() => ErrorCount.GetHashCode();
    }
}
