using ManagedIdentity.KeyVault.Configuration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ManagedIdentity.KeyVault.Configuration.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var vm = new HomeViewModel
            {
                Title = _configuration["AppSettings:HomePage:Title"],
                Description = _configuration["AppSettings:HomePage:Description"]
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}