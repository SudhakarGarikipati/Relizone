using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ReliZone.Support.Helpers
{
    public class CustomAuthorize : Attribute, IAuthorizationFilter
    {
        public string Roles { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", new { area= "User"});
            }
            else
            {
                if(context.HttpContext.User.IsInRole(Roles))
                {
                    // User is authorized, do nothing
                    return;
                }
                else
                {
                    // User is not authorized, redirect to access denied page
                    context.Result = new RedirectToActionResult("AccessDenied", "Account", new { area = "User" });
                }
            }
        }
    }
}
