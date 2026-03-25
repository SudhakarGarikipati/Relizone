using AutoMapper;
using OrderService.Application.Dtos;
using OrderService.Application.Repositories;
using OrderService.Application.Services.Abstraction;
using OrderService.Domain.Entities;

namespace OrderService.Application.Services.Implementation
{
    public class OrderAppService : IOrderAppService
    {
        readonly IOrderServiceRepository _orderServiceRepository;
        readonly IMapper _mapper;
        public OrderAppService(IOrderServiceRepository orderServiceRepository, IMapper mapper) { 
            this._orderServiceRepository = orderServiceRepository;
            this._mapper = mapper;
        }

        public Task<bool> AcceptedOrder(Guid orderId, DateTime AcceptedDateTime)
        {
           return _orderServiceRepository.AcceptedOrder(orderId, AcceptedDateTime);
        }

        public Task<bool> DeleteOrder(Guid orderId)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentNullException(nameof(orderId), "Order ID cannot be empty.");
            return _orderServiceRepository.DeleteOrder(orderId);
        }

        public List<Order> GetAllOrders()
        {
            return _orderServiceRepository.GetAllOrders();
        }

        public Order? GetOrderById(Guid orderId)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentNullException(nameof(orderId), "Order ID cannot be empty.");
            return _orderServiceRepository.GetOrderById(orderId);
        }

        public async Task<bool> SaveOrder(OrderDto orderDto, long cartId)
        {
            var order = _mapper.Map<Order>(orderDto);    
            await _orderServiceRepository.SaveOrder(order, cartId);
            return true;
        }
    }
}
