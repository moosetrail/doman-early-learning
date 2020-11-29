using System.Collections.Generic;
using EarlyLearning.API.Models.ReadingPrograms;
using EarlyLearning.ReadingPrograms.DataModels;

namespace EarlyLearning.Tests.TestHelpers.Asserts
{
    public partial class EarlyLearningAssert
    {
        public static void AreEqual(IEnumerable<ReadingProgramInfo> expectedPrograms,
            object actualPrograms)
        {

        }

        public static void AreEqual(IEnumerable<ReadingProgramInfo> expectedPrograms,
            IEnumerable<ReadingProgramVM> actualPrograms)
        {

        }
    }
}