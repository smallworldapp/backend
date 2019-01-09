using System;
using System.Linq;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model;

namespace SmallWorld.Database.Model.Impl
{
    public class MemberRepository : BaseEntityRepository<Member, IMemberRepository>, IMemberRepository
    {
        public MemberRepository(IServiceProvider provider) : this(provider, null) { }
        public MemberRepository(IServiceProvider provider, IQueryable<Member> src) : base(provider, src) { }

        public bool Find(EmailAddress email, out Member member) => Find(email).Exists(out member);
        public Optional<Member> Find(EmailAddress email) => Find(m => m.Email == email);

        protected override IMemberRepository Create(IQueryable<Member> chain) => new MemberRepository(Provider, chain);
    }
}
