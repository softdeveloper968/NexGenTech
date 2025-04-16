
using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeRoleClaimStatusExceptionReasonCategories;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class EmployeeRoleClaimStatusExceptionReasonCategoryProfile : Profile
    {
        public EmployeeRoleClaimStatusExceptionReasonCategoryProfile()
        {
            CreateMap<EmployeeRoleClaimStatusExceptionReasonCategory, EmployeeRoleClaimStatusExceptionReasonCategoryDto>().ReverseMap();
        }
    }
}
