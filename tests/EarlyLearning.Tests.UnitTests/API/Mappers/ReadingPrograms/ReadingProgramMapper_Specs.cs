using EarlyLearning.API.Mappers.ReadingPrograms;
using EarlyLearning.Tests.TestHelpers.Asserts;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using NUnit.Framework;

namespace EarlyLearning.Tests.UnitTests.API.Mappers.ReadingPrograms
{
    [TestFixture]
    public class ReadingProgramMapper_Specs
    {
        private readonly TestFactory _testFactory = new TestFactory();

        [Test]
        public void ToReadingProgramVM_should_map_all_properties()
        {
            // Given
            var program = _testFactory.NewReadingProgramInfo();

            // When
            var result = program.ToReadingProgramVM();

            // Then 
            EarlyLearningAssert.AreEqual(program, result);
        }
    }
}