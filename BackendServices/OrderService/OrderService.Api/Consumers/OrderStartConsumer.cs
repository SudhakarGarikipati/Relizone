using MassTransit;
using OrderStateMachine.Messages.Commands;
using OrderStateMachine.Messages.Events;

namespace OrderService.Api.Consumers
{
    public class OrderStartConsumer : IConsumer<IOrderStart>
    {
        public async Task Consume(ConsumeContext<IOrderStart> context)
        {
            await context.Publish<IOrderStarted>(new
            {
                OrderId = context.Message.OrderId,
                PaymentId = context.Message.PaymentId,
                CartId = context.Message.CartId,
                Products = context.Message.Products
            }
            );
        }
    }
}
