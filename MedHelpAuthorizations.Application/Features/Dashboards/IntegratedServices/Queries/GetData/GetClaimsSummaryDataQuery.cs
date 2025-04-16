using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Helpers;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetClaimsSummaryDataQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<ClaimDetailsSummary>> //AA-330
    {
    }

    public class GetClaimsSummaryDataQueryHandler : IRequestHandler<GetClaimsSummaryDataQuery, Result<ClaimDetailsSummary>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetClaimsSummaryDataQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetClaimsSummaryDataQueryHandler(IStringLocalizer<GetClaimsSummaryDataQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<ClaimDetailsSummary>> Handle(GetClaimsSummaryDataQuery query, CancellationToken cancellationToken)
        {
            // Retrieve claim status revenue totals using the claimStatusQueryService.
            var response = await _claimStatusQueryService.GetClaimsSummaryDataAsync(query) ?? new();

            // Return a successful result with the response data.
            return await Result<ClaimDetailsSummary>.SuccessAsync(response);
        }
    }
}
