using Raven.Client.Documents;
using Raven.TestDriver;

namespace EarlyLearning.Tests.TestHelpers.RavenDb
{
    public class RavenDBTestDriver : RavenTestDriver
    {
        //This allows us to modify the conventions of the store we get from 'GetDocumentStore'
        protected override void PreInitialize(IDocumentStore documentStore)
        {
            documentStore.Conventions.MaxNumberOfRequestsPerSession = 30;
        }

        public IDocumentStore GetStore()
        {
            return GetDocumentStore();
        }

        public void WaitIndexing(IDocumentStore store)
        {
            WaitForIndexing(store);
        }
    }
}