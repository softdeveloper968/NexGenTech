using AutoMapper;
using MedHelpAuthorizations.Application.Features.Dashboards.GetClaimsData;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClaimStatusTotalResultProfile : Profile
    {
        public ClaimStatusTotalResultProfile()
        {
            CreateMap<ClaimStatusTotalResult, GetClaimByProcedureSummaryResponse>().ReverseMap(); //EN-231
        }
    }
}
