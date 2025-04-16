using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class DenialMonthlyComparisonQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<MonthlyClaimSummary>>>
    {
    }

    public class DenialMonthlyComparisonQueryHandler : IRequestHandler<DenialMonthlyComparisonQuery, Result<List<MonthlyClaimSummary>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<DenialMonthlyComparisonQueryHandler> _localizer;

        // Constructor for the query handler.
        public DenialMonthlyComparisonQueryHandler(IStringLocalizer<DenialMonthlyComparisonQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<MonthlyClaimSummary>>> Handle(DenialMonthlyComparisonQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetMonthlyDenialSummaryAsync(query) ?? new List<MonthlyClaimSummary>();

                // Return a successful result with the response data.
                return await Result<List<MonthlyClaimSummary>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

}
