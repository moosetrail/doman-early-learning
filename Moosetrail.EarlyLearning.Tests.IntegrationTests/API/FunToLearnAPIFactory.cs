using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Moosetrail.EarlyLearning.Tests.IntegrationTests.API
{
    public class EarlyLearningAPIFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private static TestFactory _testFactory;

        public EarlyLearningAPIFactory(TestFactory testFactory)
        {
            _testFactory = testFactory;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                RemoveConfiguredRavenDb(services);
                AddTestRavenDb(services);
            });
        }

        private static void RemoveConfiguredRavenDb(IServiceCollection services)
        {
            var store = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(IDocumentStore));

            services.Remove(store);

            var session = services.SingleOrDefault(d => 
                d.ServiceType == typeof(IAsyncDocumentSession));
            services.Remove(session);
        }

        private static void AddTestRavenDb(IServiceCollection services)
        {
            _testFactory.GenerateNewDocumentStore(true);

            services.Add(new ServiceDescriptor(typeof(IDocumentStore), _testFactory.DocumentStore));
            services.Add(new ServiceDescriptor(typeof(IAsyncDocumentSession),
                _testFactory.DocumentStore.OpenAsyncSession()));
        }
    }
}