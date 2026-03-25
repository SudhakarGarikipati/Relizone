using AuthService.Application.Repositories;
using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        // This is a placeholder implementation of the IAuthRepository interface.
        readonly AuthServiceDbContext _context;

        public UserRepository(AuthServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public bool RegisterUser(User user, string roleName)
        {
            var userRole = _context.Roles.FirstOrDefault(r => r.Name == roleName);
            if (userRole == null)
            {
                throw new KeyNotFoundException($"Role {roleName} not found.");
            }
            user.Roles.Add(userRole);
            _context.Users.Add(user);
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("An error occurred while registering the user.", ex);
            }
        }

        public IEnumerable<User> GetAll()
        {
            return [.. _context.Users];
        }

        public User GetUserByEmail(string mailId)
        {
            return _context.Users.Include(u=>u.Roles).FirstOrDefault(u => u.Email == mailId);
        }

    }
}
