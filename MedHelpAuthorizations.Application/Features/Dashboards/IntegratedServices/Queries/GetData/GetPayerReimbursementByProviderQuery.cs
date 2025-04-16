using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetPayerReimbursementByProviderQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<PayerReimbursementByProviderSummary>>>
    {
    }
    public class GetPayerReimbursementByPayerQueryHandler : IRequestHandler<GetPayerReimbursementByProviderQuery, Result<List<PayerReimbursementByProviderSummary>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetPayerReimbursementByPayerQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetPayerReimbursementByPayerQueryHandler(IStringLocalizer<GetPayerReimbursementByPayerQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<PayerReimbursementByProviderSummary>>> Handle(GetPayerReimbursementByProviderQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetPayerReimbursementByProviderAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<PayerReimbursementByProviderSummary>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
