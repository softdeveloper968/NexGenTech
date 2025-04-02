
using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClientAlphaSplits;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class EmployeeClientAlphaSplitProfile : Profile
    {
        public EmployeeClientAlphaSplitProfile()
        {
            CreateMap<EmployeeClientAlphaSplit, EmployeeClientAlphaSplitDto>().ReverseMap();
        }
    }
}
