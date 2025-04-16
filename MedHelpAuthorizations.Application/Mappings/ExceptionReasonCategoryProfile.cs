using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ExceptionReason.Commands;
using MedHelpAuthorizations.Domain.IntegratedServices;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ExceptionReasonCategoryProfile : Profile
    {
        public ExceptionReasonCategoryProfile()
        {
            CreateMap<ClaimStatusExceptionReasonCategoryMap, AddClaimStatusExceptionReasonCategoryCommand>().ReverseMap();
        }
    }
}
