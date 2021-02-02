using System.Threading.Tasks;
using Raven.Client.Documents;

namespace EarlyLearning.RavenDb
{
    public interface RavenDbIndexManager
    {
        Task AddAllIndexes(IDocumentStore store);
    }
}