using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetChargesByLocationQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ChargesTotalsByLocation>>> //AA-330
    {
    }

    public class GetChargesByLocationQueryHandler : IRequestHandler<GetChargesByLocationQuery, Result<List<ChargesTotalsByLocation>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetChargesByLocationQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetChargesByLocationQueryHandler(IStringLocalizer<GetChargesByLocationQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<ChargesTotalsByLocation>>> Handle(GetChargesByLocationQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetChargesByLocationAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<ChargesTotalsByLocation>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {
                return await Result<List<ChargesTotalsByLocation>>.FailAsync(ex.Message);
            }
        }
    }
}
