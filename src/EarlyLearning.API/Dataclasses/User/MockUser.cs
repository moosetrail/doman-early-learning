using EarlyLearning.Core.People;

namespace EarlyLearning.API.Dataclasses.User
{
    public class MockUser : AppUser
    {
        public string Id => "MockUserId";

        public string Email => "email@test.com";
    }
}