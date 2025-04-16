using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.FlowLogEntries.Queries.Base;
using MedHelpAuthorizations.Application.Features.IntegratedServices.FlowLogEntries.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.Logs.Create;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class FlowLogEntryProfile : Profile
    {
        public FlowLogEntryProfile()
        {
            CreateMap<CreateFlowLogEntryCommand, FlowLogEntry>().ReverseMap();
            CreateMap<GetFlowLogEntryQueryBaseResponse, FlowLogEntry>().ReverseMap();
            CreateMap<GetAllFlowLogEntryQueryResponse, FlowLogEntry>().ReverseMap();
        }
    }
}
