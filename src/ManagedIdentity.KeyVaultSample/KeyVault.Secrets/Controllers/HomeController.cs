using KeyVault.Secrets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KeyVault.Secrets.Controllers
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
                var vaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));
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

        private async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            var vaultClientId = _configuration["AppSettings:KeyVaultClientId"];
            var vaultClientSecret = _configuration["AppSettings:KeyVaultClientSecret"];

            var authContext = new AuthenticationContext(authority);
            var clientCreds = new ClientCredential(vaultClientId, vaultClientSecret);
            var authResult = await authContext.AcquireTokenAsync(resource, clientCreds);

            return authResult.AccessToken;
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