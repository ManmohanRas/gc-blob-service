using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace morris_azstorage_service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration((context, config) =>
                 {
                     if (context.HostingEnvironment.IsProduction())
                     {
                         var builder = config.Build();
                         var keyVaultEndpoint = builder["AzureKeyVaultEndpoint"];
                         if (!string.IsNullOrEmpty(keyVaultEndpoint))
                         {
                             var connectionString = "RunAs=App;AppId=4984cde5-1090-4c4e-b141-4ec910038143;TenantId=ec2ce8c1-3839-4008-b949-e2b17cfa906d;AppKey=Np04aZ5IS.Hapf.99re._zQ~9R8_pfZtxN";

                             var azureServiceTokenProvider = new AzureServiceTokenProvider(connectionString);
                             var keyVaultClient = new KeyVaultClient(
                                 new KeyVaultClient.AuthenticationCallback(
                                     azureServiceTokenProvider.KeyVaultTokenCallback));

                             config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
                         }
                     }
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
