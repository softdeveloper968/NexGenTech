using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.Charges
{
    //AA-326
    public class GetCashProjectionByDayQuery : IRequest<Result<List<GetCashProjectionByDayResponse>>>
    {
        public int FilterForDays { get; set; } = 7; //AA-343
        public string ClientLocationIds { get; set; } = string.Empty; //AA-343
        public string ClientProviderIds { get; set; } = string.Empty; //AA-343
    }
    public class GetCashProjectionByDayQueryHandler : IRequestHandler<GetCashProjectionByDayQuery, Result<List<GetCashProjectionByDayResponse>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;

        public GetCashProjectionByDayQueryHandler(IClaimStatusQueryService claimStatusQueryService)
        {
            _claimStatusQueryService = claimStatusQueryService;
        }

        /// <summary>
        /// Handles the GetCashProjectionByDayQuery and retrieves cash projection data by day.
        /// </summary>
        /// <param name="query">The query specifying the parameters for the data retrieval.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A Result containing a list of GetCashProjectionByDayResponse objects.</returns>
        public async Task<Result<List<GetCashProjectionByDayResponse>>> Handle(GetCashProjectionByDayQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Query the service to execute the stored procedure
                var cashProjectionDetails = await _claimStatusQueryService.GetCashProjectionByDayAsync(query);
                var response = cashProjectionDetails.Select(z => new GetCashProjectionByDayResponse()
                {
                    ClaimCount = z.ClaimCount,
                    CheckNumber = z.CheckNumber,
                    CheckDate = z.CheckDateString,
                    PaidTotals = z.PaidTotals,
                    RevenueTotals = z.RevenueTotals,
                    ClaimLevelMd5Hash = z.ClaimLevelMd5Hash,
                    ClientInsuranceId = z.ClientInsuranceId,
                    PayerName = z.PayerName,
                    AccountNumber = z.AccountNumber,
                    ExternalId = z.ExternalId,
                    PatientLastCommaFirst = z.PatientLastCommaFirst
                }).ToList() ?? new List<GetCashProjectionByDayResponse>();

                // Return a successful Result with the response data
                return await Result<List<GetCashProjectionByDayResponse>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, or rethrow if necessary
                throw;
            }
        }
    }

}
