using System;
using System.Linq;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;

namespace SmallWorld.Database.Model.Impl
{
    public class EmailRepository : BaseEntityRepository<Email, IEmailRepository>, IEmailRepository
    {
        public EmailRepository(IServiceProvider provider) : this(provider, null) { }
        public EmailRepository(IServiceProvider provider, IQueryable<Email> src) : base(provider, src) { }

        protected override IEmailRepository Create(IQueryable<Email> chain)
        {
            return new EmailRepository(Provider, chain);
        }
    }
}
