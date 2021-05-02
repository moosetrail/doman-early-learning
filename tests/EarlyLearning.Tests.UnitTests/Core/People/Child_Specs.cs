using EarlyLearning.Core.People;
using EarlyLearning.Tests.TestHelpers.TestFactory;
using NUnit.Framework;

namespace EarlyLearning.Tests.UnitTests.Core.People
{
    [TestFixture]
    public class Child_Specs
    {
        private Child SUT;

        [SetUp]
        public void Setup()
        {
            SUT = new Child("id", "Firstname", "Lastname");
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        [Test]
        public void Constructor_should_set_firstname()
        {
            // Then 
            Assert.AreEqual("Firstname", SUT.FirstName);
        }

        [Test]
        public void Constructor_should_set_lastname()
        {
            // Then 
            Assert.AreEqual("Lastname", SUT.LastName);
        }

        [Test]
        public void Constructor_should_set_id()
        {
            // Then 
            Assert.AreEqual("id", SUT.Id);
        }
    }
}