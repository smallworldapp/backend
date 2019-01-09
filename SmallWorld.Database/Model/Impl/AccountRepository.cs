using System;
using System.Linq;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model;

namespace SmallWorld.Database.Model.Impl
{
    public class AccountRepository : BaseEntityRepository<Account, IAccountRepository>, IAccountRepository
    {
        public IQueryable<Account> NotDeleted => All
            .Where(a => a.Status != AccountStatus.Deleted);

        public AccountRepository(IServiceProvider provider) : this(provider, null) { }
        public AccountRepository(IServiceProvider provider, IQueryable<Account> src) : base(provider, src) { }

        public override Optional<Account> Find(Func<Account, bool> filter) => Find(filter, false);

        public bool Find(Guid id, bool includeDeleted, out Account acc) => Find(id, includeDeleted).Exists(out acc);
        public Optional<Account> Find(Guid id, bool includeDeleted) => Find(a => a.Guid == id, includeDeleted);

        public bool Find(Func<Account, bool> filter, bool includeDeleted, out Account acc) => Find(filter, includeDeleted).Exists(out acc);
        public Optional<Account> Find(Func<Account, bool> filter, bool includeDeleted)
        {
            var src = includeDeleted ? All : NotDeleted;

            var value = src.SingleOrDefault(filter);
            return new Optional<Account>(value != null, value);
        }

        public bool Find(EmailAddress email, out Account acc) => Find(email).Exists(out acc);
        public Optional<Account> Find(EmailAddress email) => Find(a => a.Email == email);

        protected override IAccountRepository Create(IQueryable<Account> chain) => new AccountRepository(Provider, chain);
    }
}