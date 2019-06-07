using ManagedIdentity.KeyVault.Secrets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ManagedIdentity.KeyVault.Secrets.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var vaultName = _configuration["AppSettings:KeyVaultName"];
            var secretName = _configuration["AppSettings:SecretName"];

            var vm = new HomeViewModel();

            try
            {

                // Creating the Key Vault client
                var tokenProvider = new AzureServiceTokenProvider();
                var vaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));

                // Get the secret
                var secret = await vaultClient.GetSecretAsync($"https://{vaultName}.vault.azure.net/secrets/{secretName}");



                vm.SecretValue = secret.Value;
            }
            catch (Exception ex)
            {
                vm.IsError = true;
                vm.ErrorMessage = ex.Message;
            }

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}