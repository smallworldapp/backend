using SmallWorld.Library.Validation.Abstractions;

namespace SmallWorld.Library.Validation.Impl
{
    public abstract class Validator<T> : IValidator<T>
    {
        void IValidator<T>.Validate(IValidationTarget<T> target)
        {
            Validate(target);
        }

        protected abstract bool Validate(IValidationTarget<T> target);
    }

    public static class ValidatorExtensions
    {
        public static bool Error<T>(this IValidationTarget<T> target, string message)
        {
            target.AddError(message);
            return true;
        }
    }
}
