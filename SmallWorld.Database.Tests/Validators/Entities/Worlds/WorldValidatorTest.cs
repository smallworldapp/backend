using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Database.Model.Impl;
using SmallWorld.Database.Tests.Validation.Test_Fake;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using SmallWorld.Database.Validators.Entities.Worlds;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Library.Model.Impl;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;
using Xunit;

namespace SmallWorld.Database.Tests.Validators.Entities.Worlds
{
    public class WorldValidatorTest : TestBase
    {
        protected override void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IContext, EmulatedContext>();
            services.AddSingleton<IContextLock, ContextLock>();
            services.AddSingleton<IWorldRepository, WorldRepository>();
        }

        [Theory]
        [InlineData(true, AccountType.Standard, WorldPrivacy.Public)]
        [InlineData(true, AccountType.Standard, WorldPrivacy.InviteOnly)]
        [InlineData(true, AccountType.Standard, WorldPrivacy.VerificationRequired)]
        [InlineData(false, AccountType.Research, WorldPrivacy.Public)]
        [InlineData(true, AccountType.Research, WorldPrivacy.InviteOnly)]
        [InlineData(false, AccountType.Research, WorldPrivacy.VerificationRequired)]
        public async Task Validate_Type_Privacy(bool valid, AccountType type, WorldPrivacy privacy)
        {
            using (var provider = await CreateProvider())
            {
                var worlds = provider.GetRequiredService<IWorldRepository>();

                IValidator<World> validator = new WorldValidator(worlds);

                var world = new World {
                    Account = new Account {
                        Type = type,
                    },
                    Privacy = privacy
                };

                var target = new ValidationTarget<World>(world);
                validator.Validate(target);
                Assert.Equal(valid, !target.GetResult().HasErrors);
            }
        }
    }
}
