using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Exceptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Serilog;

namespace EarlyLearning.RavenDb.Setup
{
    public class DevelopmentRavenDbSetup : RavenDbDataPopulator
    {
        private readonly RavenDbIndexManager _indexManager;
        private readonly ILogger _logger;

        public DevelopmentRavenDbSetup(RavenDbIndexManager indexManager, ILogger logger)
        {
            _indexManager = indexManager;
            _logger = logger.ForContext<DevelopmentRavenDbSetup>();
        }

        public async Task EnsureDatabaseExistsAsync(IDocumentStore store, string database = null, bool createDatabaseIfNotExists = true)
        {
            database ??= store.Database;

            if (string.IsNullOrWhiteSpace(database))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));

            try
            {
                await store.Maintenance.ForDatabase(database).SendAsync(new GetStatisticsOperation());
            }
            catch (DatabaseDoesNotExistException)
            {
                if (createDatabaseIfNotExists == false)
                    throw;

                try
                {
                    await store.Maintenance.Server.SendAsync(new CreateDatabaseOperation(new DatabaseRecord(database)));
                }
                catch (ConcurrencyException)
                {
                }
            }
        }

        public async Task Populate(IDocumentStore store)
        {
          
        }

        public async Task<IDocumentStore> Create(string url)
        {
            var store = await CreateWithoutData(url);
            await Populate(store);

            return store;
        }

        public async Task<IDocumentStore> CreateWithoutData(string url)
        {
            var setup = new GenericRavenDbSetup(_logger, _indexManager);
            var dbName = "FunToLearn-Dev";

            var store = setup.InitializeDatastore(dbName, url);
            await EnsureDatabaseExistsAsync(store, dbName);

            await setup.AddIndexesToDatabase(store);
            return store;
        }
    }
}