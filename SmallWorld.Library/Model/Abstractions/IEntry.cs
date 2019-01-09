using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SmallWorld.Library.Model.Abstractions
{
    public enum EntityState
    {
        //
        // Summary:
        //     The entity is not being tracked by the context.
        Detached = 0,
        //
        // Summary:
        //     The entity is being tracked by the context and exists in the database. Its property
        //     values have not changed from the values in the database.
        Unchanged = 1,
        //
        // Summary:
        //     The entity is being tracked by the context and exists in the database. It has
        //     been marked for deletion from the database.
        Deleted = 2,
        //
        // Summary:
        //     The entity is being tracked by the context and exists in the database. Some or
        //     all of its property values have been modified.
        Modified = 3,
        //
        // Summary:
        //     The entity is being tracked by the context but does not yet exist in the database.
        Added = 4
    }

    public interface IEntry<TEntity> where TEntity : class
    {
        EntityState State { get; }

        IEntry<TEntity> LoadRelations(string expr);

        IEntry<TEntity> LoadRelations<TProperty>(
            Expression<Func<TEntity, TProperty>> expr);

        IEntry<TEntity> LoadRelations<TCollection, TProperty>(
            Expression<Func<TEntity, ICollection<TCollection>>> collectionExpr,
            Expression<Func<TCollection, TProperty>> propExpr = null)
            where TCollection : class
            where TProperty : class;

        IQueryable<TCollection> QueryRelation<TCollection>(
            Expression<Func<TEntity, ICollection<TCollection>>> expr);
    }
}