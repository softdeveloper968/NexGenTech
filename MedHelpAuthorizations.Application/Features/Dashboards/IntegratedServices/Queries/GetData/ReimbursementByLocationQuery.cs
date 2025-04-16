using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ReimbursementByLocationQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ReimbursementByLocationSummary>>>
    {

        public class ReimbursementByLocationQueryHandler : IRequestHandler<ReimbursementByLocationQuery, Result<List<ReimbursementByLocationSummary>>>
        {
            private readonly IClaimStatusQueryService _claimStatusQueryService;
            private readonly IStringLocalizer<ReimbursementByLocationQueryHandler> _localizer;

            // Constructor for the query handler.
            public ReimbursementByLocationQueryHandler(IStringLocalizer<ReimbursementByLocationQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
            {
                _localizer = localizer;
                _claimStatusQueryService = claimStatusQueryService;
            }

            public async Task<Result<List<ReimbursementByLocationSummary>>> Handle(ReimbursementByLocationQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    // Retrieve claim status revenue totals using the claimStatusQueryService.
                    var response = await _claimStatusQueryService.GetReimbursementByLocationAsync(query) ?? new List<ReimbursementByLocationSummary>();

                    // Return a successful result with the response data.
                    return await Result<List<ReimbursementByLocationSummary>>.SuccessAsync(response);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

    }
}
