using AutoMapper;
using OrderService.Application.Dtos;
using OrderService.Domain.Entities;

namespace OrderService.Application.Profiles
{
    public class OrderServiceMapper : Profile
    {
        public OrderServiceMapper()
        {
            // CreateMap<Source, Destination>();
            CreateMap<Order, OrderDto>().ReverseMap();
        }
    }
}
