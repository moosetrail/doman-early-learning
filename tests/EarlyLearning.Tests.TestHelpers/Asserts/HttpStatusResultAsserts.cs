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

        private static T DataInMessage<T>(HttpResponseMessage message) where T : class
        {
            var json = message.Content.ReadAsStringAsync().Result;
            return TestFactory.TestFactory.FromJson<T>(json);
        }

        public static void HttpResultIsOk(HttpResponseMessage response)
        {
            NUnit.Framework.Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        public static void HttpResultIsOk(IActionResult result)
        {
            if (result is OkResult || result is OkObjectResult)
            {

            } else 
                NUnit.Framework.Assert.IsInstanceOf<OkObjectResult>(result);
        }

        public static void HttpResultIsBadRequest(string expectedMsg, IActionResult result)
        {
            NUnit.Framework.Assert.IsInstanceOf<BadRequestObjectResult>(result);
            NUnit.Framework.Assert.AreEqual(expectedMsg, ((BadRequestObjectResult)result).Value);
        }

        public static void HttpResultIsUnauthorized(IActionResult result)
        {
            NUnit.Framework.Assert.IsInstanceOf<UnauthorizedResult>(result);
        }
    }
}