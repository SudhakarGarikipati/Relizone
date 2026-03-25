using AutoMapper;
using CartService.Application.Dtos;
using CartService.Domain.Entities;

namespace CartService.Application.Mappers
{
    public class CartServiceMapper : Profile
    {
        public CartServiceMapper()
        {
            // Add your mapping configurations here
            // For example:
            // CreateMap<SourceEntity, DestinationEntity>();
            // CreateMap<SourceDto, DestinationDto>();
            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();
        }
    }
}
