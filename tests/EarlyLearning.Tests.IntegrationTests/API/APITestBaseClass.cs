using System.Net.Http;
using EarlyLearning.API;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace EarlyLearning.Tests.IntegrationTests.API
{
    public abstract class APITestBaseClass
    {
        protected static readonly TestFactory _testFactory = new TestFactory();

        protected static readonly WebApplicationFactory<Startup> ServerFactory = new EarlyLearningAPIFactory<Startup>(_testFactory);

        protected HttpClient SUT;

        [SetUp]
        public void Setup()
        {
            SUT = ServerFactory.WithWebHostBuilder(config =>
            {
                config.UseSetting("RavenDbLocation", "http://localhost:5100/");
                config.UseSetting("CORS", "http://localhost");
            }).CreateClient();
        }

        [TearDown]
        public void Teardown()
        {
            SUT.Dispose();
        }
    }
}