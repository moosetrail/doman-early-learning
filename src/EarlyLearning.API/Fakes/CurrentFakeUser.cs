using EarlyLearning.People.DataModels;
using CurrentUser = EarlyLearning.API.Dataclasses.User.CurrentUser;

namespace EarlyLearning.API.Fakes
{
    public class CurrentFakeUser: CurrentUser
    {
        public string UserId => "abcd-abcd-abcd-abcd";
    }
}