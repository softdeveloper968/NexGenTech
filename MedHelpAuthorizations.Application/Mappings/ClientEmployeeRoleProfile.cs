using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeRoles;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientEmployeeRoleProfile : Profile
    {
        public ClientEmployeeRoleProfile()
        {
            CreateMap<ClientEmployeeRole, ClientEmployeeRoleDto>()
                .ForMember(src => src.Id, map => map.MapFrom(dest => dest.Id))
                .ForMember(src => src.EmployeeClientId, map => map.MapFrom(dest => dest.EmployeeClientId))
                //.ForMember(src => src., map => map.MapFrom(dest => dest.EmployeeClientId))
                .ForMember(src => src.EmployeeRoleId, map => map.MapFrom(dest => dest.EmployeeRoleId)).ReverseMap();
                
        }
    }
}
