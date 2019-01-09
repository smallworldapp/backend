using System;
using System.Linq;
using System.Linq.Expressions;
using SmallWorld.Database.Entities;
using SmallWorld.Library.Model;
using SmallWorld.Library.Model.Abstractions;

namespace SmallWorld.Database.Model.Abstractions
{
    public interface IBaseEntityQueryable<T, out TQueryable> where T : BaseEntity where TQueryable : IBaseEntityQueryable<T, TQueryable>
    {
        IQueryable<T> All { get; }

        void Add(T value);

        bool Find(Guid id, out T value);
        Optional<T> Find(Guid id);

        bool Find(Func<T, bool> filter, out T value);
        Optional<T> Find(Func<T, bool> filter);

        TQueryable Include<TProperty>(Expression<Func<T, TProperty>> expr);

        IEntry<T> Entry(T value);
    }

    public interface IBaseEntityRepository<T, TRepo> : IBaseEntityQueryable<T, TRepo> where T : BaseEntity where TRepo : IBaseEntityRepository<T, TRepo>
    {
        void Update(T value);
    }
}