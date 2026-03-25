using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Api.Consumers;
using OrderService.Domain.Entities;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Persistance;
using OrderStateMachine.StateMachine;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ServiceRegistration.RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure MassTransit with Azure Service Bus
builder.Services.AddMassTransit(config =>
{
    // Register state machine consumers
    config.AddConsumer<OrderStartConsumer>();
    config.AddConsumer<OrderCancelledConsumer>();
    config.AddConsumer<OrderAcceptedConsumer>();

    // configure statemachine saga repository
    config.AddSagaStateMachine<OrderMachine, OrderStateMachine.Database.Entities.OrderState>()
    .EntityFrameworkRepository(r =>
    {
        r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
        r.AddDbContext<DbContext, OrderServiceDbContext>((provider, options) =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
        });
    });

    config.UsingAzureServiceBus((ctx, cfg) =>
    {
        var connectionString = builder.Configuration.GetConnectionString("ServiceBus:ConnectionString");
        cfg.Host(connectionString);

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
