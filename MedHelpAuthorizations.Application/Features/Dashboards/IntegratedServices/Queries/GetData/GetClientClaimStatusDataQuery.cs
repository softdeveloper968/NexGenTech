using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetClientClaimStatusDataQuery : ICorporateDashboardQueryBase, IRequest<Result<ClaimSummary>>
    {
        public string TenantClientString { get; set; }
    }
    public class GetClientClaimStatusDataQueryHandler : IRequestHandler<GetClientClaimStatusDataQuery, Result<ClaimSummary>>
    {
        private readonly ICorporateDashboardService _corporateDashboardService;
        private readonly IStringLocalizer<GetClientClaimStatusDataQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetClientClaimStatusDataQueryHandler(IStringLocalizer<GetClientClaimStatusDataQueryHandler> localizer, ICorporateDashboardService corporateDashboardService)
        {
            _localizer = localizer;
            _corporateDashboardService = corporateDashboardService;
        }

        public async Task<Result<ClaimSummary>> Handle(GetClientClaimStatusDataQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _corporateDashboardService.GetClientClaimStatusDataAsync(query) ?? new ClaimSummary();

                // Return a successful result with the response data.
                return await Result<ClaimSummary>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
