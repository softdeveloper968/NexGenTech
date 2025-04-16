using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.Flows.Create;
using MedHelpAuthorizations.Application.Features.IntegratedServices.Flows.Queries.Base;
using MedHelpAuthorizations.Application.Features.IntegratedServices.Flows.Queries.GetAll;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class FlowProfile : Profile
    {
        public FlowProfile()
        {
            CreateMap<CreateFlowCommand, Flow>().ReverseMap();
            CreateMap<GetAllFlowsQueryResponse, Flow>().ReverseMap();
            CreateMap<GetFlowsQueryBaseResponse, Flow>().ReverseMap();
        }
    }
}
