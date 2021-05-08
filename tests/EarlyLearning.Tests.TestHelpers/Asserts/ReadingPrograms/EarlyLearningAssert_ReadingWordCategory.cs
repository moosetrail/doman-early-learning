using System.Collections.Generic;
using System.Linq;
using EarlyLearning.API.Models.ReadingPrograms;
using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;
using EarlyLearning.ReadingPrograms.DataModels.ReadingUnits;
using Microsoft.AspNetCore.Mvc;

namespace EarlyLearning.Tests.TestHelpers.Asserts
{
    public partial class EarlyLearningAssert
    {
        public static bool AreEqual(ReadingCategory<ReadingWord> expected, ReadingCategory<ReadingWord> actual)
        {
            AreEqual(expected.Title, actual.Title);
            AreEqual(expected.ReadingCards, actual.ReadingCards, AreEqual);
            AreEqual(expected.ActivityStatus, actual.ActivityStatus);
            return true;
        }

        public static void AreEqual(IEnumerable<ReadingCategory<ReadingWord>> expected,
            IEnumerable<ReadingCategory<ReadingWord>> actual)
        {
            AreEqual(expected, actual, AreEqual);
        }

        public static bool AreEqual(ReadingCategory<ReadingWord> expected, ReadingCategoryVM actual)
        {
            AreEqual(expected.Title, actual.Title);
            AreEqual(expected.ReadingCards.Select(x => x.TextOnTheCard), actual.Cards, AreEqual);
            return true;
        }

        public static void AreEqual(IEnumerable<ReadingCategory<ReadingWord>> expected, IActionResult actualResult)
        {
            HttpResultIsOk(actualResult);
            var actual = DataInResult<IEnumerable<object>>(actualResult);
            AreEqual(expected, actual, AreEqual);
        }

        public static bool AreEqual(ReadingCategory<ReadingWord> expected, object actual)
        {
            var actualCategory = actual as ReadingCategoryVM;
            return AreEqual(expected, actualCategory);
        }
    }
}