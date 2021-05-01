using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace EarlyLearning.Tests.TestHelpers.Asserts
{
    public partial class EarlyLearningAssert
    {
        private static T ResultInOk<T>(ObjectResult result) where T : class
        {
            return result.Value as T;
        }

        public static T DataInResult<T>(IActionResult result) where T : class
        {
            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult);

            var data = objectResult.Value as T;

            return data;
        }

        public static T DataInResult<T>(HttpResponseMessage message) where T : class
        {
            var json = message.Content.ReadAsStringAsync().Result;
            return TestFactory.TestFactory.FromJson<T>(json);
        }

        public static void HttpResultIsOk(HttpResponseMessage response)
        {
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        public static void HttpResultIsOk(IActionResult result)
        {
            if (result is OkResult || result is OkObjectResult)
            {

            } else 
                Assert.IsInstanceOf<OkObjectResult>(result);
        }

        public static void HttpResultIsBadRequest(string expectedMsg, IActionResult result)
        {
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(expectedMsg, ((BadRequestObjectResult)result).Value);
        }

        public static void HttpResultIsUnauthorized(IActionResult result)
        {
            Assert.IsInstanceOf<UnauthorizedResult>(result);
        }
    }
}