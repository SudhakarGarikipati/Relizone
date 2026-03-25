using OrderService.Domain.Entities;

namespace OrderService.Application.Repositories
{
    public interface IOrderServiceRepository
    {
        List<Order> GetAllOrders();

        Task<bool> SaveOrder(Order order, long cartId);

        Order? GetOrderById(Guid orderId);

        Task<bool> AcceptedOrder(Guid orderId, DateTime AcceptedDateTime);

        Task<bool> DeleteOrder(Guid orderId);
    }
}
