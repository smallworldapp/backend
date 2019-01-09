namespace SmallWorld.Library.Model
{
    public class Optional<T>
    {
        public bool HasValue { get; }
        private readonly T value;

        public Optional(bool present, T value)
        {
            HasValue = present;
            this.value = value;
        }

        public bool Exists(out T val)
        {
            val = value;
            return HasValue;
        }
    }
}