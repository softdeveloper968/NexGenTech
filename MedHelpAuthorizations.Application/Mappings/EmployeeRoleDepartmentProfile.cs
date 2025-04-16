using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeDepartments;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class EmployeeRoleDepartmentProfile : Profile
    {
        public EmployeeRoleDepartmentProfile()
        {
            CreateMap<EmployeeRoleDepartment, EmployeeRoleDepartmentDto>().ReverseMap();                
        }
    }
}
