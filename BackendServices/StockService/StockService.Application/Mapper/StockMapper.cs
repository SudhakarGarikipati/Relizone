using AutoMapper;
using StockService.Application.Dtos;
using StockService.Domain.Entities;

namespace StockService.Application.Mapper
{
    public class StockMapper: Profile
    {
        public StockMapper()
        {
            // Create your object mappings here
            // Example: CreateMap<Source, Destination>();
            CreateMap<Stock, StockDto>().ReverseMap();
        }
    }
}
