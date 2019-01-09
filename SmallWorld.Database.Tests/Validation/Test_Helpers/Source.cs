using System.Collections;
using System.Collections.Generic;

namespace SmallWorld.Database.Tests.Validation.Test_Helpers
{
    public interface ISource
    {
        IEnumerable Valid();
        IEnumerable Invalid();
    }

    public interface ISource<out T> : ISource
    {
        new IEnumerable<T> Valid();
        new IEnumerable<T> Invalid();
    }

    public class Source
    {
        public static Source<T> Simple<T>(T valid, T invalid)
        {
            return new Source<T>(new[] { valid }, new[] { default(T) });
        }

        public static Source<T> OrDefault<T>(T valid)
        {
            return new Source<T>(new[] { valid }, new[] { default(T) });
        }
//
//        public static Source<T> OrDefault<T>(params T[] valid)
//        {
//            return new Source<T>(valid, new[] { default(T) });
//        }
    }

    public class Source<T> : ISource<T>
    {
        public delegate IEnumerable<T> Func();

        private readonly IEnumerable<T> valid;
        private readonly IEnumerable<T> invalid;

        public Source(Func valid, Func invalid) : this(valid(), invalid()) { }
        public Source(MemberData<T> valid, MemberData<T> invalid) : this(valid.Values, invalid.Values) { }

        public Source(IEnumerable<T> valid, IEnumerable<T> invalid)
        {
            this.valid = valid;
            this.invalid = invalid;
        }

        IEnumerable ISource.Valid() => valid;
        IEnumerable ISource.Invalid() => invalid;

        public IEnumerable<T> Valid() => valid;
        public IEnumerable<T> Invalid() => invalid;
    }

    //    public class Factory<T> : ISource
    //    {
    //        private readonly Delegate create;
    //        private readonly List<ISource> sources;
    //
    //        protected Factory(Delegate create, params ISource[] sources)
    //        {
    //            this.create = create;
    //            this.sources = sources.ToList();
    //        }
    //
    //        IEnumerable ISource.Valid() => Valid();
    //        IEnumerable ISource.Invalid() => Invalid();
    //
    //        public IEnumerable<T> Valid()
    //        {
    //            var args = new object[sources.Count];
    //            foreach (var set in GetValidArguments(args, 0))
    //                yield return (T)create.DynamicInvoke(set);
    //        }
    //
    //
    //        public IEnumerable<T> Invalid()
    //        {
    //            var args = new object[sources.Count];
    //            foreach (var set in GetInvalidArguments(args, 0))
    //                yield return (T)create.DynamicInvoke(set);
    //        }
    //
    //        private IEnumerable<object[]> GetValidArguments(object[] args, int start)
    //        {
    //            foreach (var value in sources[start].Valid())
    //            {
    //                args[start] = value;
    //
    //                if (start + 1 == sources.Count)
    //                    yield return args;
    //
    //                else
    //                    foreach (var set in GetValidArguments(args, start + 1))
    //                        yield return set;
    //            }
    //        }
    //
    //        private IEnumerable<object[]> GetInvalidArguments(object[] args, int start)
    //        {
    //            foreach (var value in sources[start].Invalid())
    //            {
    //                args[start] = value;
    //
    //                if (start + 1 == sources.Count)
    //                    yield return args;
    //
    //                else
    //                    foreach (var set in GetInvalidArguments(args, start + 1))
    //                        yield return set;
    //            }
    //        }
    //    }
    //
    //    public class Factory<T, T1> : Factory<T>
    //    {
    //        public Factory(Func<T1, T> create, Source<T1> a) : base(create, a) { }
    //    }
    //
    //    public class Factory<T, T1, T2> : Factory<T>
    //    {
    //        public Factory(Func<T1, T2, T> create, Source<T1> a, Source<T2> b) : base(create, a, b) { }
    //    }
    //
    //    public class Factory<T, T1, T2, T3> : Factory<T>
    //    {
    //        public Factory(Func<T1, T2, T3, T> create, Source<T1> a, Source<T2> b, Source<T3> c) : base(create, a, b, c) { }
    //    }
    //
    //    public class Factory<T, T1, T2, T3, T4> : Factory<T>
    //    {
    //        public Factory(Func<T1, T2, T3, T4, T> create, Source<T1> a, Source<T2> b, Source<T3> c, Source<T4> d) : base(create, a, b, c, d) { }
    //    }
    //
    //    public class Factory<T, T1, T2, T3, T4, T5> : Factory<T>
    //    {
    //        public Factory(Func<T1, T2, T3, T4, T5, T> create, Source<T1> a, Source<T2> b, Source<T3> c, Source<T4> d, Source<T5> e) : base(create, a, b, c, d, e) { }
    //    }
    //
    //    public class Factory<T, T1, T2, T3, T4, T5, T6> : Factory<T>
    //    {
    //        public Factory(Func<T1, T2, T3, T4, T5, T6, T> create, Source<T1> a, Source<T2> b, Source<T3> c, Source<T4> d, Source<T5> e, Source<T6> f) : base(create, a, b, c, d, e, f) { }
    //    }
    //
    //    public class Factory<T, T1, T2, T3, T4, T5, T6, T7> : Factory<T>
    //    {
    //        public Factory(Func<T1, T2, T3, T4, T5, T6, T7, T> create, Source<T1> a, Source<T2> b, Source<T3> c, Source<T4> d, Source<T5> e, Source<T6> f, Source<T7> g) : base(create, a, b, c, d, e, f, g) { }
    //    }
    //
    //    public class Factory<T, T1, T2, T3, T4, T5, T6, T7, T8> : Factory<T>
    //    {
    //        public Factory(Func<T1, T2, T3, T4, T5, T6, T7, T8, T> create, Source<T1> a, Source<T2> b, Source<T3> c, Source<T4> d, Source<T5> e, Source<T6> f, Source<T7> g, Source<T8> h) : base(create, a, b, c, d, e, f, g, h) { }
    //    }
}
