using EarlyLearning.API.Dataclasses.User;
using EarlyLearning.API.Fakes;

namespace EarlyLearning.Tests.TestHelpers.TestFactory
{
    public partial class TestFactory
    {
        public string UserId => CurrentUser.UserId;

        public CurrentUser CurrentUser => new CurrentFakeUser();
    }
}