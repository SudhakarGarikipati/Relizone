using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.Messages;
using OrderService.Application.Services.Abstraction;
using OrderStateMachine.Messages.Commands;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("api/[cotroller]/[action]")]
    public class OrderController : ControllerBase
    {
        readonly IOrderAppService _orderAppService;
        ISendEndpointProvider _sendEndpointProvider;
        IConfiguration _configuration;
        public OrderController(IOrderAppService orderAppService, ISendEndpointProvider sendEndpointProvider, IConfiguration configuration) {
            _orderAppService = orderAppService;
            _sendEndpointProvider = sendEndpointProvider;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderMessage model)
        {
            var orderDto = new Application.Dtos.OrderDto
            {
                PaymentId = model.PaymentId,
                UserId = model.UserId,
                OrderId = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow
            };

            await _orderAppService.SaveOrder(orderDto, model.CartId);

            // publish order creation message to RabbitMQ
            var uri = new Uri($"queue:{_configuration["RabbitMQ:OrderQueue"]}");
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(uri);
            await endpoint.Send<IOrderStart>(new 
            {
                orderDto.OrderId,
                orderDto.UserId,
                orderDto.PaymentId,
                model.Products,
                model.CartId
            });
            return Ok("Order Published.");
        }

    }
}
