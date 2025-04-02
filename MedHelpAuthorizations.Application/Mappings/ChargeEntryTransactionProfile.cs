using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Commands.Upsert;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Queries.GetById;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ChargeEntryTransactionProfile : Profile
    {
        public ChargeEntryTransactionProfile()
        {
            CreateMap<UpsertChargeEntryTransactionCommand, ChargeEntryTransaction>().ReverseMap();
            CreateMap<GetChargeEntryTransactionByIdResponse, ChargeEntryTransaction>().ReverseMap();
            CreateMap<GetAllChargeEntryTransactionsResponse, ChargeEntryTransaction>().ReverseMap();
        }
    }
}
