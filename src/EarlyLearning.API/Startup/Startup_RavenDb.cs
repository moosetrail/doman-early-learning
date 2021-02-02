using System.Threading.Tasks;
using EarlyLearning.RavenDb;
using EarlyLearning.RavenDb.Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raven.Client.Documents;
using Serilog;

namespace EarlyLearning.API
{
    public partial class Startup
    {
        private async Task  ConfigureRavenDb(IServiceCollection services)
        {
            IDocumentStore store = null;
            RavenDbIndexManager indexManager = new StandardRavenDbIndexManager();

            if (_env.IsDevelopment())
            {
                _logger.Information("Setup RavenDb for Development");
                var databaseCreator = new DevelopmentRavenDbSetup(indexManager, _logger);
                store = await databaseCreator.Create(Configuration.GetValue<string>("RavenDbUrl"));
            }

            if (_env.IsStaging())
            {
                _logger.Information("Setup RavenDb for Staging");
                var databaseCreator = new ProductionRavenDbSetup(indexManager, _logger);
                store = await databaseCreator.Create(Configuration["RavenDbUrl"],
                    Configuration["RavenDbStagingApiCert"]);
            }

            if (_env.IsProduction())
            {
                _logger.Information("Setup RavenDb for Production");
                indexManager = new EmptyRavenDbIndexManager();
                var databaseCreator = new ProductionRavenDbSetup(indexManager, _logger);
                store = await databaseCreator.Create(Configuration["RavenDbUrl"],
                    Configuration["RavenDbProductionApiCert"]);
            }

            if (store == null)
            {
                _logger.Information("RavenDb is not setup");
                return;
            }

            Log.Information("Add RavenDocumentStore as Singleton");
            services.AddSingleton(store);
            services.AddScoped(sp => store.OpenAsyncSession());
        }
    }
}