using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetProviderTotalsByPayerQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<PayerProviderTotals>>>
    {
    }
    public class GetProviderTotalsByPayerQueryHandler : IRequestHandler<GetProviderTotalsByPayerQuery, Result<List<PayerProviderTotals>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetProviderTotalsByPayerQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetProviderTotalsByPayerQueryHandler(IStringLocalizer<GetProviderTotalsByPayerQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<PayerProviderTotals>>> Handle(GetProviderTotalsByPayerQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetProviderTotalsByPayerQuery(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<PayerProviderTotals>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
