using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmallWorld.Models.Providers;

namespace SmallWorld.Filters
{
    public class AuthRequiredAttribute : TypeFilterAttribute
    {
        public AuthRequiredAttribute() : base(typeof(AuthRequiredFilter)) {}

        private class AuthRequiredFilter : IAuthorizationFilter
        {
            private readonly AuthProvider provider;

            public AuthRequiredFilter(AuthProvider provider)
            {
                this.provider = provider;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var auth = provider.GetSession(context.HttpContext);

                if (auth != null)
                    return;

                context.Result = new UnauthorizedResult();
            }
        }
    }
}