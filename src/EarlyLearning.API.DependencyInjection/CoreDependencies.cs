using EarlyLearning.Core;
using EarlyLearning.Core.RavenDb;
using Microsoft.Extensions.DependencyInjection;

namespace EarlyLearning.API.DependencyInjection
{
    internal static class CoreDependencies
    {
        public static void Add(IServiceCollection services)
        {
            services.AddTransient<ChildManager, ChildManagerOnRavenDb>();
        }
    }
}