using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Mappers;
using PaymentService.Application.Repositories;
using PaymentService.Application.Services.Abstraction;
using PaymentService.Application.Services.Implementation;
using PaymentService.Infrastructure.Persistance;
using PaymentService.Infrastructure.Persistance.Repositories;
using PaymentService.Infrastructure.Providers.Abstraction;
using PaymentService.Infrastructure.Providers.Implementation;

namespace PaymentService.Infrastructure
{
    public class ServiceRegistration
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // Add infrastructure services here
            services.AddScoped<IPaymentAppService, PaymentAppService>();
            services.AddScoped<IPaymentServiceRepository, PaymentServiceRepository>();
            services.AddScoped<IPaymentProvider, PaymentProvider>();
            services.AddAutoMapper(config => config.AddProfile<PaymentMapper>());
            services.AddDbContext<PaymentServiceDbContext>(options =>
            {
                options.UseSqlServer( configuration.GetConnectionString("DbConnection"));
            });
        }
    }   
}
