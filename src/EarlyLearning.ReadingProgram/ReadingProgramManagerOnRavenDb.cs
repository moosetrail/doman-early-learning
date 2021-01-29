using System.Collections.Generic;
using System.Threading.Tasks;
using EarlyLearning.ReadingPrograms.DataModels;

namespace EarlyLearning.ReadingPrograms
{
    public class ReadingProgramManagerOnRavenDb : ReadingProgramManager
    {
        public ReadingProgramManagerOnRavenDb()
        {

        }

        public Task<IEnumerable<ReadingProgramInfo>> GetAllProgramsForUser(string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}