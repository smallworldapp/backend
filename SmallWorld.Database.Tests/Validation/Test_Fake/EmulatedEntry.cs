using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SmallWorld.Library.Model;
using SmallWorld.Library.Model.Abstractions;

namespace SmallWorld.Database.Tests.Validation.Test_Fake
{
    public class EmulatedEntry<T> : IEntry<T> where T : class
    {
        private readonly T value;

        public EmulatedEntry(T value)
        {
            this.value = value;
        }

        public EntityState State => EntityState.Modified;

        public IEntry<T> LoadRelations(string expr)
        {
            return this;
        }

        public IEntry<T> LoadRelations<TProperty>(Expression<Func<T, TProperty>> expr)
        {
            return this;
        }

        public IEntry<T> LoadRelations<TCollection, TProperty>(Expression<Func<T, ICollection<TCollection>>> collectionExpr, Expression<Func<TCollection, TProperty>> propExpr = null) where TCollection : class where TProperty : class
        {
            return this;
        }

        public IQueryable<TCollection> QueryRelation<TCollection>(Expression<Func<T, ICollection<TCollection>>> expr)
        {
            var path = PropertyPath<T>.Create(expr);

            var set = path.GetValue(value);
            if (set == null)
                path.TrySetValue(value, set = new HashSet<TCollection>());

            return new EnumerableQuery<TCollection>((IEnumerable<TCollection>)set);
        }
    }
}
