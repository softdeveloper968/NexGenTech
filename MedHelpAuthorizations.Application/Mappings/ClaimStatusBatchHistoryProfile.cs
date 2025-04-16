using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Commands.Update;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Commands.Create;

using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClaimStatusBatchHistoryProfile : Profile
    {
        public ClaimStatusBatchHistoryProfile()
        {
            CreateMap<ClaimStatusBatch, ClaimStatusBatchHistory>()
               .ForMember(th => th.Id, map => map.Ignore())
               .ForMember(th => th.ClaimStatusBatchId, map => map.MapFrom(tr => tr.Id));

            CreateMap<CreateClaimStatusBatchCommand, ClaimStatusBatchHistory>()
               .ForMember(th => th.Id, map => map.Ignore());

            CreateMap<UpdateCompletedClaimStatusBatchCommand, ClaimStatusBatchHistory>()
               .ForMember(th => th.Id, map => map.Ignore())
               .ForMember(th => th.ClaimStatusBatchId, map => map.MapFrom(tr => tr.Id));
        }
    }
}
