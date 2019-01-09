using System;
using System.Collections.Generic;
using System.Text;

namespace SmallWorld.Library.Validation.Abstractions
{
    public interface IValidationResult
    {
        bool HasErrors { get; }
    }
}
