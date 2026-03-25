using Microsoft.AspNetCore.Mvc;
using ReliZone.Web.HttpClients;
using ReliZone.Web.Models;
using System.Diagnostics;

namespace ReliZone.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CatalogServiceClient _catelogServiceClient;

        public HomeController(ILogger<HomeController> logger, CatalogServiceClient catalogServiceClient)
        {
            _logger = logger;
            _catelogServiceClient = catalogServiceClient ?? throw new ArgumentNullException(nameof(catalogServiceClient));
        }

        public IActionResult Index()
        {
            var products = _catelogServiceClient.GetAllProductAsync().Result;
            if(products == null || !products.Any())
            {
                return NotFound("No products found.");
            }
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
