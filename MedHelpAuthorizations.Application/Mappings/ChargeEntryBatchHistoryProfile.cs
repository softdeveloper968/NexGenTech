using AutoMapper;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Commands.Create;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Commands.Update;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ChargeEntryBatchHistoryProfile : Profile
    {
        public ChargeEntryBatchHistoryProfile()
        {
            CreateMap<ChargeEntryBatch, ChargeEntryBatchHistory>()
               .ForMember(th => th.Id, map => map.Ignore())
               .ForMember(th => th.ChargeEntryBatchId, map => map.MapFrom(tr => tr.Id));

            CreateMap<CreateChargeEntryBatchCommand, ChargeEntryBatchHistory>()
               .ForMember(th => th.Id, map => map.Ignore());

            CreateMap<UpdateChargeEntryBatchCommand, ChargeEntryBatchHistory>()
               .ForMember(th => th.Id, map => map.Ignore())
               .ForMember(th => th.ChargeEntryBatchId, map => map.MapFrom(tr => tr.Id));
        }
    }
}
