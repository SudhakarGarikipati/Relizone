using OrderService.Application.Dtos;
using OrderService.Domain.Entities;

namespace OrderService.Application.Services.Abstraction
{
    public interface IOrderAppService
    {
        List<Order> GetAllOrders();

        Task<bool> SaveOrder(OrderDto order, long cartId);

        Order? GetOrderById(Guid orderId);

        Task<bool> AcceptedOrder(Guid orderId, DateTime AcceptedDateTime);

        Task<bool> DeleteOrder(Guid orderId);
    }
}
