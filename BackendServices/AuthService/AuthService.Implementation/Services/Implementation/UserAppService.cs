using AuthService.Application.DTOs;
using AuthService.Application.Repositories;
using AuthService.Application.Services.Abstractions;
using AuthService.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;


namespace AuthService.Application.Services.Implementation
{
    public class UserAppService : IUserAppService
    {
        readonly IUserRepository _repository;
        readonly IMapper _mapper;
        readonly IConfiguration _configuration;

        public UserAppService(IUserRepository repository, IMapper mapper, IConfiguration configuration)
        {
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        private string GenerateJwtToken(UserDto userDto)
        {
            // This method should generate a JWT token for the user.
            // Implementation is not provided here, as it depends on the specific JWT library and configuration used.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            int expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"]);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userDto.Name),
                new Claim(JwtRegisteredClaimNames.Email, userDto.Name),
                new Claim("Roles", string.Join("",userDto.Roles)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public IEnumerable<UserDto> GetAll()
        {
            var users = _repository.GetAll();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public UserDto GetUserByEmail(string mailId)
        {
            var user = _repository.GetUserByEmail(mailId);
            return _mapper.Map<UserDto>(user);
        }

        public UserDto LoginUser(LoginDto loginDto)
        {
            var user = _repository.GetUserByEmail(loginDto.Email);
            if(user != null)
            {
                var isMatching = BC.Verify(loginDto.Password, user.Password);
                if (isMatching)
                {
                    var userDto =  _mapper.Map<UserDto>(user);
                    userDto.Token = GenerateJwtToken(userDto);
                    return userDto;
                }
                else
                {
                    throw new UnauthorizedAccessException("Invalid password.");
                }
            }
            throw new UnauthorizedAccessException("User not found.");
        }

        public bool SignUpUser(SignUpDto signUpDto, string role)
        {
            var user = _repository.GetUserByEmail(signUpDto.Email);
            if (user != null)
            {
                throw new InvalidOperationException("User already exists.");
            }
            else
            {
                var newUser = _mapper.Map<User>(signUpDto);
                newUser.Password = BC.HashPassword(signUpDto.Password);
                return _repository.RegisterUser(newUser, role);
            }
        }
    }
}
