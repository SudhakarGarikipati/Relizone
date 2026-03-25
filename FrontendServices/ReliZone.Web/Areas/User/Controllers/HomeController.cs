using Microsoft.AspNetCore.Mvc;

namespace ReliZone.Web.Areas.User.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
