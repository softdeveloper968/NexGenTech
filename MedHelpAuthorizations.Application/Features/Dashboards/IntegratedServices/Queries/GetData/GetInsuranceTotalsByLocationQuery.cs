using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetInsuranceTotalsByLocationQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<InsuranceTotalsByLocationSummary>>>
    {
    }
    public class GetInsuranceTotalsByLocationQueryHandler : IRequestHandler<GetInsuranceTotalsByLocationQuery, Result<List<InsuranceTotalsByLocationSummary>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetInsuranceTotalsByLocationQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetInsuranceTotalsByLocationQueryHandler(IStringLocalizer<GetInsuranceTotalsByLocationQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<InsuranceTotalsByLocationSummary>>> Handle(GetInsuranceTotalsByLocationQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetInsuranceTotalsByLocationsAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<InsuranceTotalsByLocationSummary>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {
                return await Result<List<InsuranceTotalsByLocationSummary>>.FailAsync(ex.Message);
            }
        }
    }
}
