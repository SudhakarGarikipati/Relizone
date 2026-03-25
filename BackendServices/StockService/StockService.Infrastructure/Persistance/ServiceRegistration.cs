using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockService.Application.Mapper;
using StockService.Application.Repositories;
using StockService.Application.Services.Abstraction;
using StockService.Application.Services.Implementation;
using StockService.Infrastructure.Persistance.Repositories;

namespace StockService.Infrastructure.Persistance
{
    public class ServiceRegistration
    {
        public static void RegisterDependencies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(config =>
            {
               config.AddProfile<StockMapper>();
            });
            // Register your services here
            services.AddDbContext<StockServiceDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DbConnection");
                options.UseSqlServer(connectionString);
            });
            services.AddScoped<IStockAppService, StockAppService>();
            services.AddScoped<IStockServiceRepository, StockServiceRepository>();
        }
    }
}
