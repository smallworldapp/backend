using System;
using SmallWorld.Library.Validation.Abstractions;

namespace SmallWorld.Library.Validation
{
    public class ValidationException : Exception
    {
        public IValidationResult Result { get; }

        public ValidationException(IValidationResult result) : base(result.ToString())
        {
            Result = result;
        }
    }
}
