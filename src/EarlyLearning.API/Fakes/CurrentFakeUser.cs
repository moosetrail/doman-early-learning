using EarlyLearning.People.DataModels;

namespace EarlyLearning.API.Fakes
{
    public class CurrentFakeUser: CurrentUser
    {
        public string UserId => "abcd-abcd-abcd-abcd";
    }
}