using System.Collections.Generic;
using System.Threading.Tasks;
using EarlyLearning.Core.Program;
using EarlyLearning.ReadingPrograms.DataModels;

namespace EarlyLearning.ReadingPrograms
{
    public interface ReadingProgram<T> where T: ReadingUnit
    {
        public string ProgramId { get; }

        Task<IEnumerable<T>> GetCurrent();

        Task<IEnumerable<T>> GetPlanned(int limit, int offset);

        Task<IEnumerable<T>> GetRetired(int limit, int offset);

        Task Add(T toAdd);

        Task ChangeStatus(string unitId, ActivityStatus newStatus);

        Task MovePlanned(string unitId, int toSpot);
    }
}