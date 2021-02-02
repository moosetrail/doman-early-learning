using System;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Serilog;

namespace EarlyLearning.RavenDb.Setup
{
    public class ProductionRavenDbSetup
    {
        private readonly RavenDbIndexManager _indexManager;
        private readonly ILogger _logger;

        public ProductionRavenDbSetup(RavenDbIndexManager indexManager, ILogger logger)
        {
            _indexManager = indexManager;
            _logger = logger.ForContext<DevelopmentRavenDbSetup>();
        }

        public async Task<IDocumentStore> Create(string url, string certSecret)
        {
            var certificate = ConstructCertificate(certSecret);
            return await createStoreWithCert(url, certificate);
        }

        private async Task<IDocumentStore> createStoreWithCert(string url, X509Certificate2 certificate)
        {
            var setup = new GenericRavenDbSetup(_logger, _indexManager);
            var store = setup.InitializeDatastore("FunToLearn", url, certificate);
            await setup.AddIndexesToDatabase(store);

            return store;
        }

        private X509Certificate2 ConstructCertificate(string certSecret)
        {
            if (string.IsNullOrWhiteSpace(certSecret))
            {
                Log.Error("Couldn't load a certificate for database");
            }
            else
            {
                Log.Debug("Found raven-db certificate");
            }

            var privateCertBytes = Convert.FromBase64String(certSecret);
            var clientCertificate = new X509Certificate2(privateCertBytes, (string)null,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            return clientCertificate;
        }

        public async Task<IDocumentStore> CreateWithCertFile(string url, string filePath, SecureString certPassword)
        {
               var clientCertificate = new X509Certificate2(filePath, certPassword,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
               return await createStoreWithCert(url, clientCertificate);
        }
    }
}