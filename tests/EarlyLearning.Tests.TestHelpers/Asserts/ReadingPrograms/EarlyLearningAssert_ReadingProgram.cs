using System.Collections.Generic;
using System.Net.Http;
using EarlyLearning.API.Models.ReadingPrograms;
using EarlyLearning.ReadingPrograms.DataModels;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace EarlyLearning.Tests.TestHelpers.Asserts
{
    public partial class EarlyLearningAssert
    {
        public static bool AreEqual(ReadingProgramInfo expected, ReadingProgramInfo actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.Id, actual.Id);
            AreEqual(expected.Children, actual.Children, AreEqual);

            return true;
        }

        public static bool AreEqual(ReadingProgramInfo expected, ReadingProgramVM actual)
        {
            Assert.NotNull(actual, "Actual was null");
            Assert.AreEqual(expected.Id, actual.ProgramId, "ProgramId didn't match");

            AreEqual(expected.Children, actual.ChildrenIds, AreEqual);

            return true;
        }

        public static void AreEqual(IEnumerable<ReadingProgramInfo> expectedPrograms,
            IActionResult actualActionResult)
        {
            HttpResultIsOk(actualActionResult);
            var actualVms = ResultInOk<IEnumerable<ReadingProgramVM>>(actualActionResult as OkObjectResult);
            AreEqual(expectedPrograms, actualVms, AreEqual);
        }

        public static void AreEqual(IEnumerable<ReadingProgramInfo> expectedPrograms,
            HttpResponseMessage actualResponse)
        {
            HttpResultIsOk(actualResponse);

            var actualVms = DataInMessage<IEnumerable<ReadingProgramVM>>(actualResponse);
            AreEqual(expectedPrograms, actualVms, AreEqual);
        }
    }
}