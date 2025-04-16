using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.Charges
{
    //AA-326
    public class GetCashValueForRevenueByDayQuery : IRequest<Result<List<GetCashValueForRevenueByDayResponse>>>
    {
        /// <summary>
        /// If value is "ServiceDate" then the items will be filtered based on their service date
        /// If value is "BilledDate" then the items will be filtered based on their billed date 
        /// </summary>
        public string FilterBy { get; set; } = "ServiceDate";
        public int FilterForDays { get; set; } = -7; //AA-331
        public string ClientLocationIds { get; set; } = string.Empty; //AA-331
        public string ClientProviderIds { get; set; } = string.Empty; //AA-331
        //public int ClientInsuranceId { get; set; } = 0;
    }

    public class GetCashValueForRevenueByDayQueryHandler : IRequestHandler<GetCashValueForRevenueByDayQuery, Result<List<GetCashValueForRevenueByDayResponse>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;

        public GetCashValueForRevenueByDayQueryHandler(IClaimStatusQueryService claimStatusQueryService)
        {
            _claimStatusQueryService = claimStatusQueryService;
        }

        /// <summary>
        /// Handles the GetCashValueForRevenueByDayQuery and retrieves cash value for revenue by day.
        /// </summary>
        /// <param name="query">The query specifying the parameters for the data retrieval.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A Result containing a list of GetCashValueForRevenueByDayResponse objects.</returns>
        public async Task<Result<List<GetCashValueForRevenueByDayResponse>>> Handle(GetCashValueForRevenueByDayQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Query the service to execute the stored procedure
                var response = await _claimStatusQueryService.GetCashValueForRevenueByDayAsync(query);

                // Return a successful Result with the response data
                return await Result<List<GetCashValueForRevenueByDayResponse>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, or rethrow if necessary
                throw;
            }
        }
    }
}
