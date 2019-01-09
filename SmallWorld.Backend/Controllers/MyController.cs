using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Auth;
using SmallWorld.Models.Providers;

namespace SmallWorld.Controllers
{
    public class MyController : Controller
    {
        private Session session;

        protected Session Auth
        {
            get
            {
                if (session == null)
                    session = HttpContext.RequestServices.GetService<AuthProvider>().GetSession(HttpContext);

                return session;
            }
            set
            {
                if (session != null)
                    Debugger.Break();

                session = value;

                HttpContext.RequestServices.GetService<AuthProvider>().AddSession(session);
            }
        }

        protected IPermissions Permissions => Auth?.GetPermissions(HttpContext.RequestServices);
    }
}
