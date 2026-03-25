using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ReliZone.Web.HttpClients;
using ReliZone.Web.Models;
using System.Security.Claims;
using System.Text.Json;

namespace ReliZone.Web.Controllers
{
    public class AccountController : Controller
    {
        readonly AuthServiceClient _authServiceClient;

        public AccountController(AuthServiceClient authServiceClient)
        {
            _authServiceClient = authServiceClient ?? throw new ArgumentNullException(nameof(authServiceClient));
        }

        public IActionResult Login()
        {
            return View();
        }

        private void GenerateTicket(UserModel user)
        {
            // This method is a placeholder for generating authentication tickets.
            // In a real application, you would implement logic to create and sign an authentication ticket.
            // For example, you might use ASP.NET Core's authentication middleware to create a cookie or JWT token.
            // This could involve setting claims, expiration, and other properties of the ticket.
            // Example:
            // var claims = new List<Claim>
            // {
            //     new Claim(ClaimTypes.Name, userModel.Username),
            //     new Claim(ClaimTypes.Role, userModel.Role)
            // };
            // var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            // var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            // await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            string strData =  JsonSerializer.Serialize(user, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.UserData, strData),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, string.Join(",", user.Roles))
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = true, // This will keep the user logged in across sessions
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30) // Set the expiration time for the cookie
            };

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authenticationProperties);

        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                    var user = _authServiceClient.LoginAsync(loginModel).Result;
                if (user != null)
                {
                    GenerateTicket(user);
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl); 
                    }
                    else if(user.Roles.Contains("User"))
                    {
                        return RedirectToAction("Index", "Home", new {area="User"});
                    }
                    else if(user.Roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(loginModel);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(SignUpModel signUpModel)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var isSuccess = _authServiceClient.Register(signUpModel).Result;
                    if (isSuccess)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }
            return RedirectToAction("Login", "Account");
        }
    }
}
