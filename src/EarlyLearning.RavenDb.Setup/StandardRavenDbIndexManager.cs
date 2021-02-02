using System.Threading.Tasks;
using EarlyLearning.ReadingPrograms.RavenDb.Indexes;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;

namespace EarlyLearning.RavenDb.Setup
{
    public class StandardRavenDbIndexManager : RavenDbIndexManager
    {
        public async Task AddAllIndexes(IDocumentStore store)
        {
            await IndexCreation.CreateIndexesAsync(typeof(ReadingProgram_ByUser).Assembly, store);
        }
    }
}