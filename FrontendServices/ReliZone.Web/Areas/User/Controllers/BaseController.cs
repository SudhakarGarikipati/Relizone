using Microsoft.AspNetCore.Mvc;
using ReliZone.Web.Helpers;

namespace ReliZone.Web.Areas.User.Controllers
{
    [CustomAuthorize(Roles = "User")]
    [Area("User")]
    public class BaseController : Controller
    {
        
    }
}
