using System.Threading.Tasks;
using EarlyLearning.Tests.TestHelpers.Asserts;
using NUnit.Framework;

namespace EarlyLearning.Tests.IntegrationTests.API.ReadingPrograms
{
    [TestFixture]
    public class ReadingProgramController_Specs : APITestBaseClass
    {
        private static string baseUrl = "api/v1/reading-program";

        [Test]
        public async Task GetAllReadingProgramsForUser_should_return_programs()
        {
            // Given
            var programs = new[]
            {
                _testFactory.AddNewReadingProgram()
            };
            _testFactory.AddNewReadingProgram("other id");

            // When
            var result = await SUT.GetAsync(baseUrl);

            // Then 
            EarlyLearningAssert.AreEqual(programs, result);
        }
    }
}