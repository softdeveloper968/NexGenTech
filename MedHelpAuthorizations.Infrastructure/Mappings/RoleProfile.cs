using AutoMapper;
using MedHelpAuthorizations.Application.Responses.Identity;
using Microsoft.AspNetCore.Identity;

namespace MedHelpAuthorizations.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, IdentityRole>().ReverseMap();
            CreateMap<RoleNamesResponse, IdentityRole>().ReverseMap();
        }
    }
}