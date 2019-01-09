using System.ComponentModel.DataAnnotations;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Library.Validation.Abstractions
{
    public interface IValidator<in T>
    {
        void Validate(IValidationTarget<T> target);
    }
}