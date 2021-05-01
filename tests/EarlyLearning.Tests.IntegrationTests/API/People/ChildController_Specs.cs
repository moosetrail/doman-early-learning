using System.Threading.Tasks;
using EarlyLearning.API.Models.People;
using EarlyLearning.Tests.TestHelpers.Asserts;
using NUnit.Framework;

namespace EarlyLearning.Tests.IntegrationTests.API.People
{
    [TestFixture]
    public class ChildController_Specs : APITestBaseClass
    {
        private static string baseUrl = "api/v1/children";

        [Test]
        public async Task GetChildren_should_return_expected_children()
        {
            // Given
            var currentUserId = "abcd-abcd-abcd-abcd";
            var children = new[]
            {
                _testFactory.AddNewChild("Name 1", "Smith",  currentUserId),
                _testFactory.AddNewChild("Name 2", "Smith", currentUserId)
            };
            _testFactory.AddNewChild("Name 3", "Olson", "1234-1234-1234-1234");

            _testFactory.WaitOfIndexesInDocumentStore();

            // When
            var result = await SUT.GetAsync(baseUrl);

            // Then 
            EarlyLearningAssert.HttpResultIsOk(result);
            var actualResult = EarlyLearningAssert.DataInResult<ChildVM[]>(result);
            EarlyLearningAssert.AreEqual(children, actualResult, EarlyLearningAssert.AreEqual);
        }
    }
}