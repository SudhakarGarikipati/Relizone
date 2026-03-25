using MassTransit;
using OrderService.Application.Services.Abstraction;
using OrderStateMachine.Messages.Events;

namespace OrderService.Api.Consumers
{
    public class OrderAcceptedConsumer : IConsumer<IOrderAccepted>
    {
        private readonly IOrderAppService _orderapiService;
        public OrderAcceptedConsumer(IOrderAppService orderAppService)
        {
            _orderapiService = orderAppService;
        }

        public async Task Consume(ConsumeContext<IOrderAccepted> context)
        {
            var order = context.Message;
            await _orderapiService.AcceptedOrder(order.OrderId, order.AcceptedDateTime);
            throw new NotImplementedException();
        }
    }
}
