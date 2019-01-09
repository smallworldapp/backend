using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SmallWorld.Database.Tests
{
    public class MemberData : IEnumerable<object[]>
    {
        private readonly IList<object[]> contents = new List<object[]>();

        public void Add(params object[] args)
        {
            contents.Add(args);
        }

        IEnumerator IEnumerable.GetEnumerator() => contents.GetEnumerator();
        public IEnumerator<object[]> GetEnumerator() => contents.Select(value => value).GetEnumerator();
    }


    public class MemberData<T> : IEnumerable<object[]>
    {
        private readonly List<T> contents = new List<T>();

        public void Add(IEnumerable<T> src)
        {
            contents.AddRange(src);
        }

        public void Add(params T[] value)
        {
            if (value == null)
            {
                if (typeof(T).IsValueType) throw new ArgumentException();
                contents.Add(default(T));
            }
            else
                Add((IEnumerable<T>)value);
        }

        public MemberData<T2> Cast<T2>(Func<T, T2> mapper)
        {
            var mapped = new MemberData<T2>();

            foreach (var from in Values)
            {
                mapped.Add(mapper(from));
            }

            return mapped;
        }

        public IEnumerable<T> Values => contents;
        public T Value => contents.First();

        IEnumerator IEnumerable.GetEnumerator() => contents.GetEnumerator();
        public IEnumerator<object[]> GetEnumerator() => contents.Select(value => new object[] { value }).GetEnumerator();
    }
}
