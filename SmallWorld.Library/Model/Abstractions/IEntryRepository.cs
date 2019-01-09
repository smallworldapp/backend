namespace SmallWorld.Library.Model.Abstractions
{
    public interface IEntryRepository
    {
        IEntry<T> Entry<T>(T t) where T : class;
    }
}
