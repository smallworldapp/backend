using System;
using System.Linq;
using SmallWorld.Database.Entities;
using SmallWorld.Library.Model;

namespace SmallWorld.Database.Model.Abstractions
{
    public interface IAccountRepository : IBaseEntityRepository<Account, IAccountRepository>
    {
        IQueryable<Account> NotDeleted { get; }

        bool Find(Guid id, bool includeDeleted, out Account acc);
        Optional<Account> Find(Guid id, bool includeDeleted);

        bool Find(Func<Account, bool> filter, bool includeDeleted, out Account acc);
        Optional<Account> Find(Func<Account, bool> filter, bool includeDeleted);

        bool Find(EmailAddress email, out Account acc);
        Optional<Account> Find(EmailAddress email);
    }
}
