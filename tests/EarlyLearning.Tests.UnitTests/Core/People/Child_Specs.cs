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
            SUT = new Child("Firstname", "Lastname");
        }

        [TearDown]
        public void Teardown()
        {
            SUT = null;
        }

        #region Constructor

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
            Assert.AreEqual(ExpectedChildId, SUT.Id);
        }

        private static string ExpectedChildId => "Child/Lastname, Firstname";

        #endregion

        #region SetIdWithNumber

        [Test]
        public void SetIdWithNumber_should_set_id_with_number_on_end()
        {
            // When
            SUT.SetIdWithNumber(3);

            // Then 
            Assert.AreEqual(ExpectedChildId + " - 3", SUT.Id);
        }

        #endregion
    }
}