using Microsoft.AspNetCore.Mvc;
using ReliZone.Web.Models;
using System.Security.Claims;
using System.Text.Json;

namespace ReliZone.Web.Controllers
{
    public class BaseController : Controller
    {
        public UserModel CurrentUser
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userData = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;
                    if (!string.IsNullOrEmpty(userData))
                    {
                        return JsonSerializer.Deserialize<UserModel>(userData, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            WriteIndented = true
                        });
                    }
                }
                return null;
            }
        }
    }
}
