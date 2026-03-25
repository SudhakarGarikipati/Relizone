using MassTransit;
using OrderService.Application.Services.Abstraction;
using OrderStateMachine.Messages.Commands;
using OrderStateMachine.Messages.Events;
using System.Collections.Concurrent;

namespace OrderService.Api.Consumers
{
    public class OrderCancelledConsumer : IConsumer<IOrderCancelled>
    {
        private readonly IOrderAppService _orderapiService;

        public OrderCancelledConsumer(IOrderAppService orderAppService)
        {
            _orderapiService = orderAppService;
        }

        public async Task Consume(ConsumeContext<IOrderCancelled> context)
        {
           var order = context.Message;
            await _orderapiService.DeleteOrder(order.OrderId);
        }
    }
}
