using System;

namespace SmallWorld.Library.Validation.Abstractions
{
    public interface IValidationProvider
    {
        IServiceProvider ServiceProvider { get; }

        IValidationResult Validate<T>(T value);
        IValidationResult Validate<T, TChild>(IValidationTarget<T> context, TChild value);
        IValidationResult Validate<T, TChild>(IValidationTarget<T> context, string name, TChild value);
    }
}
