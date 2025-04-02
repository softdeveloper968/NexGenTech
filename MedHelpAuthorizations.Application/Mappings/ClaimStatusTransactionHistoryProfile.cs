using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Upsert;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClaimStatusTransactionHistoryProfile : Profile
    {
        public ClaimStatusTransactionHistoryProfile() 
        {
			CreateMap<ClaimStatusTransaction, ClaimStatusTransactionHistory>()
               .ForMember(th => th.Id, map => map.Ignore())
			   .ForMember(th => th.ClaimLineItemStatus, map => map.Ignore())
			   .ForMember(th => th.ClaimStatusTransactionId, map => map.MapFrom(tr => tr.Id));

            CreateMap<UpsertClaimStatusTransactionCommand, ClaimStatusTransactionHistory>()
               .ForMember(th => th.Id, map => map.Ignore())
               .ForMember(th => th.ClaimStatusTransactionId, map => map.MapFrom(tr => tr.Id))
               .ForMember(th => th.ClaimStatusExceptionReasonCategoryId, map => map.MapFrom(tr => tr.ExceptionReasonCategoryId));
        }
    }
}
