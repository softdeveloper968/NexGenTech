using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class PaymentsMonthlyQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<MonthlyClaimSummary>>> //AA-133
    {
    }

    public class PaymentsMonthlyQueryHandler : IRequestHandler<PaymentsMonthlyQuery, Result<List<MonthlyClaimSummary>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<PaymentsMonthlyQueryHandler> _localizer;

        // Constructor for the query handler.
        public PaymentsMonthlyQueryHandler(IStringLocalizer<PaymentsMonthlyQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<MonthlyClaimSummary>>> Handle(PaymentsMonthlyQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetPaymentsMonthlyDataAsync(query) ?? new List<MonthlyClaimSummary>();

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
