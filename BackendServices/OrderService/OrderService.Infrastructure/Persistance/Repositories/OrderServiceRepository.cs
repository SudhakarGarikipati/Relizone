using OrderService.Application.Repositories;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistance.Repositories
{
    public class OrderServiceRepository : IOrderServiceRepository
    {
        readonly OrderServiceDbContext _orderServiceDbContext;

        public OrderServiceRepository(OrderServiceDbContext orderServiceDbContext)
        {
            this._orderServiceDbContext = orderServiceDbContext;
        }

        public Task<bool> AcceptedOrder(Guid orderId, DateTime AcceptedDateTime)
        {
            try
            {
                var order = _orderServiceDbContext.Orders.FirstOrDefault(o => o.OrderId == orderId);
                if (order == null)
                {
                    throw new Exception("Order not found.");
                }
                else
                {
                    order.AcceptDate = AcceptedDateTime;
                    _orderServiceDbContext.Orders.Update(order);
                    _orderServiceDbContext.SaveChanges();
                    return Task.FromResult(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while accepting the order.", ex);
            }
        }

        public Task<bool> DeleteOrder(Guid orderId)
        {
            try
            {
                var order = _orderServiceDbContext.Orders.FirstOrDefault(o => o.OrderId == orderId);
                if (order == null)
                {
                    throw new Exception("Order not found.");
                }
                else
                {
                    _orderServiceDbContext.Orders.Remove(order);
                    _orderServiceDbContext.SaveChanges();
                    return Task.FromResult(true);
                }
            }
            catch( Exception ex)
            {
               throw new Exception("An error occurred while deleting the order.", ex);  
            }
        }

        public List<Order> GetAllOrders()
        {
           return _orderServiceDbContext.Orders.ToList();
        }

        public Order? GetOrderById(Guid orderId)
        {
            return _orderServiceDbContext.Orders.FirstOrDefault(o => o.OrderId == orderId);
        }

        public async Task<bool> SaveOrder(Order order, long cartId)
        {
            try { 
               await _orderServiceDbContext.Orders.AddAsync(order);
               await _orderServiceDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the order.", ex);
            }
            return true;
        }
    }
}
