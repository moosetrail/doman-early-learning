using System.Collections.Generic;
using System.Threading.Tasks;
using EarlyLearning.Core.Program;
using EarlyLearning.ReadingPrograms.DataModels;
using Raven.Client.Documents.Session;
using Serilog;

namespace EarlyLearning.ReadingPrograms.RavenDb
{
    public abstract class ReadingProgramOnRavenDb<T>: ReadingProgram<T> where T : ReadingUnit
    {
        private readonly ILogger _logger;
        private readonly IAsyncDocumentSession _session;

        protected ReadingProgramOnRavenDb(string programId, IAsyncDocumentSession session, ILogger logger)
        {
            _session = session;
            _logger = logger;
            ProgramId = programId;
        }

        public string ProgramId { get; }

        public async Task<IEnumerable<T>> GetCurrent()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetPlanned(int limit, int offset)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetRetired(int limit, int offset)
        {
            throw new System.NotImplementedException();
        }

        public async Task Add(T toAdd)
        {
            throw new System.NotImplementedException();
        }

        public async Task ChangeStatus(string unitId, ActivityStatus newStatus)
        {
            throw new System.NotImplementedException();
        }

        public async Task MovePlanned(string unitId, int toSpot)
        {
            throw new System.NotImplementedException();
        }
    }
}