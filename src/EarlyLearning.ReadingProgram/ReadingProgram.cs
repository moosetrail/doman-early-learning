using System.Collections.Generic;
using System.Threading.Tasks;
using EarlyLearning.Core.Program;
using EarlyLearning.ReadingPrograms.DataModels;

namespace EarlyLearning.ReadingPrograms
{
    public interface ReadingProgram<T> where T: ReadingUnit
    {
        Task<IEnumerable<T>> GetCurrent(string programId);

        Task<IEnumerable<T>> GetPlanned(string programId, int limit, int offset);

        Task<IEnumerable<T>> GetRetired(string programId, int limit, int offset);

        Task Add(T toAdd, string programId);

        Task ChangeStatus(string unitId, ActivityStatus newStatus);

        Task MovePlanned(string unitId, string programId, int toSpot);
    }
}