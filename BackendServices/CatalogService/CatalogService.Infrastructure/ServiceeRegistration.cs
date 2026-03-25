using CatalogService.Application.Mappers;
using CatalogService.Application.Repositories;
using CatalogService.Application.Services.Abstractions;
using CatalogService.Application.Services.Implementations;
using CatalogService.Infrastructure.Persistance;
using CatalogService.Infrastructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Infrastructure
{
    public class ServiceeRegistration
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register application services
            services.AddScoped<IProductAppService, ProductAppService>();
            // Register domain repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            // Register AutoMapper
            services.AddAutoMapper(config => config.AddProfile<ProductMapper>());
            // Register DbContext
            services.AddDbContext<CatalogServiceDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbConnection")));
        }
    }
}
