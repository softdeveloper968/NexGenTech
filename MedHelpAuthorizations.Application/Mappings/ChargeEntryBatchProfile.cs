
namespace MedHelpAuthorizations.Application.Mappings
{
    using AutoMapper;

    using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Base;
    using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Commands.Create;
    using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Commands.Update;
    using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetAll;
    using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetByCriteria;
    using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetById;
    using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetRecent;
    using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

    public class ChargeEntryBatchProfile : Profile
    {
        public ChargeEntryBatchProfile()
        {
            CreateMap<CreateChargeEntryBatchCommand, ChargeEntryBatch>().ReverseMap();
            CreateMap<UpdateChargeEntryBatchCommand, ChargeEntryBatch>().ReverseMap();

            CreateMap<GetRecentChargeEntryBatchesByClientIdResponse, ChargeEntryBatch>();
            CreateMap<ChargeEntryBatch, GetRecentChargeEntryBatchesByClientIdResponse>()
                .ForPath(x => x.ClientId, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.ClientId))
                .ForPath(x => x.ClientCode, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.Client.ClientCode))
                .ForPath(x => x.RpaTypeDescription, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.RpaType.Description))
                .ForPath(x => x.AuthTypeName, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.AuthType.Name))
                .ForPath(x => x.TransactionTypeName, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.TransactionType.Description));

            CreateMap<GetChargeEntryBatchByIdResponse, ChargeEntryBatch>();
            CreateMap<ChargeEntryBatch, GetChargeEntryBatchByIdResponse>()
                .ForPath(x => x.ClientId, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.ClientId))
                .ForPath(x => x.ClientCode, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.Client.ClientCode))
                .ForPath(x => x.RpaTypeDescription, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.RpaType.Description))
                .ForPath(x => x.AuthTypeName, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.AuthType.Name))
                .ForPath(x => x.TransactionTypeName, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.TransactionType.Description));

            CreateMap<GetChargeEntryUnprocessedBatchesResponse, ChargeEntryBatch>();
            CreateMap<ChargeEntryBatch, GetChargeEntryUnprocessedBatchesResponse>()
                .ForPath(x => x.ClientId, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.ClientId))
                .ForPath(x => x.ClientCode, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.Client.ClientCode))
                .ForPath(x => x.RpaTypeDescription, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.RpaType.Description))
                .ForPath(x => x.AuthTypeName, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.AuthType.Name))
                .ForPath(x => x.TransactionTypeName, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.TransactionType.Description));

            CreateMap<GetAllChargeEntryBatchesResponse, ChargeEntryBatch>();
            CreateMap<ChargeEntryBatch, GetAllChargeEntryBatchesResponse>()
                .ForPath(x => x.ClientId, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.ClientId))
                .ForPath(x => x.ClientCode, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.Client.ClientCode))
                .ForPath(x => x.RpaTypeDescription, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.RpaType.Description))
                .ForPath(x => x.AuthTypeName, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.AuthType.Name))
                .ForPath(x => x.TransactionTypeName, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.TransactionType.Description));

            CreateMap<GetChargeEntryBatchBaseResponse, ChargeEntryBatch>();
            CreateMap<ChargeEntryBatch, GetChargeEntryBatchBaseResponse>()
                .ForPath(x => x.ClientId, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.ClientId))
                .ForPath(x => x.ClientCode, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.Client.ClientCode))
                .ForPath(x => x.RpaTypeDescription, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.RpaType.Description))
                .ForPath(x => x.AuthTypeName, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.AuthType.Name))
                .ForPath(x => x.TransactionTypeName, map => map.MapFrom(y => y.ChargeEntryRpaConfiguration.TransactionType.Description));
        }
    }
}
