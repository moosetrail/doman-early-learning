using System.Collections.Generic;
using EarlyLearning.API.Models.ReadingPrograms;
using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;
using EarlyLearning.ReadingPrograms.DataModels.ReadingUnits;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace EarlyLearning.Tests.TestHelpers.Asserts
{
    public partial class EarlyLearningAssert
    {
        public static bool AreEqual(ReadingCategory<ReadingWord> expected, ReadingCategoryVM actual)
        {
            Assert.Fail("Not implemented yet");
            return true;
        }

        public static void AreEqual(IEnumerable<ReadingCategory<ReadingWord>> expected, IActionResult actualResult)
        {
            HttpResultIsOk(actualResult);
            var actual = DataInResult<IEnumerable<ReadingCategoryVM>>(actualResult);
            AreEqual(expected, actual, AreEqual);
        }
    }
}