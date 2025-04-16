using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ChargesByProviderQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ChargesTotalsByProvider>>> //AA-330
    {
    }

    public class ChargesByProviderQueryHandler : IRequestHandler<ChargesByProviderQuery, Result<List<ChargesTotalsByProvider>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ChargesByProviderQueryHandler> _localizer;

        // Constructor for the query handler.
        public ChargesByProviderQueryHandler(IStringLocalizer<ChargesByProviderQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<ChargesTotalsByProvider>>> Handle(ChargesByProviderQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetChargesByProviderAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<ChargesTotalsByProvider>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
