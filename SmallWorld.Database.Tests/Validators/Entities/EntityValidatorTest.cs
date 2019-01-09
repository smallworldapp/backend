using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using SmallWorld.Database.Validators.Entities;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;
using Xunit;

namespace SmallWorld.Database.Tests.Validators.Entities
{
    public class EntityValidatorTest : TestBase
    {
        private class FakeEntity : BaseEntity { }

        private class FakeEntry<T> : IEntry<T> where T : class
        {
            public EntityState State { get; }

            public FakeEntry(EntityState state)
            {
                State = state;
            }

            public IEntry<T> LoadRelations(string expr)
            {
                throw new NotImplementedException();
            }

            public IEntry<T> LoadRelations<TProperty>(Expression<Func<T, TProperty>> expr)
            {
                throw new NotImplementedException();
            }

            public IEntry<T> LoadRelations<TCollection, TProperty>(Expression<Func<T, ICollection<TCollection>>> collectionExpr, Expression<Func<TCollection, TProperty>> propExpr = null) where TCollection : class where TProperty : class
            {
                throw new NotImplementedException();
            }

            public IQueryable<TCollection> QueryRelation<TCollection>(Expression<Func<T, ICollection<TCollection>>> expr)
            {
                throw new NotImplementedException();
            }
        }

        private class FakeEntryRepository : IEntryRepository
        {
            public EntityState State { get; set; } = EntityState.Unchanged;

            public IEntry<T> Entry<T>(T t) where T : class
            {
                return new FakeEntry<T>(State);
            }
        }

        protected override void AddServices(IServiceCollection services)
        {
            services.AddValidation();

            services.AddSingleton<FakeEntryRepository>();
            services.AddSingleton<IEntryRepository>(p => p.GetService<FakeEntryRepository>());

            services.AddSingleton<IValidator<BaseEntity>, EntityValidator>();
        }

        [Theory]
        [InlineData(false, EntityState.Unchanged)]
        [InlineData(true, EntityState.Deleted)]
        [InlineData(true, EntityState.Detached)]
        [InlineData(true, EntityState.Modified)]
        [InlineData(true, EntityState.Added)]
        public async Task Validate(bool shouldContinue, EntityState state)
        {
            using (var provider = await CreateProvider())
            {
                var validator = provider.GetRequiredService<IValidator<BaseEntity>>();
                var entries = provider.GetRequiredService<FakeEntryRepository>();
                var entity = new FakeEntity();
                entries.State = state;

                var target = new ValidationTarget<BaseEntity>(entity);
                validator.Validate(target);
                Assert.Equal(shouldContinue, target.Continue);
            }
        }
    }
}
