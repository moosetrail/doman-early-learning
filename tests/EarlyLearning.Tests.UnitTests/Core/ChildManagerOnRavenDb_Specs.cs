using System.Threading.Tasks;
using EarlyLearning.Core.RavenDb;
using EarlyLearning.People;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using NUnit.Framework;

namespace EarlyLearning.Tests.UnitTests.Core
{
    [TestFixture]
    public class ChildManagerOnRavenDb_Specs
    {
        private ChildManagerOnRavenDb SUT;
        private readonly TestFactory _testFactory = new TestFactory();

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
            

            // Then 
        }
    }
}