using System.Threading.Tasks;
using Raven.Client.Documents;

namespace EarlyLearning.RavenDb.Setup
{
    public class EmptyRavenDbIndexManager: RavenDbIndexManager
    {
        public Task AddAllIndexes(IDocumentStore store)
        {
            return Task.CompletedTask;
        }
    }
}