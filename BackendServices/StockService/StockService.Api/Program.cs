using MassTransit;
using StockService.Api.Consumers;
using StockService.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);
ServiceRegistration.RegisterDependencies(builder.Services, builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure MassTransit with Azure Service Bus
builder.Services.AddMassTransit(config =>
{
    // Register the consumer
    config.AddConsumer<StockValidationConsumer>();

    // Configure Azure Service Bus
    config.UsingAzureServiceBus((ctx, cfg) =>
    {
        var connectionString = builder.Configuration.GetConnectionString("ServiceBus:ConnectionString");
        cfg.Host(connectionString);

        // Configure the endpoint for the consumer
        cfg.ConfigureEndpoints(ctx);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
