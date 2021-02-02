
using System.Collections.Generic;
using System.Threading.Tasks;
using EarlyLearning.ReadingPrograms.DataModels;

namespace EarlyLearning.ReadingPrograms
{
    public interface ReadingProgram<T> where T: ReadingUnit
    {
        Task<IEnumerable<T>> GetCurrent(string programId);

        Task<IEnumerable<T>> GetPlanned(string programId, int limit, int offset);

        Task<IEnumerable<T>> GetRetired(string programId, int limit, int offset);
    }
}