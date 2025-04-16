using MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.Base;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IManuallyRunJobService
    {
        Task<bool> UpdateNormalizedClaimsAsync(CancellationToken cancellationToken);
        Task<bool> DoResetUnknownDaysWaitLapsedClaims();
        Task<bool> ProcessClaimsManually(); //AA-231

        /// <summary>
        /// Fetch data for specific tenant
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DoUpdateDevDemoDataAuditDates(CancellationToken cancellationToken);

        Task<bool> UpdateClaimStatusBatchClaimsByCptCode(CancellationToken cancellationToken); //EN-214
        Task ProcessFeeScheduleEntriesForAllTenants(); //EN-232
        Task ProcessFeeScheduleEntriesForSingleTenant(string tenantIdentifier, int clientFeeScheduleId); //EN-232 
        Task PreSummarizedClaimsDataRefreshService(CancellationToken cancellationToken);
        Task GetClaimLineItemForUnPaidClaims(CancellationToken cancellationToken); //EN-747

    }
}