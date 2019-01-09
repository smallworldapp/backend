using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Model.Impl
{
    public abstract class BaseEntityRepository<T, TRepo> : IBaseEntityRepository<T, TRepo> where T : BaseEntity where TRepo : IBaseEntityRepository<T, TRepo>
    {
        protected IServiceProvider Provider { get; }
        protected IContext Context { get; }

        private readonly IValidationProvider validation;

        private IQueryable<T> src;

        public IQueryable<T> All => src ?? (src = Context.Set<T>());

        protected BaseEntityRepository(IServiceProvider provider, IQueryable<T> chain = null)
        {
            Provider = provider;
            Context = provider.GetRequiredService<IContext>();
            validation = provider.GetService<IValidationProvider>();

            src = chain;
        }

        public virtual void Add(T value)
        {
            if (value.Guid != Guid.Empty)
                throw new ValidationException(new ValidationResult("Defined invalid Guid"));

            value.CreateIds();

            Context.Add(value);

            var result = Validate(value);
            if (result.HasErrors)
                throw new ValidationException(result);
        }

        public void Update(T value)
        {
            if (value.Guid == Guid.Empty)
                throw new ValidationException(new ValidationResult("Defined invalid Guid"));

            Context.Update(value);

            var result = Validate(value);
            if (result.HasErrors)
                    throw new ValidationException(result);
        }

        private IValidationResult Validate(T value)
        {
            if (Find(value.Guid, out var same) && same != value)
                return new ValidationResult("Duplicate Guid");

            if (validation != null)
                return validation.Validate(value);

            return ValidationResult.Success;
        }

        public virtual IEntry<T> Entry(T value)
        {
            return Context.Entry(value);
        }

        public virtual bool Find(Guid id, out T value) => Find(id).Exists(out value);
        public virtual Optional<T> Find(Guid id) => Find(a => a.Guid == id);

        public virtual bool Find(Func<T, bool> filter, out T value) => Find(filter).Exists(out value);
        public virtual Optional<T> Find(Func<T, bool> filter)
        {
            var value = All.SingleOrDefault(filter);
            return new Optional<T>(value != null, value);
        }

        public virtual TRepo Include<TProperty>(Expression<Func<T, TProperty>> expr)
        {
            return Create(All.Include(expr));
        }

        protected abstract TRepo Create(IQueryable<T> chain);
    }
}