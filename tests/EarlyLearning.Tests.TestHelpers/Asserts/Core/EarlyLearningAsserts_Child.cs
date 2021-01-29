using EarlyLearning.API.Models.People;
using EarlyLearning.Core.People;

namespace EarlyLearning.Tests.TestHelpers.Asserts
{
    public partial class EarlyLearningAssert
    {
        public static bool AreEqual(Child expected, Child actual)
        {
            AreEqual(expected.Id, actual.Id, "Child id didn't match");
            AreEqual(expected.FirstName, actual.FirstName, "Child first name didn't match");
            AreEqual(expected.LastName, actual.LastName, "Child last name didn't match");

            return true;
        }

        public static bool AreEqual(Child expected, ChildVM actual)
        { 
            AreEqual(expected.Id, actual.Id, "Child id didn't match");
            AreEqual(expected.FirstName, actual.FirstName, "Child first name didn't match");
            AreEqual(expected.LastName, actual.LastName, "Child last name didn't match");

            return true;
        }
    }
}