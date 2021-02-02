using System.Linq;
using EarlyLearning.ReadingPrograms.DataModels;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using NUnit.Framework;

namespace EarlyLearning.Tests.UnitTests.ReadingPrograms.DataModels
{
    [TestFixture]
    public class ReadingProgramInfo_Specs
    {
        private ReadingProgramInfo SUT;
        private readonly TestFactory _testFactory = new TestFactory();

        [SetUp]
        public void Setup()
        {
            SUT = new ReadingProgramInfo();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        #region Constructor

        [Test]
        public void Constructor_should_set_id_when_provided()
        {
            // When
            SUT = new ReadingProgramInfo("My id");

            // Then 
            Assert.AreEqual("My id", SUT.Id);
        }

        [Test]
        public void Constructor_should_set_children()
        {
            // Given
            var children = _testFactory.NewChildList().Select(x => x.Id).ToArray();

            // When
            SUT = new ReadingProgramInfo(childIds: children);

            // Then 
            CollectionAssert.AreEqual(children, SUT.Children);
            Assert.IsNull(SUT.Id);
        }

        #endregion
    }
}