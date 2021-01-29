using System.Threading.Tasks;
using EarlyLearning.API.Controllers;
using EarlyLearning.API.Controllers.ReadingPrograms;
using EarlyLearning.People.DataModels;
using EarlyLearning.ReadingPrograms;
using EarlyLearning.Tests.TestHelpers.Asserts;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using Moq;
using NUnit.Framework;

namespace EarlyLearning.Tests.UnitTests.API.Controllers.ReadingPrograms
{
    [TestFixture]
    public class ReadingProgramController_Specs
    {
        private ReadingProgramController SUT;
        private readonly TestFactory _testFactory = new TestFactory();
        private Mock<ReadingProgramManager> _programManager;
        private Mock<CurrentUser> _user;
        private static string userId = "My user";

        [SetUp]
        public void Setup()
        {
            _testFactory.GenerateNewDocumentStore();
            _programManager = new Mock<ReadingProgramManager>();
            _user = new Mock<CurrentUser>();
            _user.Setup(x => x.UserId).Returns(userId);
            SUT = new ReadingProgramController(_testFactory.MockLogger(), _programManager.Object, _user.Object);
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void Should_be_ApiControllerBase()
        {
            NUnit.Framework.Assert.IsInstanceOf<ApiControllerBase>(SUT);
        }

        [Test]
        public async Task GetReadingProgramsForUser_should_return_reading_programs_for_current_user()
        {
            // Given
            var programs = new[]
            {
                _testFactory.NewReadingProgramInfo(),
                _testFactory.NewReadingProgramInfo()
            };
            
            _programManager.Setup(x => x.GetAllProgramsForUser(userId)).ReturnsAsync(programs);

            // When
            var result = await SUT.GetReadingProgramsForUser();

            // Then
            EarlyLearningAssert.AreEqual(programs, result);
        }
    }
}