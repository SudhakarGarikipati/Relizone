using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using AutoMapper;

namespace AuthService.Application.Mappers
{
    public class AuthMapper : Profile
    {
        public AuthMapper()
        {
            // Define your mappings here
            // Example:
            // CreateMap<SourceType, DestinationType>()
            //     .ForMember(dest => dest.Property, opt => opt.MapFrom(src => src.OtherProperty));

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r=>r.Name).ToArray()));

            CreateMap<SignUpDto, User>();
        }
    }
}
