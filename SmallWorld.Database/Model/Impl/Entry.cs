using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmallWorld.Library.Model;
using SmallWorld.Library.Model.Abstractions;

namespace SmallWorld.Database.Model.Impl
{
    public class Entry<TEntity> : IEntry<TEntity> where TEntity : class
    {
        private readonly EntityEntry<TEntity> entity;

        public Entry(EntityEntry<TEntity> entity)
        {
            this.entity = entity;
        }

        public EntityState State
        {
            get
            {
                switch (entity.State)
                {
                    case Microsoft.EntityFrameworkCore.EntityState.Detached:
                        return EntityState.Detached;
                    case Microsoft.EntityFrameworkCore.EntityState.Unchanged:
                        return EntityState.Unchanged;
                    case Microsoft.EntityFrameworkCore.EntityState.Deleted:
                        return EntityState.Deleted;
                    case Microsoft.EntityFrameworkCore.EntityState.Modified:
                        return EntityState.Modified;
                    case Microsoft.EntityFrameworkCore.EntityState.Added:
                        return EntityState.Added;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public IEntry<TEntity> LoadRelations(string expr)
        {
            var path = PropertyPath<TEntity>.Parse(expr);
            LoadRelations(path);
            return this;
        }

        public IEntry<TEntity> LoadRelations<TProperty>(Expression<Func<TEntity, TProperty>> expr)
        {
            var path = PropertyPath<TEntity>.Create(expr);
            LoadRelations(path);
            return this;
        }

        private void LoadRelations(PropertyPath<TEntity> path)
        {
            EntityEntry node = entity;

            foreach (var prop in path.Properties)
            {
                var isCollection = typeof(IEnumerable).IsAssignableFrom(prop.PropertyType);

                if (isCollection && !ReferenceEquals(prop, path.Properties.Last()))
                    throw new ArgumentException("Invalid path: " + path);

                if (isCollection)
                    node.Collection(prop.Name).Load();
                else
                {
                    var refEntity = node.Reference(prop.Name);
                    refEntity.Load();

                    if (refEntity.CurrentValue == null)
                        return;

                    node = entity.Context.Entry(refEntity.CurrentValue);
                }
            }
        }

        public IEntry<TEntity> LoadRelations<TCollection, TProperty>(
            Expression<Func<TEntity, ICollection<TCollection>>> collectionExpr,
            Expression<Func<TCollection, TProperty>> propExpr = null
        )
            where TProperty : class
            where TCollection : class
        {
            var path = PropertyPath<TEntity>.Create(collectionExpr);

            EntityEntry node = entity;

            foreach (var prop in path.Properties.Take(path.Properties.Count - 1))
            {
                if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
                    throw new ArgumentException("Invalid path: " + collectionExpr);

                var refEntity = node.Reference(prop.Name);
                refEntity.Load();

                if (refEntity.CurrentValue == null)
                    return this;

                node = entity.Context.Entry(refEntity.CurrentValue);
            }

            node.Collection(path.Properties.Last().Name).Load();

            if (propExpr != null)
            {
                var collection = (ICollection<TCollection>)path.Properties.Last().GetValue(node.Entity);

                foreach (var item in collection)
                {
                    var entry = new Entry<TCollection>(entity.Context.Entry(item));
                    entry.LoadRelations(propExpr);
                }
            }

            return this;
        }

        public IQueryable<TCollection> QueryRelation<TCollection>(Expression<Func<TEntity, ICollection<TCollection>>> expr)
        {
            var path = PropertyPath<TEntity>.Create(expr);
            EntityEntry node = entity;

            foreach (var prop in path.Properties.Take(path.Properties.Count - 1))
            {
                var isCollection = typeof(IEnumerable).IsAssignableFrom(prop.PropertyType);

                if (isCollection && !ReferenceEquals(prop, path.Properties.Last()))
                    throw new ArgumentException("Invalid path: " + expr);

                var refEntity = node.Reference(prop.Name);
                refEntity.Load();

                if (refEntity.CurrentValue == null)
                    throw new NullReferenceException();

                node = entity.Context.Entry(refEntity.CurrentValue);
            }

            return (IQueryable<TCollection>)node.Collection(path.Properties.Last().Name).Query();
        }
    }
}