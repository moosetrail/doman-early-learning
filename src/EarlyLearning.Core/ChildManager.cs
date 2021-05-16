using System.Collections.Generic;
using System.Threading.Tasks;
using EarlyLearning.Core.People;

namespace EarlyLearning.Core
{
    public interface ChildManager
    {
        Task<Child> AddChildForUser(string firstName, string lastname, string userId);

        Task<IEnumerable<Child>> GetChildrenForUser(string adultId);

        Task<bool> ChildExists(string firstName, string lastName, string userId);

        Task<Child> GetChild(string firstName, string lastName, string userId);
    }
}