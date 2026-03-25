using AuthService.Domain.Entities;

namespace AuthService.Application.Repositories
{
    public interface IUserRepository
    {
        public IEnumerable<User> GetAll();

        public User GetUserByEmail(string mailId);

        public bool RegisterUser(User user, string role);

    }
}
