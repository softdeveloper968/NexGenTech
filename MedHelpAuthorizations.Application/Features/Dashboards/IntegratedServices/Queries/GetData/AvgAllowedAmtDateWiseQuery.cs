using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class AvgAllowedAmtDateWiseQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<AvgAllowedAmountSummary>>>
    {

        public class AvgAllowedAmtDateWiseQueryHandler : IRequestHandler<AvgAllowedAmtDateWiseQuery, Result<List<AvgAllowedAmountSummary>>>
        {
            private readonly IClaimStatusQueryService _claimStatusQueryService;
            private readonly IStringLocalizer<AvgAllowedAmtDateWiseQueryHandler> _localizer;

            // Constructor for the query handler.
            public AvgAllowedAmtDateWiseQueryHandler(IStringLocalizer<AvgAllowedAmtDateWiseQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
            {
                _localizer = localizer;
                _claimStatusQueryService = claimStatusQueryService;
            }

            public async Task<Result<List<AvgAllowedAmountSummary>>> Handle(AvgAllowedAmtDateWiseQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    // Retrieve claim status revenue totals using the claimStatusQueryService.
                    var response = await _claimStatusQueryService.GetAvgAllowedAmtDateWiseAsync(query) ?? new List<AvgAllowedAmountSummary>();

                    // Return a successful result with the response data.
                    return await Result<List<AvgAllowedAmountSummary>>.SuccessAsync(response);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

    }
}
