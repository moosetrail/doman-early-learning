using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EarlyLearning.API.Controllers;
using EarlyLearning.API.Controllers.ReadingPrograms;
using EarlyLearning.Core.People;
using EarlyLearning.People.DataModels;
using EarlyLearning.ReadingPrograms.DataModels;
using EarlyLearning.Tests.TestHelpers.Asserts;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EarlyLearning.Tests.UnitTests.API.Controllers.ReadingPrograms
{
    [TestFixture]
    public class ReadingProgramController_Specs
    {
        private ReadingProgramController SUT;
        private readonly TestFactory _testFactory = new TestFactory();
        private Mock<AppUser> currentUserMock = new Mock<AppUser>();

        [SetUp]
        public void Setup()
        {
            _testFactory.GenerateNewDocumentStore();
            SUT = new ReadingProgramController(_testFactory.MockLogger());
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void Should_be_ApiControllerBase()
        {
            Assert.IsInstanceOf<ApiControllerBase>(SUT);
        }

        #region GetReadingProgramsForUser

        [Test]
        public async Task GetReadingProgramsForUser_should_return_reading_programs_for_current_user()
        {
            // Given
            var children = _testFactory.NewChildList().ToArray();
            SetCurrentUserWithChildren("test@example.com", children);

            var programs = new[]
            {
                CreateReadingProgram(children),
                CreateReadingProgram(children[0])
            };

            CreateReadingProgram(_testFactory.NewChild());

            // When
            var result = await SUT.GetReadingProgramsForUser();

            // Then
            Assert.IsInstanceOf<OkObjectResult>(result);
            var resultObject = result as OkObjectResult;
            Assert.IsNotNull(resultObject);
            EarlyLearningAssert.AreEqual(programs, resultObject.Value);
        }

        #endregion

        #region TestHelpers

        private ReadingProgramInfo CreateReadingProgram(params Child[] forChildren)
        {
            var program = new ReadingProgramInfo(forChildren);

            using var session = _testFactory.DocumentStore.OpenSession();
            session.Store(program);
            session.SaveChanges();

            return program;
        }

        private void SetCurrentUserWithChildren(string email, IEnumerable<Child> children)
        {
            currentUserMock.Setup(x => x.Email).Returns(email);
            var user = new AdultProgrammer(email);
            foreach (var child in children)
            {
                user.AddChild(child.Id);
            }

            using var session = _testFactory.DocumentStore.OpenSession();
            session.Store(user);
            session.SaveChanges();
        }

        #endregion
    }
}