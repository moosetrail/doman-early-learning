using System.Collections.Generic;
using System.Threading.Tasks;
using EarlyLearning.Core.People;
using EarlyLearning.People;
using Raven.Client.Documents.Session;
using Serilog;

namespace EarlyLearning.Core.RavenDb
{
    public class ChildManagerOnRavenDb : ChildManager
    {
        private readonly IAsyncDocumentSession _session;
        private readonly ILogger _logger;

        public ChildManagerOnRavenDb(IAsyncDocumentSession session, ILogger logger)
        {
            _session = session;
            _logger = logger.ForContext<ChildManagerOnRavenDb>();
        }

        public Task<Child> AddChildWithAdult(string firstName, string lastname, string adultId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Child>> GetChildrenForAdult(string adultId)
        {
            throw new System.NotImplementedException();
        }
    }
}