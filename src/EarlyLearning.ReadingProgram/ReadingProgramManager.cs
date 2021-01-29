using System.Collections.Generic;
using System.Threading.Tasks;
using EarlyLearning.ReadingPrograms.DataModels;

namespace EarlyLearning.ReadingPrograms
{
    public interface ReadingProgramManager
    {
        Task<IEnumerable<ReadingProgramInfo>> GetAllProgramsForUser(string userId);
    }
}