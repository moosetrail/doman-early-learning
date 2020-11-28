using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EarlyLearning.Tests.TestHelpers.Asserts
{
    public partial class EarlyLearningAssert
    {
        private StringContent ToJson(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }

        private string ToJsonString(object obj)
        {
            var json = ToJson(obj);
            return json.ReadAsStringAsync().Result;
        }
    }
}