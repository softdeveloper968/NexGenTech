using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetDenialReasonsByLocationsQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<DenialReasonsByLocationsSummary>>>
    {
    }
    public class GetDenialReasonsByLocationsQueryHandler : IRequestHandler<GetDenialReasonsByLocationsQuery, Result<List<DenialReasonsByLocationsSummary>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetDenialReasonsByLocationsQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetDenialReasonsByLocationsQueryHandler(IStringLocalizer<GetDenialReasonsByLocationsQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<DenialReasonsByLocationsSummary>>> Handle(GetDenialReasonsByLocationsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetDenialReasonssByLocationsAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<DenialReasonsByLocationsSummary>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {
                return await Result<List<DenialReasonsByLocationsSummary>>.FailAsync(ex.Message);
            }
        }
    }
}
