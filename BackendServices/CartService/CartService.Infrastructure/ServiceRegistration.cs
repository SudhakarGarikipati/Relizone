using CartService.Application.Mappers;
using CartService.Application.Repositories;
using CartService.Application.Services.Abstraction;
using CartService.Application.Services.Implementation;
using CartService.Infrastructure.Persistance;
using CartService.Infrastructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CartService.Infrastructure
{
    public class ServiceRegistration
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register the DbContext
            services.AddDbContext<CartServiceDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbConnection")));
            // Register the repositories
            services.AddScoped<ICartReporitory, CartReporitory>();
            // Register the application services
            services.AddScoped<ICartAppService, CartAppService>();
            // Register AutoMapper
            services.AddAutoMapper(conf=>conf.AddProfile<CartServiceMapper>());
          
        }
    }
}
