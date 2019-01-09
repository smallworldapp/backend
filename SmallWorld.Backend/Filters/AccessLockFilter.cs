using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SmallWorld.Database;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Models.Emailing;

namespace SmallWorld.Filters
{
    public class AccessLockFilter : IAsyncResourceFilter
    {
        private readonly IContextLock access;
        private readonly EmailProvider email;
        private readonly SmallWorldContext sw;

        public AccessLockFilter(IContextLock access, EmailProvider email, SmallWorldContext sw)
        {
            this.access = access;
            this.email = email;
            this.sw = sw;
        }

        private async Task SaveChanges(IContextWritableHandle handle)
        {
#if DEBUG
            if (sw != null)
            {
                var pre = sw.ChangeTracker.Entries().Count(a => a.State != Microsoft.EntityFrameworkCore.EntityState.Unchanged);
                sw.ChangeTracker.DetectChanges();
                var post = sw.ChangeTracker.Entries().Count(a => a.State != Microsoft.EntityFrameworkCore.EntityState.Unchanged);
                Debug.Assert(pre == post, "Forgot to call Context.Update for something");
            }
#endif

            await handle.Finish();
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext before, ResourceExecutionDelegate next)
        {
            var modifying = false;
            if (before.ActionDescriptor is ControllerActionDescriptor action)
                modifying = action.MethodInfo.GetCustomAttributes<DatabaseUpdateAttribute>().Any();

            if (modifying)
            {
                using (var handle = await access.Write())
                {
                    var after = await next();
                    var status = after.HttpContext.Response.StatusCode;

                    if (status < 400)
                        await SaveChanges(handle);
                }
            }
            else
            {
                using (await access.Read())
                {
                    await next();
                }
            }
        }
    }
}
