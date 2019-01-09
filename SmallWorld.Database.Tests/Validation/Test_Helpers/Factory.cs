using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using SmallWorld.Database.Entities;
using Xunit;

namespace SmallWorld.Database.Tests.Validation.Test_Helpers
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public abstract class Factory<T> : ISource<T> where T : BaseEntity, new()
    {
        private readonly List<(PropertyInfo property, ISource source)> fields = new List<(PropertyInfo, ISource)>();

        protected Factory()
        {
            var fieldInfos = GetType().GetFields();
            foreach (var field in fieldInfos)
            {
                var prop = typeof(T).GetProperty(field.Name);
                Assert.NotNull(prop);

                var value = field.GetValue(this);
                Assert.NotNull(value);

                fields.Add((prop, (ISource)value));
            }
        }

        IEnumerable ISource.Valid() => Valid();
        IEnumerable ISource.Invalid() => Invalid();

        protected virtual IEnumerable<T> ValidBase() { yield return new T().Init(); }
        protected virtual IEnumerable<T> InvalidBase() { yield return new T(); }

        public IEnumerable<T> Valid()
        {
            foreach (var target in ValidBase())
                foreach (var value in GetValidValues(target, 0))
                    yield return value;
        }

        public IEnumerable<T> Invalid()
        {
            foreach (var target in InvalidBase())
                foreach (var value in GetInvalidValues(target, 0))
                    yield return value;
        }

        private IEnumerable<T> GetValidValues(T target, int start)
        {
            var field = fields[start];
            foreach (var value in field.source.Valid())
            {
                field.property.SetValue(target, value);

                if (start + 1 == fields.Count)
                {
                    yield return target;
                    yield break;
                }

                foreach (var recur in GetValidValues(target, start + 1))
                    yield return recur;
            }
        }

        private IEnumerable<T> GetInvalidValues(T target, int start)
        {
            var field = fields[start];
            foreach (var value in field.source.Invalid())
            {
                field.property.SetValue(target, value);

                if (start + 1 == fields.Count)
                {
                    yield return target;
                    yield break;
                }

                foreach (var recur in GetInvalidValues(target, start + 1))
                    yield return recur;
            }
        }
    }
}