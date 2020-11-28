using System.Net.Http;
using EarlyLearning.API;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace EarlyLearning.Tests.IntegrationTests.API
{
    public abstract class ApiTestBase
    {
        protected static readonly TestFactory TestFactory = new TestFactory();

        protected static readonly WebApplicationFactory<Startup> ServerFactory = new EarlyLearningAPIFactory<Startup>(TestFactory);

        protected HttpClient SUT;

        [SetUp]
        protected void Setup()
        {
            SUT = ServerFactory.WithWebHostBuilder(config =>
            {
                config.UseSetting("RavenDbLocation", "http://localhost:5100/");
                config.UseSetting("CORS", "http://localhost");
            }).CreateClient();
        }

        [TearDown]
        protected void Teardown()
        {
            SUT.Dispose();
        }
    }
}