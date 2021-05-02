using System.Linq;
using System.Threading.Tasks;
using EarlyLearning.Core.Program.ActivityStatuses;
using EarlyLearning.ReadingPrograms;
using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;
using EarlyLearning.ReadingPrograms.DataModels.ReadingUnits;
using EarlyLearning.ReadingPrograms.RavenDb;
using EarlyLearning.ReadingPrograms.RavenDb.DataTransferObjects;
using EarlyLearning.Tests.TestHelpers.Asserts;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using NUnit.Framework;

namespace EarlyLearning.Tests.UnitTests.ReadingPrograms.RavenDb
{
    [TestFixture]
    public class SingleWordReadingProgramOnRavenDb_Specs
    {

        private SingleWordReadingProgramOnRavenDb SUT;
        private readonly TestFactory _testFactory = new TestFactory();
        private static string _programId = "ProgramId";

        [SetUp]
        public void Setup()
        {
            _testFactory.GenerateNewDocumentStore();
            SUT = new SingleWordReadingProgramOnRavenDb(_programId, _testFactory.DocumentStore.OpenAsyncSession(), _testFactory.TestLogger());
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
            var result = await SUT.GetCurrent();

            // Then 
            EarlyLearningAssert.AreEqual(categories, result);
        }

        [Test]
        public async Task MovePlanned_should_set_new_sortIndex_to_expected()
        {
            // Given
            var categories = _testFactory.AddNewWordCategories(5, _programId, new Planned());

            var toMove = categories.ElementAt(0);

            _testFactory.WaitOfIndexesInDocumentStore();

            // When
            await SUT.MovePlanned(toMove.Id, 3);

            // Then
            using var session = _testFactory.DocumentStore.OpenSession();
            var updated = session.Load<ReadingCategoryDTO<ReadingWordDTO>>(toMove.Id);

            Assert.AreEqual(3.5, updated.ActivityStatus.SortingIndex);
        }

        [Test]
        public async Task MovePlanned_should_set_new_sortIndex_to_front()
        {
            // Given
            var categories = _testFactory.AddNewWordCategories(5, _programId, new Planned());

            var toMove = categories.ElementAt(3);

            _testFactory.WaitOfIndexesInDocumentStore();

            // When
            await SUT.MovePlanned(toMove.Id, 0);

            // Then 
            using var session = _testFactory.DocumentStore.OpenSession();
            var updated = session.Load<ReadingCategoryDTO<ReadingWordDTO>>(toMove.Id);
            var oldFront = session.Load<ReadingCategoryDTO<ReadingWordDTO>>(categories.ElementAt(0).Id);

            Assert.AreEqual(0, updated.ActivityStatus.SortingIndex);
            Assert.AreEqual(0.5, oldFront.ActivityStatus.SortingIndex);
        }

        [Test]
        public async Task MovePlanned_should_set_sortIndex_to_last_when_neg_spot()
        {
            // Given
            var categories = _testFactory.AddNewWordCategories(5, _programId, new Planned());

            var toMove = categories.ElementAt(3);

            _testFactory.WaitOfIndexesInDocumentStore();

            // When
            await SUT.MovePlanned(toMove.Id, -1);

            // Then 
            using var session = _testFactory.DocumentStore.OpenSession();
            var updated = session.Load<ReadingCategoryDTO<ReadingWordDTO>>(toMove.Id);

            Assert.AreEqual(5, updated.ActivityStatus.SortingIndex);
        }

        [Test]
        public async Task MovePlanned_should_be_able_to_move_in_front_of_current_position()
        {
            // Given
            var categories = _testFactory.AddNewWordCategories(5, _programId, new Planned());

            var toMove = categories.ElementAt(4);

            _testFactory.WaitOfIndexesInDocumentStore();

            // When
            await SUT.MovePlanned(toMove.Id, 1);

            // Then 
            using var session = _testFactory.DocumentStore.OpenSession();
            var updated = session.Load<ReadingCategoryDTO<ReadingWordDTO>>(toMove.Id);

            Assert.AreEqual(0.5, updated.ActivityStatus.SortingIndex);
        }
    }
}