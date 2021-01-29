using System.Threading.Tasks;
using EarlyLearning.ReadingPrograms;
using EarlyLearning.Tests.TestHelpers.Asserts;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using NUnit.Framework;

namespace EarlyLearning.Tests.UnitTests.ReadingPrograms
{
    [TestFixture]
    public class ReadingProgramManagerOnRavenDb_Specs
    {
        private ReadingProgramManagerOnRavenDb SUT;
        private readonly TestFactory _testFactory = new TestFactory();
        private static string userId = "My user id";

        [SetUp]
        public void Setup()
        {
            SUT = new ReadingProgramManagerOnRavenDb();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void Should_be_ReadingProgramManager()
        {
            Assert.IsInstanceOf<ReadingProgramManager>(SUT);
        }

        [Test]
        public async Task GetAllProgramsForUser_should_return_programs_for_user_in_ravenDb()
        {
            // Given
            var programs = new[]
            {
                _testFactory.AddNewReadingProgram(userId),
                _testFactory.AddNewReadingProgram(userId)
            };
            _testFactory.AddNewReadingProgram(userId + 1);

            // When
            var result = await SUT.GetAllProgramsForUser(userId);

            // Then 
            EarlyLearningAssert.AreEqual(programs, result, EarlyLearningAssert.AreEqual);
        }
    }
}