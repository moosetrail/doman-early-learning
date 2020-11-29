using System.Collections.Generic;
using System.Linq;
using EarlyLearning.Core.People;
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
            SUT = new ReadingProgramInfo(new List<Child>());
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        #region Constructor

        [Test]
        public void Constructor_should_set_childIds()
        {
            // Given
            var children = _testFactory.NewChildList();

            // When
            SUT = new ReadingProgramInfo(children);

            // Then 
            CollectionAssert.AreEqual(children.Select(x => x.Id), SUT.ChildrenIds);
        }

        #endregion
    }
}