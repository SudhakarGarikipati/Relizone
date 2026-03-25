using AuthService.Application.Mappers;
using AuthService.Application.Repositories;
using AuthService.Application.Services.Abstractions;
using AuthService.Application.Services.Implementation;
using AuthService.Infrastructure.Persistance;
using AuthService.Infrastructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure
{
    public class ServiceRegistration
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register your services here
            // Example: services.AddScoped<IUserService, UserService>();
            // Example: services.AddDbContext<AuthServiceDbContext>(options => options.UseSqlServer("YourConnectionString"));

            // Add Db Context
            services.AddDbContext<AuthServiceDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbConnection")));

            services.AddAutoMapper(config => config.AddProfile<AuthMapper>());

            // Add other necessary registrations like AutoMapper, MediatR, etc.
            services.AddScoped<IUserAppService, UserAppService>();

            // Register other services as needed
            services.AddScoped<IUserRepository, UserRepository>();

        }
    }
}
