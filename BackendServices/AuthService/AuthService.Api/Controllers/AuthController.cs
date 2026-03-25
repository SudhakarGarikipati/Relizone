using AuthService.Application.DTOs;
using AuthService.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserAppService _userAppService;

        public AuthController(IUserAppService userAppService)
        {
            // Constructor logic can be added here if needed
            _userAppService = userAppService ?? throw new ArgumentNullException(nameof(userAppService));
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Invalid login request..!");
            }
            UserDto user = _userAppService.LoginUser(loginDto);
            if (user == null)
            {
                return BadRequest("Invalid email or password...!");
            }

            return Ok(user);
        }

        [HttpPost]
        public IActionResult SignUp([FromBody] SignUpDto signUpDto)
        {
            if (signUpDto == null)
            {
                return BadRequest("Inavalid SignUp details...!");
            }
            var isSuccess =  _userAppService.SignUpUser(signUpDto, signUpDto.Role);
            if (isSuccess) { 
                return Ok("User registered successfully...!");
            }
            else
            {
                return BadRequest("User registration failed...!");
            }
        }
    }
}
