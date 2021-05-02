using Microsoft.Extensions.DependencyInjection;

namespace EarlyLearning.API.DependencyInjection
{
    public static class EarlyLearningAPI
    {
        public static void BuildDependencies(IServiceCollection services)
        {
            CoreDependencies.Add(services);
            ReadingProgramDependencies.Add(services);
        }
    }
}