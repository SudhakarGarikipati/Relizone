using AuthService.Application.DTOs;

namespace AuthService.Application.Services.Abstractions
{
    public interface IUserAppService
    {
        public IEnumerable<UserDto> GetAll();

        public UserDto GetUserByEmail(string mailId);

        public UserDto LoginUser(LoginDto loginDto);

        public bool SignUpUser(SignUpDto signUpDto, string role);
    }
}
