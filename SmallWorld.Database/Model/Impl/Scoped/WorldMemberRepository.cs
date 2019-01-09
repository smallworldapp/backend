using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;

namespace SmallWorld.Database.Model.Impl.Scoped
{
    public class WorldMemberRepository : MemberRepository
    {
        public World World { get; }

        public WorldMemberRepository(IServiceProvider provider, World world, IQueryable<Member> src = null) : base(provider, src ?? GetQueryable(provider, world))
        {
            World = world;

            Context.Entry(world).LoadRelations(w => w.Members);
        }

        public override void Add(Member member)
        {
            member.World = World;
            World.Members.Add(member);

            base.Add(member);
        }

        protected override IMemberRepository Create(IQueryable<Member> chain) => new WorldMemberRepository(Provider, World, chain);

        private static IQueryable<Member> GetQueryable(IServiceProvider provider, World world)
        {
            var context = provider.GetRequiredService<IEntryRepository>();
            var entry = context.Entry(world);
            return entry.QueryRelation(w => w.Members);
        }
    }
}
