using Microsoft.AspNetCore.Mvc.Razor;
using ReliZone.Web.Models;
using System.Security.Claims;
using System.Text.Json;

namespace ReliZone.Web.Helpers
{
    public abstract class BasePageView<TModel> : RazorPage<TModel>
    {
        public UserModel CurrentUser
        {
            get
            {
                if(User.Identity.IsAuthenticated)
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
