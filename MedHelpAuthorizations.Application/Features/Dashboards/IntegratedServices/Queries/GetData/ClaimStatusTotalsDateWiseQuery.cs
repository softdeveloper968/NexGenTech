using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ClaimStatusTotalsDateWiseQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<ClaimStatusTotalSummary>>
    {

    }
    public class ClaimStatusTotalsDateWiseQueryHandler : IRequestHandler<ClaimStatusTotalsDateWiseQuery, Result<ClaimStatusTotalSummary>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ClaimStatusTotalsDateWiseQueryHandler> _localizer;

        // Constructor for the query handler.
        public ClaimStatusTotalsDateWiseQueryHandler(IStringLocalizer<ClaimStatusTotalsDateWiseQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<ClaimStatusTotalSummary>> Handle(ClaimStatusTotalsDateWiseQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetClaimStatusTotalsDateWiseAsync(query) ?? new ClaimStatusTotalSummary();

                // Return a successful result with the response data.
                return await Result<ClaimStatusTotalSummary>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
