using SmallWorld.Database.Entities;
using SmallWorld.Library.Model;

namespace SmallWorld.Database.Model.Abstractions
{
    public interface IMemberRepository : IBaseEntityRepository<Member, IMemberRepository>
    {
        bool Find(EmailAddress email, out Member member);
        Optional<Member> Find(EmailAddress email);
    }
}
