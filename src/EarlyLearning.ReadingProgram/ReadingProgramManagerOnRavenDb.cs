using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EarlyLearning.Core.DTOForRavenDb;
using EarlyLearning.ReadingPrograms.DataModels;
using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;
using EarlyLearning.ReadingPrograms.DataModels.ReadingUnits;
using EarlyLearning.ReadingPrograms.RavenDb;
using EarlyLearning.ReadingPrograms.RavenDb.DataTransferObjects;
using EarlyLearning.ReadingPrograms.RavenDb.Indexes;
using EarlyLearning.ReadingPrograms.RavenDb.ObjectMappers;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Serilog;

namespace EarlyLearning.ReadingPrograms
{
    public class ReadingProgramManagerOnRavenDb : ReadingProgramManager
    {
        private readonly IAsyncDocumentSession _session;
        private readonly ILogger _logger;

        public ReadingProgramManagerOnRavenDb(IAsyncDocumentSession session, ILogger logger)
        {
            _session = session;
            _logger = logger.ForContext<ReadingProgramManagerOnRavenDb>();
        }

        public async Task<IEnumerable<ReadingProgramInfo>> GetAllProgramsForUser(string userId)
        {
            var programDtoList = await _session.Query<ReadingProgram_ByUser.Result, ReadingProgram_ByUser>()
                .Where(x => x.UserIds.Any(u => u == userId)).OfType<ReadingProgramInfoDTO>().ToListAsync();

            var programList = programDtoList.Select(x => x.ToReadingProgramInfo());

            return programList;
        }

        public async Task<bool> UserCanAccessProgram(string programId, string userId)
        {
            var program = await _session.Include<ChildDTO>(x => x.Id).LoadAsync<ReadingProgramInfoDTO>(programId);
            var childrenOnProgram = await _session.LoadAsync<ChildDTO>(program.ChildrenIds);
            var userCanAccess = childrenOnProgram.Select(x => x.Value).SelectMany(x => x.Adults).Any(x => x == userId);
            return userCanAccess;
        }

        public async Task<ReadingProgram<T>> GetReadingProgram<T>(string programId) where T : ReadingUnit
        {
            var programInfo = await _session.LoadAsync<ReadingProgramInfoDTO>(programId);

            if (programInfo != null)
            {
                if (typeof(T) == typeof(ReadingCategory<ReadingWord>))
                {
                    return new SingleWordReadingProgramOnRavenDb(programId, _session, _logger) as ReadingProgram<T>;
                }
                else
                {
                    throw new NotSupportedException("The reading program is not yet supported by the software");
                }
            }

            return null;
        }
    }
}