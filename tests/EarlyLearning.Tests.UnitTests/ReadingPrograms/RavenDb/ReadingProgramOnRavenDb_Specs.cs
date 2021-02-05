using System.Threading.Tasks;
using EarlyLearning.Core.Program.ActivityStatuses;
using EarlyLearning.ReadingPrograms;
using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;
using EarlyLearning.ReadingPrograms.DataModels.ReadingUnits;
using EarlyLearning.ReadingPrograms.RavenDb;
using EarlyLearning.Tests.TestHelpers.Asserts;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using NUnit.Framework;

namespace EarlyLearning.Tests.UnitTests.ReadingPrograms.RavenDb
{
    [TestFixture]
    public class ReadingProgramOnRavenDb_Specs
    {

        private ReadingProgramOnRavenDb SUT;
        private readonly TestFactory _testFactory = new TestFactory();
        private static string _programId = "ProgramId";

        [SetUp]
        public void Setup()
        {
            _testFactory.GenerateNewDocumentStore();
            SUT = new ReadingProgramOnRavenDb(_testFactory.DocumentStore.OpenAsyncSession());
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void Should_be_ReadingProgram()
        {
            Assert.IsInstanceOf<ReadingProgram<ReadingCategory<ReadingWord>>>(SUT);
        }

        [Test]
        public async Task GetCurrent_should_return_categories_that_are_current_on_the_program()
        {
            // Given
            var categories = _testFactory.AddNewWordCategories(5, _programId, new CurrentlyActive());
            _testFactory.AddNewWordCategories(programId: "Other id", status: new CurrentlyActive());
            _testFactory.AddNewWordCategories(programId: _programId, status: new Planned());

            _testFactory.WaitOfIndexesInDocumentStore();

            // When
            var result = await SUT.GetCurrent(_programId);

            // Then 
            EarlyLearningAssert.AreEqual(categories, result);
        }
    }
}