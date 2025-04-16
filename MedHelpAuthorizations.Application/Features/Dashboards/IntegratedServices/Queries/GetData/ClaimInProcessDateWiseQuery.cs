using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ClaimInProcessDateWiseQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ClaimProcessSummary>>>
    {

        public class ClaimInProcessDateWiseQueryHandler : IRequestHandler<ClaimInProcessDateWiseQuery, Result<List<ClaimProcessSummary>>>
        {
            private readonly IClaimStatusQueryService _claimStatusQueryService;
            private readonly IStringLocalizer<ClaimInProcessDateWiseQueryHandler> _localizer;

            // Constructor for the query handler.
            public ClaimInProcessDateWiseQueryHandler(IStringLocalizer<ClaimInProcessDateWiseQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
            {
                _localizer = localizer;
                _claimStatusQueryService = claimStatusQueryService;
            }

            public async Task<Result<List<ClaimProcessSummary>>> Handle(ClaimInProcessDateWiseQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    // Retrieve claim status revenue totals using the claimStatusQueryService.
                    var response = await _claimStatusQueryService.GetClaimInProcessDateWiseAsync(query) ?? new List<ClaimProcessSummary>();

                    // Return a successful result with the response data.
                    return await Result<List<ClaimProcessSummary>>.SuccessAsync(response);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

    }
}
