
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;

namespace morris_azstorage_service.Helpers
{
    public class AzureStorageBlobOptionsTokenGenerator
    {
       public IConfiguration Configuration { get; }


        public AzureStorageBlobOptionsTokenGenerator(IConfiguration configuration)
        {
             Configuration = configuration;
        }

        [Test]
        public async Task ConnectionStringAsync()
        {
            // Get a connection string to our Azure Storage account.  You can
            // obtain your connection string from the Azure Portal (click
            // Access Keys under Settings in the Portal Storage account blade)
            // or using the Azure CLI with:
            //
            //     az storage account show-connection-string --name <account_name> --resource-group <resource_group>
            //
            // And you can provide the connection string to your application
            // using an environment variable.
            string ConnectionString = Configuration["AzureStorageBlobOptions:MCPRIMAConnectionString"];

            // Create a client that can authenticate with a connection string
            BlobServiceClient service = new BlobServiceClient(ConnectionString);

            // Make a service request to verify we've successfully authenticated
            await service.GetPropertiesAsync();
        }

 

        /// <summary>
        /// Use a shared key to access a Storage Account.
        ///
        /// Shared Key authorization relies on your account access keys and
        /// other parameters to produce an encrypted signature string that is
        /// passed on the request in the Authorization header.
        /// </summary>
        [Test]
        public async Task SharedKeyAuthAsync()
        {
            // Get a Storage account name, shared key, and endpoint Uri.
            //
            // You can obtain both from the Azure Portal by clicking Access
            // Keys under Settings in the Portal Storage account blade.
            //
            // You can also get access to your account keys from the Azure CLI
            // with:
            //
            //     az storage account keys list --account-name <account_name> --resource-group <resource_group>
            //
            string accountName = Configuration["AzureStorageBlobOptions:AccountName"];
            string accountKey = Configuration["AzureStorageBlobOptions:AccountKey"];
            Uri serviceUri = new Uri(Configuration["AzureStorageBlobOptions:FilePath"]);

            // Create a SharedKeyCredential that we can use to authenticate
            StorageSharedKeyCredential credential = new StorageSharedKeyCredential(accountName, accountKey);

            // Create a client that can authenticate with a connection string
            BlobServiceClient service = new BlobServiceClient(serviceUri, credential);

            // Make a service request to verify we've successfully authenticated
            await service.GetPropertiesAsync();
        }

        /// <summary>
        /// Use a shared access signature to acces a Storage Account.
        ///
        /// A shared access signature (SAS) is a URI that grants restricted
        /// access rights to Azure Storage resources. You can provide a shared
        /// access signature to clients who should not be trusted with your
        /// storage account key but to whom you wish to delegate access to
        /// certain storage account resources. By distributing a shared access
        /// signature URI to these clients, you can grant them access to a
        /// resource for a specified period of time, with a specified set of
        /// permissions.
        /// </summary>
        [Test]
        public async Task SharedAccessSignatureAuthAsync()
        {
            string StorageAccountName = Configuration["AzureStorageBlobOptions:AccountName"];
            string StorageAccountKey = Configuration["AzureStorageBlobOptions:AccountKey"];
           
           // Create a service level SAS that only allows reading from service
            // level APIs
            AccountSasBuilder sas = new AccountSasBuilder
            {
           
          
                // Allow access to blobs
                Services = AccountSasServices.Blobs,

                // Allow access to the service level APIs
                ResourceTypes = AccountSasResourceTypes.Service,

                // Access expires in 1 hour!
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };
            // Allow read access
            sas.SetPermissions(AccountSasPermissions.All);

            // Create a SharedKeyCredential that we can use to sign the SAS token
            StorageSharedKeyCredential credential = new StorageSharedKeyCredential(StorageAccountName, StorageAccountKey);

            // Build a SAS URI
            UriBuilder sasUri = new UriBuilder(Configuration["AzureStorageBlobOptions:FilePath"]);
            sasUri.Query = sas.ToSasQueryParameters(credential).ToString();

            // Create a client that can authenticate with the SAS URI
            BlobServiceClient service = new BlobServiceClient(sasUri.Uri);

            // Make a service request to verify we've successfully authenticated
            await service.GetPropertiesAsync();

            // Try to create a new container (which is beyond our
            // delegated permission)
            RequestFailedException ex =
                Assert.ThrowsAsync<RequestFailedException>(
                    async () => await service.CreateBlobContainerAsync("sample-container"));
                    

            Assert.AreEqual(403, ex.Status);
        }


    }
}
