using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetPaymentsByInsuranceQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<PaymentSummary>>>
    {
    }
    public class GetPaymentsByInsuranceQueryHandler : IRequestHandler<GetPaymentsByInsuranceQuery, Result<List<PaymentSummary>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetPaymentsByInsuranceQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetPaymentsByInsuranceQueryHandler(IStringLocalizer<GetPaymentsByInsuranceQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<PaymentSummary>>> Handle(GetPaymentsByInsuranceQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetPaymentsTotalsByPayerAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<PaymentSummary>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
