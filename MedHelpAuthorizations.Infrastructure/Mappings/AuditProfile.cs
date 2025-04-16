using AutoMapper;
using MedHelpAuthorizations.Application.Models.Audit;
using MedHelpAuthorizations.Application.Responses.Audit;

namespace MedHelpAuthorizations.Infrastructure.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}