using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ChargesByPayerQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ChargesByPayer>>> //AA-330
    {
    }

    public class ChargesByPayerQueryHandler : IRequestHandler<ChargesByPayerQuery, Result<List<ChargesByPayer>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ChargesByPayerQueryHandler> _localizer;

        // Constructor for the query handler.
        public ChargesByPayerQueryHandler(IStringLocalizer<ChargesByPayerQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<ChargesByPayer>>> Handle(ChargesByPayerQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetChargesByPayerAsync(query) ?? new();

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
