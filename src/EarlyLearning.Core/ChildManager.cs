using System.Collections.Generic;
using System.Threading.Tasks;
using EarlyLearning.Core.People;

namespace EarlyLearning.People
{
    public interface ChildManager
    {
        Task<Child> AddChildWithAdult(string firstName, string lastname, string adultId);

        Task<IEnumerable<Child>> GetChildrenForAdult(string adultId);
    }
}