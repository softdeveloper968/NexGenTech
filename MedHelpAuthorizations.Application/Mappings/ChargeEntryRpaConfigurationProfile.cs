using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryRpaConfigurations.Commands.Create;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryRpaConfigurations.Commands.Update;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryRpaConfigurations.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryRpaConfigurations.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryRpaConfigurations.Queries.GetById;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Mappings
{
    using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryRpaConfigurations.Queries.GetByRpaType;
    using MedHelpAuthorizations.Application.Models.IntegratedServices.ChargeEntry;

    public class ChargeEntryRpaConfigurationProfile : Profile
    {
        public ChargeEntryRpaConfigurationProfile()
        {
            CreateMap<ChargeEntryRpaConfiguration, UiPathProcessConfiguration>()
                .ForPath(x => x.ClientCode, map => map.MapFrom(y => y.Client.ClientCode))
                .ForPath(x => x.RpaTypeDescription, map => map.MapFrom(y => y.RpaType.Code))
                .ForPath(x => x.IsMaxConsecutiveIssueResolved, map => map.MapFrom(y => y.RpaType.IsMaxConsecutiveIssueResolved))
                .ForPath(x => x.TransactionTypeName, map => map.MapFrom(y => y.TransactionType.Code))
                .ForPath(x => x.AuthTypeName, map => map.MapFrom(y => y.AuthType.Name));

            CreateMap<CreateChargeEntryRpaConfigurationCommand, ChargeEntryRpaConfiguration>().ReverseMap();
            CreateMap<UpdateChargeEntryRpaConfigurationCommand, ChargeEntryRpaConfiguration>().ReverseMap();
            CreateMap<GetChargeEntryRpaConfigurationByIdResponse, ChargeEntryRpaConfiguration>().ReverseMap();
            CreateMap<GetChargeEntryRpaConfigurationByCriteriaResponse, ChargeEntryRpaConfiguration>().ReverseMap();
            CreateMap<GetAllChargeEntryRpaConfigurationsResponse, ChargeEntryRpaConfiguration>().ReverseMap();
            CreateMap<GetChargeEntryRpaConfigurationByRpaTypeResponse, ChargeEntryRpaConfiguration>().ReverseMap();
        }
    }
}
