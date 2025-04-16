using MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IClientFeeScheduleService
	{
        Task<bool> ProcessFeeScheduleMatchedClaim(ClaimStatusBatchClaim claim, string tenantIdentifier = null, int? clientFeeScheduleEntryId = null);
        Task<decimal> GetLatestPaidAmountForPayerCptDos(int clientInsuranceId, int? clientCptCode, int clientId, DateTime DateOfService);//EN-264

		Task<List<FeeScheduleCriteriaResultViewModel>> GetClaimStatusAveragePaidAmountAsync(FeeScheduleCriteriaModel feeScheduleCriteriaModel, int clientId = 0, string connStr = null);

        Task UpdateOrCreateClaimStatusTransactionAsync(ClaimStatusBatchClaim claim, int clientFeeScheduleEntryId);


    }
}
