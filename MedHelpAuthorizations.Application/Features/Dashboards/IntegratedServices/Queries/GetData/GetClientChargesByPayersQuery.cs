using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetClientChargesByPayersQuery : ICorporateDashboardQueryBase, IRequest<Result<List<ChargesByPayer>>> //AA-330
    {
        public string TenantClientString { get; set; }
    }

    public class GetClientChargesByPayersQueryHandler : IRequestHandler<GetClientChargesByPayersQuery, Result<List<ChargesByPayer>>>
    {
        private readonly ICorporateDashboardService _corporateDashboardService;
        private readonly IStringLocalizer<GetClientChargesByPayersQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetClientChargesByPayersQueryHandler(IStringLocalizer<GetClientChargesByPayersQueryHandler> localizer, ICorporateDashboardService claimStatusQueryService)
        {
            _localizer = localizer;
            _corporateDashboardService = claimStatusQueryService;
        }

        public async Task<Result<List<ChargesByPayer>>> Handle(GetClientChargesByPayersQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _corporateDashboardService.GetClientChargesByPayerDataAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<ChargesByPayer>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
