using System.Text.Json;

namespace EarlyLearning.Tests.TestHelpers.TestFactory
{
    public partial class TestFactory
    {
        private static JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            IgnoreNullValues = true,
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static string ToJson(object obj)
        {
            var json = JsonSerializer.Serialize(obj, jsonOptions);
            return json;
        }

        public static T FromJson<T>(string json)
        {
            var deserialized = JsonSerializer.Deserialize<T>(json, jsonOptions);
            return deserialized;
        }
    }
}