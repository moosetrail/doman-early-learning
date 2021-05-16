using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EarlyLearning.Core.Exceptions;
using EarlyLearning.ReadingPrograms;
using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;
using EarlyLearning.ReadingPrograms.DataModels.ReadingUnits;
using EarlyLearning.ReadingPrograms.RavenDb.DataTransferObjects;
using EarlyLearning.ReadingPrograms.RavenDb.Indexes;
using EarlyLearning.Tests.TestHelpers.Asserts;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using NUnit.Framework;
using Raven.Client.Documents.Linq;

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
            _testFactory.GenerateNewDocumentStore(new ReadingProgram_ByUser());
            SUT = new ReadingProgramManagerOnRavenDb(_testFactory.DocumentStore.OpenAsyncSession(), _testFactory.TestLogger());
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

        [Test]
        public async Task UserCanAccessProgram_should_return_true_if_user_is_user_for_child_on_program()
        {
            // Given
            var program = _testFactory.AddNewReadingProgram(userId);

            // When
            var result = await SUT.UserCanAccessProgram(program.Id, userId);

            // Then 
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UserCanAccessProgram_should_return_false_if_user_is_not_user_for_any_child_on_program()
        {
            // Given
            var program = _testFactory.AddNewReadingProgram(userId);

            // When
            var result = await SUT.UserCanAccessProgram(program.Id, "other user");

            // Then 
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetReadingProgram_should_return_singleWord_readingProgram()
        {
            // Given
            var program = _testFactory.AddNewReadingProgram(userId);

            // When
            var result = await SUT.GetReadingProgram<ReadingCategory<ReadingWord>>(program.Id);

            // Then 
            Assert.AreEqual(program.Id, result.ProgramId);
        }

        [Test]
        public async Task GetReadingProgram_should_return_null_if_no_program_with_id_exists()
        {
            // When
            var result = await SUT.GetReadingProgram<ReadingCategory<ReadingWord>>("my id");

            // Then 
            Assert.IsNull(result);
        }

        [Test]
        public void GetReadingProgram_should_throw_if_reading_program_type_isnt_supported_yet()
        {
            // Given
            var program = _testFactory.AddNewReadingProgram(userId);

            // Then 
            Assert.ThrowsAsync<NotSupportedException>(async () => await SUT.GetReadingProgram<HomemadeBook>(program.Id));
        }

        [Test]
        public async Task CreateNewProgram_should_add_program_to_database()
        {
            // Given
            var children = SetupChildren();

            // When
            await SUT.CreateNewProgram(children, userId);

            // Then 
            var inDb = GetReadingProgramForChildren(children);
            Assert.NotNull(inDb);
        }

        private IEnumerable<string> SetupChildren()
        {
            var childList = new []
            {
                _testFactory.AddNewChild("Child 1", "A", userId).Id,
                _testFactory.AddNewChild("Child 2", "B", userId).Id
            };
            return childList;
        }

        private ReadingProgramInfoDTO GetReadingProgramForChildren(IEnumerable<string> children)
        {
            using var session = _testFactory.DocumentStore.OpenSession();
            var inDb = session.Query<ReadingProgramInfoDTO>().SingleOrDefault(x =>
                x.ChildrenIds.ContainsAll(children));
            return inDb;
        }

        [Test]
        public async Task CreateNewProgram_should_return_program_info_for_new_program()
        {
            // Given
            var children = SetupChildren();

            // When
            var result = await SUT.CreateNewProgram(children, userId);

            // Then 
            var inDb = GetReadingProgramForChildren(children);
            EarlyLearningAssert.AreEqual(inDb, result);
        }

        [Test]
        public void CreateNewProgram_should_throw_if_user_isnt_authorized_to_handle_any_of_the_children()
        {
            var children = SetupChildren();
            var additionalChild = _testFactory.AddNewChild("Child 3", "C", "other adult");
            children = new List<string>(children){additionalChild.Id};

            // Then 
            var ex = Assert.ThrowsAsync<UserNotAuthorizedException>(async () =>
                await SUT.CreateNewProgram(children, userId));
        }
    }
}