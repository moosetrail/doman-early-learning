using System;
using EarlyLearning.Core.Program;
using EarlyLearning.Core.Program.ActivityStatuses;
using NUnit.Framework;

namespace EarlyLearning.Tests.TestHelpers.Asserts
{
    public partial class EarlyLearningAssert
    {
        public static void AreEqual(ActivityStatus expected, ActivityStatus actual)
        {
            switch (expected)
            {
                case CurrentlyActive ex: AreEqual(ex, actual as CurrentlyActive); break;
                case Planned ex: AreEqual(ex, actual as Planned); break;
                case Retired ex: AreEqual(ex, actual as Retired); break;
                case null: Assert.IsNull(actual); break;
                default: throw new NotSupportedException();
            }
        }

        public static bool AreEqual(CurrentlyActive expected, CurrentlyActive actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.StartDate, actual.StartDate);
            return true;
        }

        public static bool AreEqual(Planned expected, Planned actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.DateAddedToPlan, actual.DateAddedToPlan);
            AreEqual(expected.SortingIndex, actual.SortingIndex);
            return true;
        }

        public static bool AreEqual(Retired expected, Retired actual)
        {
            Assert.NotNull(actual);
            Assert.AreEqual(expected.RetirementDate, actual.RetirementDate);
            return true;
        }
    }
}