using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace ManagedIdentity.KeyVault.Configuration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((ctx, config) =>
                {
                    if (ctx.HostingEnvironment.IsProduction())
                    {
                        var configRoot = config.Build();
                        var tokenProvider = new AzureServiceTokenProvider();

                        config.AddAzureKeyVault(
                            $"https://{configRoot["AppSettings:KeyVaultName"]}.vault.azure.net/",
                            new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback)),
                            new DefaultKeyVaultSecretManager());
                    }

                })
                .UseStartup<Startup>();
    }
}
