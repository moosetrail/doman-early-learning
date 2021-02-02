using EarlyLearning.ReadingPrograms;
using Microsoft.Extensions.DependencyInjection;

namespace EarlyLearning.API.DependencyInjection
{
    internal static class ReadingProgramDependencies
    {
        public static void Add(IServiceCollection services)
        {
            services.AddTransient<ReadingProgramManager, ReadingProgramManagerOnRavenDb>();
        }
    }
}