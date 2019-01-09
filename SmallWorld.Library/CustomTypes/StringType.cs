using System;

namespace SmallWorld.Library.CustomTypes
{
    public abstract class StringType
    {
        public string Value { get; }

        protected StringType(string value)
        {
            Value = value;
        }

        public int Length => Value.Length;

        public static bool operator ==(StringType a, StringType b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (ReferenceEquals(a, null))
                return ReferenceEquals(b.Value, null);

            if (ReferenceEquals(b, null))
                return ReferenceEquals(a.Value, null);

            return a.Equals(b);
        }

        public static bool operator !=(StringType a, StringType b) => !(a == b);

        public override string ToString() => Value;

        public override bool Equals(object obj) => obj is StringType str && Value == str.Value;
        public override int GetHashCode()
        {
            return Value == null ? 0 : Value.GetHashCode();
        }
    }

    public abstract class StringType<T> : StringType, IEquatable<T>, IComparable<T> where T : StringType<T>
    {
        private readonly StringComparison comparison;

        protected StringType(string value, StringComparison comparison) : base(value)
        {
            this.comparison = comparison;
        }

        public T Trim() => Create(Value.Trim(), comparison);
        public T Trim(char c) => Create(Value.Trim(c), comparison);

        public override int GetHashCode() => base.GetHashCode();
        public override bool Equals(object obj)
        {
            return obj is T t && Equals(t);
        }

        public bool Equals(T other)
        {
            if (other == null)
                return Value == null;

            return string.Equals(Value, other.Value, comparison);
        }

        public int CompareTo(T other)
        {
            if (other == null)
                return string.Compare(Value, null, comparison);

            return string.Compare(Value, other.Value, comparison);
        }

        protected abstract T Create(string value, StringComparison comparison);
    }
}