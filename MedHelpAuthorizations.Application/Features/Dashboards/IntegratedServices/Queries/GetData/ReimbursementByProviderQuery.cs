using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ReimbursementByProviderQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ReimbursementByProviderSummary>>>
    {


        public class ReimbursementByProviderQueryHandler : IRequestHandler<ReimbursementByProviderQuery, Result<List<ReimbursementByProviderSummary>>>
        {
            private readonly IClaimStatusQueryService _claimStatusQueryService;
            private readonly IStringLocalizer<ReimbursementByProviderQueryHandler> _localizer;

            // Constructor for the query handler.
            public ReimbursementByProviderQueryHandler(IStringLocalizer<ReimbursementByProviderQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
            {
                _localizer = localizer;
                _claimStatusQueryService = claimStatusQueryService;
            }

            public async Task<Result<List<ReimbursementByProviderSummary>>> Handle(ReimbursementByProviderQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    // Retrieve claim status revenue totals using the claimStatusQueryService.
                    var response = await _claimStatusQueryService.GetReimbursementByProviderAsync(query) ?? new List<ReimbursementByProviderSummary>();

                    // Return a successful result with the response data.
                    return await Result<List<ReimbursementByProviderSummary>>.SuccessAsync(response);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

    }
}
