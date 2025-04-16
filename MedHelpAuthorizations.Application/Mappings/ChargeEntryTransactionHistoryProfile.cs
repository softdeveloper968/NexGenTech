using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Commands.Upsert;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ChargeEntryTransactionHistoryProfile : Profile
    {
        public ChargeEntryTransactionHistoryProfile() 
        {
            CreateMap<ChargeEntryTransaction, ChargeEntryTransactionHistory>()
               .ForMember(th => th.Id, map => map.Ignore())
               .ForMember(th => th.ChargeEntryTransactionId, map => map.MapFrom(tr => tr.Id));

            CreateMap<UpsertChargeEntryTransactionCommand, ChargeEntryTransactionHistory>()
               .ForMember(th => th.Id, map => map.Ignore())
               .ForMember(th => th.ChargeEntryTransactionId, map => map.MapFrom(tr => tr.Id));
        }
    }
}
