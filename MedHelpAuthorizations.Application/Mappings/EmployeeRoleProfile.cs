using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeRoles;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class EmployeeRoleProfile : Profile
    {
        public EmployeeRoleProfile()
        {
            CreateMap<EmployeeRole, EmployeeRoleDto>().ReverseMap();                
        }
    }
}
