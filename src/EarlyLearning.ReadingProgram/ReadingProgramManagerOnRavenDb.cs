using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EarlyLearning.ReadingPrograms.DataModels;
using EarlyLearning.ReadingPrograms.RavenDb.DataTransferObjects;
using EarlyLearning.ReadingPrograms.RavenDb.Indexes;
using EarlyLearning.ReadingPrograms.RavenDb.ObjectMappers;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace EarlyLearning.ReadingPrograms
{
    public class ReadingProgramManagerOnRavenDb : ReadingProgramManager
    {
        private readonly IAsyncDocumentSession _session;

        public ReadingProgramManagerOnRavenDb(IAsyncDocumentSession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<ReadingProgramInfo>> GetAllProgramsForUser(string userId)
        {
            var programDtoList = await _session.Query<ReadingProgram_ByUser.Result, ReadingProgram_ByUser>()
                .Where(x => x.UserIds.Any(u => u == userId)).OfType<ReadingProgramInfoDTO>().ToListAsync();

            var programList = programDtoList.Select(x => x.ToReadingProgramInfo());

            return programList;
        }
    }
}