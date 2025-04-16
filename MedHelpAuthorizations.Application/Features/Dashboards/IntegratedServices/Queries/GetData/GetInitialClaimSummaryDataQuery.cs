using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetInitialClaimSummaryDataQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<ClaimDetailsSummary>>
    {

    }

    public class GetInitialClaimSummaryDataQueryHandler : IRequestHandler<GetInitialClaimSummaryDataQuery, Result<ClaimDetailsSummary>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetInitialClaimSummaryDataQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetInitialClaimSummaryDataQueryHandler(IStringLocalizer<GetInitialClaimSummaryDataQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<ClaimDetailsSummary>> Handle(GetInitialClaimSummaryDataQuery query, CancellationToken cancellationToken)
        {
            // Retrieve claim status revenue totals using the claimStatusQueryService.
            var response = await _claimStatusQueryService.GetInitialClaimSummaryDataAsync(query) ?? new();

            // Return a successful result with the response data.
            return await Result<ClaimDetailsSummary>.SuccessAsync(response);
        }
    }
}
