using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

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

        public static void IsSubset<E, A>(IEnumerable<E> expectedSubset, IEnumerable<A> actualFull,
            Func<E, A, bool> findSame, Func<E, A, bool> areEqual)
        {
            foreach (var expected in expectedSubset)
            {
                var actual = actualFull.SingleOrDefault(x => findSame(expected, x));
                Assert.NotNull(actual, "Didn't find a matching element in full set");
                areEqual(expected, actual);
            }
        }

        public static void AreEquivalent<E, A>(IEnumerable<E> expected, IEnumerable<A> actual,
            Func<E, A, bool> findSame, Func<E, A, bool> areEqual)
        {
            NUnit.Framework.Assert.AreEqual(expected.Count(), actual.Count());
            IsSubset(expected, actual, findSame, areEqual);
        }

        public static void AreEqual<E, A>(IEnumerable<E> expected, IEnumerable<A> actual,
            Func<E, A, bool> areEqual)
        {
            NUnit.Framework.Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < expected.Count(); i++)
            {
                var expectedElement = expected.ElementAt(i);
                var actualElement = actual.ElementAt(i);

                areEqual(expectedElement, actualElement);
            }

            
        }

        public static bool AreEqual(object expected, object actual)
        {
            Assert.AreEqual(expected, actual);
            return true;
        }

        public static bool AreEqual(object expected, object actual, string message)
        {
            Assert.AreEqual(expected, actual, message);
            return true;
        }
    }
}