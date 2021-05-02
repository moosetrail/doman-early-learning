using System.Collections.Generic;
using System.Threading.Tasks;
using EarlyLearning.API.Controllers.People;
using EarlyLearning.API.Models.People;
using EarlyLearning.Core;
using EarlyLearning.Tests.TestHelpers.Asserts;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using Moq;
using NUnit.Framework;

namespace EarlyLearning.Tests.UnitTests.API.Controllers.People
{
    [TestFixture]
    public class ChildController_Specs
    {

        private ChildController SUT;
        private readonly TestFactory _testFactory = new TestFactory();
        private Mock<ChildManager> _manager;

        [SetUp]
        public void Setup()
        {
            _manager = new Mock<ChildManager>();
            _testFactory.GenerateNewDocumentStore();

            SUT = new ChildController(_testFactory.CurrentUser, _manager.Object, _testFactory.TestLogger());
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public async Task AddChild_should_return_child_from_manager()
        {
            // Given
            var childToAdd = new AddChildVM
            {
                FirstName = "Adam",
                LastName = "Smith"
            };
            var child = _testFactory.NewChild("Adam", "Smith");
            _manager.Setup(x => x.AddChildForUser("Adam", "Smith", _testFactory.CurrentUser.UserId))
                .ReturnsAsync(child);

            // When
            var result = await SUT.AddChild(childToAdd);

            // Then 
            EarlyLearningAssert.HttpResultIsOk(result);
            EarlyLearningAssert.AreEqual(child, EarlyLearningAssert.DataInResult<ChildVM>(result));
        }

        [Test]
        public async Task AddChild_should_return_badRequest_if_childToAdd_isnt_valid()
        {
            // Given
            var childToAdd = new AddChildVM
            {
                FirstName = "Adam",
                LastName = null
            };
            SUT.ModelState.AddModelError("LastName", "Last name required");

            // When
            var result = await SUT.AddChild(childToAdd);

            // Then 
            EarlyLearningAssert.HttpResultIsBadRequest("Invalid child", result);
        }

        [Test]
        public async Task GetChildren_should_return_all_children_from_manager()
        {
            // Given
            var children = new[]
            {
                _testFactory.NewChild(),
                _testFactory.NewChild()
            };
            _manager.Setup(x => x.GetChildrenForUser(_testFactory.CurrentUser.UserId)).ReturnsAsync(children);

            // When
            var result = await SUT.GetChildren();

            // Then 
            EarlyLearningAssert.HttpResultIsOk(result);
            var actualData = EarlyLearningAssert.DataInResult<IEnumerable<ChildVM>>(result);
            EarlyLearningAssert.AreEqual(children, actualData, EarlyLearningAssert.AreEqual);
        }
    }
}