using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Database.Model.Impl;
using SmallWorld.Database.Tests.Validation.Test_Fake;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using SmallWorld.Database.Validators.Entities.Members;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Library.Model.Impl;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;
using Xunit;

namespace SmallWorld.Database.Tests.Validators.Entities.Members
{
    public class MemberValidatorTest : TestBase
    {
        protected override void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IContext, EmulatedContext>();
            services.AddSingleton<IContextLock, ContextLock>();
            services.AddSingleton<IEntryRepository, EntryRepository>();
            services.AddSingleton<IWorldRepository, WorldRepository>();
        }

        protected override async Task Initialize(IServiceProvider provider)
        {
            var context = provider.GetRequiredService<IContext>();
            await context.Initialize();
        }

        [Fact]
        public async Task Validate_OneMember()
        {
            using (var provider = await CreateProvider())
            {
                var access = provider.GetRequiredService<IContextLock>();
                var worlds = provider.GetRequiredService<IWorldRepository>();
                var entries = provider.GetRequiredService<IEntryRepository>();

                IValidator<Member> validator = new MemberValidator(entries, worlds);

                var member = new Member {
                    World = new World()
                };

                using (await access.Read())
                {
                    var target = new ValidationTarget<Member>(member);
                    validator.Validate(target);
                    Assert.False(target.GetResult().HasErrors);
                }
            }
        }

        [Theory]
        [InlineData(false, "test@example.com", "test@example.com")]
        [InlineData(false, "test@ExAmPlE.com", "test@example.com")]
        [InlineData(false, "TeSt@example.com", "test@example.com")]
        [InlineData(true, "test-2@example.com", "test@example.com")]
        public async Task Validate_TwoMembers(bool valid, string email1, string email2)
        {
            using (var provider = await CreateProvider())
            {
                var context = provider.GetRequiredService<IContext>();
                var access = provider.GetRequiredService<IContextLock>();
                var worlds = provider.GetRequiredService<IWorldRepository>();
                var entries = provider.GetRequiredService<IEntryRepository>();

                IValidator<Member> validator = new MemberValidator(entries, worlds);

                var world = new World();

                using (var handle = await access.Write())
                {
                    var one = new Member {
                        World = world,
                        Email = new EmailAddress(email1)
                    };

                    world.Members = new HashSet<Member> { one };

                    context.Add(one);
                    await handle.Finish();
                }

                using (await access.Read())
                {
                    var two = new Member {
                        World = world,
                        Email = new EmailAddress(email2)
                    };

                    var target = new ValidationTarget<Member>(two);
                    validator.Validate(target);
                    Assert.Equal(valid, !target.GetResult().HasErrors);
                }
            }
        }
    }
}
