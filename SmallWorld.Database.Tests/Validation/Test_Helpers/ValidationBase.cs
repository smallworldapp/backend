using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Model;
using SmallWorld.Database.Tests.Validation.Test_Fake;
using SmallWorld.Database.Validators;
using SmallWorld.Library.Model;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace SmallWorld.Database.Tests.Validation.Test_Helpers
{
    public abstract class ValidationBase<T> : TestBase
    {
        private readonly ITestOutputHelper output;

        protected ValidationBase(ITestOutputHelper output)
        {
            this.output = output;
        }

        protected override void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IContext, EmulatedContext>();
            services.AddModel();
            services.AddValidators();
            services.AddValidation();
            services.AddRepositories();
        }

        protected virtual async Task<bool> Validate(T value)
        {
            using (var provider = await CreateProvider())
            {
                var access = provider.GetRequiredService<IContextLock>();
                var validation = provider.GetRequiredService<IValidationProvider>();

                using (await access.Read())
                {
                    var result = validation.Validate(value);
                    return !result.HasErrors;
                }
            }
        }

        public virtual async Task Validate_True(T value)
        {
            Assert.True(await Validate(value));
        }

        public virtual async Task Validate_False(T value)
        {
            Assert.False(await Validate(value));
        }
    }
}