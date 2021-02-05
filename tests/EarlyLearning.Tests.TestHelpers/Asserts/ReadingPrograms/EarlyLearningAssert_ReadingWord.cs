using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;

namespace EarlyLearning.Tests.TestHelpers.Asserts
{
    public partial class EarlyLearningAssert
    {
        public static bool AreEqual(ReadingWord expected, ReadingWord actual)
        {
            AreEqual(expected.TextOnTheCard, actual.TextOnTheCard);
            return true;
        }
    }
}