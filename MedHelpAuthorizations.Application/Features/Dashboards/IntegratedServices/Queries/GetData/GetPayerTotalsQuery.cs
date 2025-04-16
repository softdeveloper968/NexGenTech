using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetPayerTotalsQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<PayerTotalsByProvider>>>
    {
    }

    public class GetPayerTotalsQueryHandler : IRequestHandler<GetPayerTotalsQuery, Result<List<PayerTotalsByProvider>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetPayerTotalsQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetPayerTotalsQueryHandler(IStringLocalizer<GetPayerTotalsQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<PayerTotalsByProvider>>> Handle(GetPayerTotalsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetPayerTotalsQuery(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<PayerTotalsByProvider>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
