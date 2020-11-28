using EarlyLearning.API.Controllers.ReadingProgram;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using NUnit.Framework;

namespace EarlyLearning.Tests.UnitTests.API.Controllers.ReadingProgram
{
    [TestFixture]
    public class ReadingProgramController_Specs
    {
        private ReadingProgramController SUT;
        private readonly TestFactory _testFactory = new TestFactory();

        [SetUp]
        public void Setup()
        {
            SUT = new ReadingProgramController();
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        
    }
}