using System;
using System.Linq;
using System.Threading.Tasks;
using EarlyLearning.API.Controllers.ReadingPrograms;
using EarlyLearning.API.Models.ReadingPrograms;
using EarlyLearning.Core.Program.ActivityStatuses;
using EarlyLearning.ReadingPrograms;
using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;
using EarlyLearning.ReadingPrograms.DataModels.ReadingUnits;
using EarlyLearning.Tests.TestHelpers.Asserts;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using Microsoft.AspNetCore.Mvc;
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
        private Mock<ReadingProgramManager> _manager;

        [SetUp]
        public void Setup()
        {
            _readingProgram = new Mock<ReadingProgram<ReadingCategory<ReadingWord>>>();
            _manager = new Mock<ReadingProgramManager>();
            _manager.Setup(x => x.UserCanAccessProgram(It.IsAny<string>(), _testFactory.UserId)).ReturnsAsync(true);
            _manager.Setup(x => x.GetReadingProgram<ReadingCategory<ReadingWord>>(It.IsAny<string>()))
                .ReturnsAsync(_readingProgram.Object);

            SUT = new ReadingWordsCategoryController(_manager.Object, _testFactory.CurrentUser, _testFactory.TestLogger());
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

        [Test]
        [TestCaseSource(nameof(_apiEndpoints))]
        public async Task GetCurrent_should_return_not_authorized_if_user_is_not_allowed_on_program(Func<string, ReadingWordsCategoryController, Task<IActionResult>> func)
        {
            // Given
            _manager.Setup(x => x.UserCanAccessProgram("Program id", _testFactory.UserId));

            // When
            var result = await func("Program id", SUT);

            // Then 
            EarlyLearningAssert.HttpResultIsUnauthorized(result);
        }

        private static Func<string, ReadingWordsCategoryController, Task<IActionResult>>[] _apiEndpoints =
            {
                (programId, SUT) => SUT.GetCurrent(programId),
                (programId, SUT) => SUT.GetPlanned(programId),
                (programId, SUT) => SUT.GetRetired(programId),
                (programId, SUT) => SUT.Add(new ReadingCategoryToAddVM
                {
                    OnTheCards = new string[0]
                }, programId),
                (programId, SUT) => SUT.ChangeStatus(programId, "unit", ReadingUnitStatusVM.Active),
                (programId, SUT) => SUT.MovePlanned("unit", programId, 3),
            };

        [Test]
        [TestCaseSource(nameof(_apiEndpoints))]
        public async Task GetCurrent_should_return_notFound_if_program_does_not_exist(Func<string, ReadingWordsCategoryController, Task<IActionResult>> func)
        {
            // Given
            _manager.Setup(x => x.GetReadingProgram<ReadingCategory<ReadingWord>>(It.IsAny<string>()))
                .ReturnsAsync((ReadingProgram<ReadingCategory<ReadingWord>>)null);

            // When
            var result = await func("program id", SUT);

            // Then 
            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        [Test]
        public async Task GetCurrent_should_return_planned_units_from_program()
        {
            // Given
            var categories = _testFactory.NewWordCategories(5);
            _readingProgram.Setup(x => x.GetPlanned(5, 10)).ReturnsAsync(categories);

            // When
            var result = await SUT.GetPlanned("program id", 5, 10);

            // Then 
            EarlyLearningAssert.AreEqual(categories, result);
        }

        [Test]
        public async Task GetCurrent_should_return_retired_units_from_program()
        {
            // Given
            var categories = _testFactory.NewWordCategories(5);
            _readingProgram.Setup(x => x.GetRetired(5, 10)).ReturnsAsync(categories);

            // When
            var result = await SUT.GetRetired("program id", 5, 10);

            // Then 
            EarlyLearningAssert.AreEqual(categories, result);
        }

        [Test]
        public async Task Add_should_add_category_to_program()
        {
            // Given
            var toAdd = new ReadingCategoryToAddVM
            {
                Title = "My category",
                OnTheCards = new[] {"Card 1", "Card 2", "Card 3"}
            };
            var addedCategory =
                _testFactory.NewWordCategory(new Planned(), "My category", "catId", toAdd.OnTheCards.ToArray());
            _readingProgram.Setup(x => x.Add(It.IsAny<ReadingCategory<ReadingWord>>())).ReturnsAsync(addedCategory);

            // When
            var result = await SUT.Add(toAdd, "program id");

            // Then 
            EarlyLearningAssert.HttpResultIsOk(result);
            var actualData = EarlyLearningAssert.DataInResult<ReadingCategoryVM>(result);
            EarlyLearningAssert.AreEqual(addedCategory, actualData);
        }

        [Test]
        public async Task ChangeStatus_should_change_status_in_program()
        {
            // Given

            // When
            var result = await SUT.ChangeStatus("program id", "unit", ReadingUnitStatusVM.Active);

            // Then
            EarlyLearningAssert.HttpResultIsOk(result);
            _readingProgram.Verify(x => x.ChangeStatus("unit", It.IsAny<CurrentlyActive>()));
        }

        [Test]
        public async Task MovePlanned_should_move_planned_in_program()
        {
            // Given

            // When
            var result = await SUT.MovePlanned("unit", "program id", 5);

            // Then 
            EarlyLearningAssert.HttpResultIsOk(result);
            _readingProgram.Verify(x => x.MovePlanned("unit", 5));
        }
    }
}