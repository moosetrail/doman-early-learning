using System.Threading.Tasks;
using Raven.Client.Documents;

namespace EarlyLearning.RavenDb
{
    public interface RavenDbDataPopulator
    {
        public Task EnsureDatabaseExistsAsync(IDocumentStore store, string database = null,
            bool createDatabaseIfNotExists = true);

        public Task Populate(IDocumentStore store);
    }
}