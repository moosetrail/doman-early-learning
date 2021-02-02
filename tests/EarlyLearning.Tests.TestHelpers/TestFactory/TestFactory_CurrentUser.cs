using EarlyLearning.API.Fakes;
using EarlyLearning.People.DataModels;
using Moq;

namespace EarlyLearning.Tests.TestHelpers.TestFactory
{
    public partial class TestFactory
    {
        public string UserId => CurrentUser.UserId;

        public CurrentUser CurrentUser => new CurrentFakeUser();
    }
}