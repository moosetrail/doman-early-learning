using System.Net.Http;
using System.Text;
using EarlyLearning.API;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

        protected StringContent ToJson(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }

        protected string ToJsonString(object obj)
        {
            var json = ToJson(obj);
            return json.ReadAsStringAsync().Result;
        }
    }
}