//using Microsoft.Extensions.DependencyInjection;
//using SmallWorld.Database.Model.Abstractions;
//using SmallWorld.Database.Model.Impl;
//using SmallWorld.Database.Tests.Fakes.Model;
//using SmallWorld.Library.Model.Abstractions;
//using SmallWorld.Library.Model.Impl;
//
//namespace SmallWorld.Database.Tests.Fakes
//{
//    public static class FakeExtensions
//    {
//        public static IServiceCollection AddFakeContext(this IServiceCollection services)
//        {
//            services.AddScoped<IContext, FakeContext>();
//            services.AddScoped<IContextLock, ContextLock>();
//
//            return services;
//        }
//    }
//}
