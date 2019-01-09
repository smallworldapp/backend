namespace SmallWorld.Library.Validation.Abstractions
{
    public interface IValidationTarget
    {
        bool Continue { get; set; }

        void AddError(string message);
        void AddError(string name, string message);

        IValidationTarget<TChild> Child<TChild>(TChild childValue);
        IValidationTarget<TChild> Child<TChild>(string name, TChild childValue);

        IValidationResult GetResult();
    }

    public interface IValidationTarget<out T> : IValidationTarget
    {
        T Value { get; }
    }
}
