using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Upsert;
using MedHelpAuthorizations.Application.Interfaces.Common;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
	public interface IClaimStatusTransactionService : IService
    {
		Task<int> UpsertClaimStatusTransaction(UpsertClaimStatusTransactionCommand request, string tenantIdentifier = null);
	}
}