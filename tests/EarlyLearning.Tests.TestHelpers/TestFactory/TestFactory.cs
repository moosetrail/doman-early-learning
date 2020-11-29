using System;
using Moosetrail.EarlyLearning.Tests.TestHelpers.RavenDb;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;

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

        public IDocumentStore GenerateNewDocumentStore(bool includeAllIndexes = false)
        {
            RavenDbTestDriver ??= new RavenDBTestDriver();
            DocumentStore = RavenDbTestDriver.GetStore();

            if (includeAllIndexes)
            {
                AddAllIndexes(DocumentStore);
                WaitOfIndexesInDocumentStore();
            }

            return DocumentStore;
        }

        public IDocumentStore GenerateNewDocumentStore(params AbstractIndexCreationTask[] indexesToCreate)
        {
            RavenDbTestDriver ??= new RavenDBTestDriver();
            DocumentStore = RavenDbTestDriver.GetStore();

            IndexCreation.CreateIndexes(indexesToCreate, DocumentStore);
            WaitOfIndexesInDocumentStore();

            return DocumentStore;
        }

        private static void AddAllIndexes(IDocumentStore store)
        {
            // IndexCreation.CreateIndexes(typeof(School_ByNameOfSchoolWithId).Assembly, store);
        }

        public void WaitOfIndexesInDocumentStore()
        {
            RavenDbTestDriver.WaitIndexing(DocumentStore);
        }

        public static string[] EmptyStrings = {"", "  ", null};
    }
}