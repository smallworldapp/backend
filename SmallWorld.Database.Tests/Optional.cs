using System;

namespace SmallWorld.Database.Tests
{
    public struct Optional<T>
    {
        public T Value { get; }
        public bool HasValue { get; }

        public Optional(bool present, T value)
        {
            HasValue = present;
            Value = value;
        }

        public T Or(T or) => HasValue ? Value : or;
        public T Or(Func<T> or) => HasValue ? Value : or();

        public static implicit operator T(Optional<T> src) => src.Value;
        public static implicit operator Optional<T>(T src) => new Optional<T>(true, src);
    }
}
