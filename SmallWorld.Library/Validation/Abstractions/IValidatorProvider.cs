using System;
using System.Collections.Generic;

namespace SmallWorld.Library.Validation.Abstractions
{
    public interface IValidatorProvider
    {
//        bool HasValidator(Type targetType);

        IEnumerable<Type> GetValidators(Type targetType);

//        object CreateValidator(IServiceProvider provider, Type targetType);
    }
}
