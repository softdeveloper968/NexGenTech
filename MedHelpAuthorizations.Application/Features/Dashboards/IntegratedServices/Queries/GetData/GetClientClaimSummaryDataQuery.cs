using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetClientClaimSummaryDataQuery : ICorporateDashboardQueryBase, IRequest<Result<ClaimSummary>>
    {
        public string TenantClientString { get; set; }
    }
    public class GetClientClaimSummaryDataQueryHandler : IRequestHandler<GetClientClaimSummaryDataQuery, Result<ClaimSummary>>
    {
        private readonly ICorporateDashboardService _corporateDashboardQueryService;
        private readonly IStringLocalizer<GetClientClaimSummaryDataQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetClientClaimSummaryDataQueryHandler(IStringLocalizer<GetClientClaimSummaryDataQueryHandler> localizer, ICorporateDashboardService corporateDashboardQueryService)
        {
            _localizer = localizer;
            _corporateDashboardQueryService = corporateDashboardQueryService;
        }

        public async Task<Result<ClaimSummary>> Handle(GetClientClaimSummaryDataQuery query, CancellationToken cancellationToken)
        {
            // Retrieve claim status revenue totals using the claimStatusQueryService.
            var response = await _corporateDashboardQueryService.GetClientClaimSummaryDataAsync(query) ?? new();

            // Return a successful result with the response data.
            return await Result<ClaimSummary>.SuccessAsync(response);
        }
    }
}
