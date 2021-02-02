using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Serilog;

namespace EarlyLearning.RavenDb.Setup
{
    internal class GenericRavenDbSetup
    {
        private readonly ILogger _logger;
        private readonly RavenDbIndexManager _indexManager;

        public GenericRavenDbSetup(ILogger logger, RavenDbIndexManager indexManager)
        {
            _logger = logger;
            _indexManager = indexManager;
        }

        public IDocumentStore InitializeDatastore(string dbName, string url, X509Certificate2 clientCertificate = null)
        {
            try
            {
                var store = new DocumentStore
                {
                    Certificate = clientCertificate,
                    Urls = new[]
                    {
                        url
                    },
                    Database = dbName
                };
                _logger.Information("Setup connection to RavenDb on urls {urls}", store.Urls);
                _logger.Information("Set the default database to {databaseName}", store.Database);

                if(clientCertificate != null)
                    _logger.Information("Used certificate to authenticate with RavenDb server");

                store.Initialize();
                _logger.Information("RavenDb initialized");

                return store;
            }
            catch (Exception e)
            {
                _logger.Fatal("Couldn't connect to the database", e);
                throw;
            }
        }

        public async Task AddIndexesToDatabase(IDocumentStore store)
        {
            try
            {
                await _indexManager.AddAllIndexes(store);
                _logger.Information("RavenDb indexes created");
            }
            catch (Exception e)
            {
                _logger.Fatal("Failed to add indexes to database", e);
                throw;
            }
        }
    }
}