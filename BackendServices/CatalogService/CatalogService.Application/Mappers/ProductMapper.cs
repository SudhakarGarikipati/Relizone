using AutoMapper;
using CatalogService.Application.DTOs;
using CatalogService.Domain.Entities;


namespace CatalogService.Application.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDto>()
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.Ignore());
        }
    }
}
