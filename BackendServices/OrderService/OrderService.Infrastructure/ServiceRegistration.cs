using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Profiles;
using OrderService.Application.Repositories;
using OrderService.Application.Services.Abstraction;
using OrderService.Application.Services.Implementation;
using OrderService.Infrastructure.Persistance;
using OrderService.Infrastructure.Persistance.Repositories;

namespace OrderService.Infrastructure
{
    public class ServiceRegistration
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register your services here
            services.AddScoped<IOrderServiceRepository, OrderServiceRepository>();
            services.AddScoped<IOrderAppService, OrderAppService>();
            services.AddDbContext<OrderServiceDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DbConnection");
                options.UseSqlServer(connectionString);
            });
            services.AddAutoMapper(config =>
            {
                config.AddProfile<OrderServiceMapper>();
            }); 
        }
    }
}
