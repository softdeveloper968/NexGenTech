using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetPayerReimbursementByLocationQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<PayerReimbursementSummary>>>
    {
    }
    public class GetPayerReimbursementByLocationQueryHandler : IRequestHandler<GetPayerReimbursementByLocationQuery, Result<List<PayerReimbursementSummary>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetPayerReimbursementByLocationQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetPayerReimbursementByLocationQueryHandler(IStringLocalizer<GetPayerReimbursementByLocationQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<PayerReimbursementSummary>>> Handle(GetPayerReimbursementByLocationQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetPayerReimbursementByLocationAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<PayerReimbursementSummary>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {
                return await Result<List<PayerReimbursementSummary>>.FailAsync(ex.Message);
            }
        }
    }
}
