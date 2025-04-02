
using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClientAlphaSplits;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class AlphaSplitProfile : Profile
    {
        public AlphaSplitProfile()
        {
            CreateMap<AlphaSplit, AlphaSplitDto>().ReverseMap();
        }
    }
}
