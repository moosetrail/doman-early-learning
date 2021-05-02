using System.Collections.Generic;
using System.Threading.Tasks;
using EarlyLearning.ReadingPrograms.DataModels;

namespace EarlyLearning.ReadingPrograms
{
    public interface ReadingProgramManager
    {
        Task<IEnumerable<ReadingProgramInfo>> GetAllProgramsForUser(string userId);

        Task<bool> UserCanAccessProgram(string programId, string userId);

        Task<ReadingProgram<T>> GetReadingProgram<T>(string programId) where T : ReadingUnit;
    }
}