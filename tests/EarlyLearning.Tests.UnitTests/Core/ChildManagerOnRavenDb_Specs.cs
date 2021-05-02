using System.Threading.Tasks;
using EarlyLearning.Core;
using EarlyLearning.Core.People;
using EarlyLearning.Core.RavenDb;
using EarlyLearning.Tests.TestHelpers.Asserts;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using NUnit.Framework;

namespace EarlyLearning.Tests.UnitTests.Core
{
    [TestFixture]
    public class ChildManagerOnRavenDb_Specs
    {
        private ChildManagerOnRavenDb SUT;
        private readonly TestFactory _testFactory = new TestFactory();
        private static string UserId = "My user";

        [SetUp]
        public void Setup()
        {
            _testFactory.GenerateNewDocumentStore();
            SUT = new ChildManagerOnRavenDb(_testFactory.DocumentStore.OpenAsyncSession(), _testFactory.TestLogger());
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void Should_be_ChildManager()
        {
            Assert.IsInstanceOf<ChildManager>(SUT);
        }

        [Test]
        public async Task AddChildWithAdult_should_save_child_to_db()
        {
            // Given

            // When
            var result = await SUT.AddChildForUser("Adam", "Smith", UserId);

            // Then 
            var session = _testFactory.DocumentStore.OpenSession();
            var childInDb = session.Load<Child>(result.Id);
            Assert.NotNull(childInDb);
            Assert.AreEqual("Adam", childInDb.FirstName);
            Assert.AreEqual("Smith", childInDb.LastName);
        }

        [Test]
        public async Task AddChildWIthAdult_should_set_id_to_be_name_of_child()
        {
            // Given

            // When
            var result = await SUT.AddChildForUser("Adam", "Smith", UserId);

            // Then
            Assert.AreEqual("Child/Smith, Adam", result.Id);
        }

        [Test]
        public async Task AddChildWithAdult_should_add_number_if_more_than_one_child_with_same_name_exists()
        {
            // Given
            await SUT.AddChildForUser("Adam", "Smith", UserId);
            _testFactory.WaitOfIndexesInDocumentStore();

            // When
            var result = await SUT.AddChildForUser("Adam", "Smith", UserId);

            // Then
            Assert.AreEqual("Child/Smith, Adam - 2", result.Id);
        }

        [Test]
        public async Task GetChildrenForUser_should_return_children_for_user()
        {
            // Given
            var expectedChildren = new[]
            {
                await SUT.AddChildForUser("Adam", "Smith", UserId),
                await SUT.AddChildForUser("Lisa", "Adams", UserId)
            };
            await SUT.AddChildForUser("Nils", "Ericsson", "Other userId");

            _testFactory.WaitOfIndexesInDocumentStore();

            // When
            var result = await SUT.GetChildrenForUser(UserId);

            // Then 
            EarlyLearningAssert.AreEqual(expectedChildren, result, EarlyLearningAssert.AreEqual);
        }
    }
}