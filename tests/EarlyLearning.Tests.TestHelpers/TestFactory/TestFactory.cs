using System;
using System.Threading.Tasks;
using EarlyLearning.RavenDb.Setup;
using EarlyLearning.Tests.TestHelpers.RavenDb;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;

namespace EarlyLearning.Tests.TestHelpers.TestFactory
{
    public partial class TestFactory
    {
        private Random random;

        public TestFactory()
        {
            random = new Random();
        }

        public TestFactory(bool setupRavenDb)
        {
            if (setupRavenDb)
            {
                GenerateNewDocumentStore();
            }
        }

        public RavenDBTestDriver RavenDbTestDriver { get; set; }

        public IDocumentStore DocumentStore { get; set; }

        public IAsyncDocumentSession ActiveSession { get; set; }

        public IDocumentStore GenerateNewDocumentStore(bool includeAllIndexes = false)
        {
            RavenDbTestDriver ??= new RavenDBTestDriver();
            DocumentStore = RavenDbTestDriver.GetStore();
            TestLogger().Debug("Creating a new RavenDb store");

            if (includeAllIndexes)
            {
                AddAllIndexes(DocumentStore);
                WaitOfIndexesInDocumentStore();
            }

            ActiveSession = DocumentStore.OpenAsyncSession();

            return DocumentStore;
        }

        public IDocumentStore GenerateNewDocumentStore(params AbstractIndexCreationTask[] indexesToCreate)
        {
            RavenDbTestDriver ??= new RavenDBTestDriver();
            DocumentStore = RavenDbTestDriver.GetStore();
            TestLogger().Debug("Creating a new RavenDb store");

            IndexCreation.CreateIndexes(indexesToCreate, DocumentStore);
            WaitOfIndexesInDocumentStore();

            ActiveSession = DocumentStore.OpenAsyncSession();

            return DocumentStore;
        }

        private static void AddAllIndexes(IDocumentStore store)
        {
           var indexManager = new StandardRavenDbIndexManager();
           Task.WaitAll(indexManager.AddAllIndexes(store));
        }

        public void WaitOfIndexesInDocumentStore()
        {
            TestLogger().Debug("Waiting for indexes in RavenDb");
            RavenDbTestDriver.WaitIndexing(DocumentStore);
        }

        public static string[] EmptyStrings = {"", "  ", null};
    }
}