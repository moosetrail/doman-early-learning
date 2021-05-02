using System.Threading.Tasks;
using EarlyLearning.API.Controllers.ReadingPrograms;
using EarlyLearning.ReadingPrograms;
using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;
using EarlyLearning.ReadingPrograms.DataModels.ReadingUnits;
using EarlyLearning.Tests.TestHelpers.Asserts;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using Moq;
using NUnit.Framework;

namespace EarlyLearning.Tests.UnitTests.API.Controllers.ReadingPrograms
{
    [TestFixture]
    public class ReadingWordsCategoryController_Specs
    {
        private ReadingWordsCategoryController SUT;
        private readonly TestFactory _testFactory = new TestFactory();
        private Mock<ReadingProgram<ReadingCategory<ReadingWord>>> _readingProgram;

        [SetUp]
        public void Setup()
        {
            _readingProgram = new Mock<ReadingProgram<ReadingCategory<ReadingWord>>>();
            SUT = new ReadingWordsCategoryController(_readingProgram.Object);
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void Should_be_ReadingCategoryController()
        {
            Assert.IsInstanceOf<ReadingCategoryController<ReadingCategory<ReadingWord>>>(SUT);
        }

        [Test]
        public async Task GetCurrent_should_return_current_units_from_program()
        {
            // Given
            var categories = _testFactory.NewWordCategories(5);
            _readingProgram.Setup(x => x.GetCurrent()).ReturnsAsync(categories);

            // When
            var result = await SUT.GetCurrent("program id");

            // Then 
            EarlyLearningAssert.AreEqual(categories, result);
        }
    }
}