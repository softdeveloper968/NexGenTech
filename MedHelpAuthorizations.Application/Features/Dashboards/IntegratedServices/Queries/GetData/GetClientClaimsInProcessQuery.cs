using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetClientClaimsInProcessQuery : ICorporateDashboardQueryBase, IRequest<Result<List<ClaimSummary>>>
    {
        public string TenantClientString { get; set; }
    }
    public class GetClientClaimsInProcessQueryHandler : IRequestHandler<GetClientClaimsInProcessQuery, Result<List<ClaimSummary>>>
    {
        private readonly ICorporateDashboardService _corporateDashboardService;
        private readonly IStringLocalizer<GetClientClaimsInProcessQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetClientClaimsInProcessQueryHandler(IStringLocalizer<GetClientClaimsInProcessQueryHandler> localizer, ICorporateDashboardService corporateDashboardService)
        {
            _localizer = localizer;
            _corporateDashboardService = corporateDashboardService;
        }

        public async Task<Result<List<ClaimSummary>>> Handle(GetClientClaimsInProcessQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _corporateDashboardService.GetClientClaimsInProcessDataAsync(query) ?? new List<ClaimSummary>();

                // Return a successful result with the response data.
                return await Result<List<ClaimSummary>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
