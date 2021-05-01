using System;
using System.Linq;
using EarlyLearning.API.Dataclasses.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Serilog;
using TestFactory = EarlyLearning.Tests.TestHelpers.TestFactory.TestFactory;

namespace EarlyLearning.Tests.IntegrationTests.API
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
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                ChangeLoggingSetup(services);
                ChangeRavenDbSetup(services);
                SetupCurrentUser(services);
            });
        }

        private static void ChangeRavenDbSetup(IServiceCollection services)
        {
            RemoveConfiguredRavenDb(services);
            AddTestRavenDb(services);
        }

        private static void RemoveConfiguredRavenDb(IServiceCollection services)
        {
            RemoveService(services, typeof(IDocumentStore));

            var session = services.SingleOrDefault(d =>
                d.ServiceType == typeof(IAsyncDocumentSession));
            services.Remove(session);
        }

        private static void RemoveService(IServiceCollection services, Type type)
        {
            var service = services.SingleOrDefault(
                d => d.ServiceType ==
                     type);

            services.Remove(service);
        }

        private static void AddTestRavenDb(IServiceCollection services)
        {
            _testFactory.GenerateNewDocumentStore(true);

            services.Add(new ServiceDescriptor(typeof(IDocumentStore), _testFactory.DocumentStore));
            services.Add(new ServiceDescriptor(typeof(IAsyncDocumentSession),
                _testFactory.DocumentStore.OpenAsyncSession()));
        }

        private static void ChangeLoggingSetup(IServiceCollection services)
        {
            var startupLogger = services.Where(x => x.ServiceType == typeof(ILogger)).ToList();
            foreach (var serviceDescriptor in startupLogger)
            {
                services.Remove(serviceDescriptor);
            }

            services.AddSingleton(_testFactory.TestLogger());
        }

        private static void SetupCurrentUser(IServiceCollection services)
        {
            RemoveService(services, typeof(CurrentUser));
            services.Add(new ServiceDescriptor(typeof(CurrentUser), _testFactory.CurrentUser));
        }
    }
}