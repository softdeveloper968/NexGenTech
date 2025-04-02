using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetInsurancesByProviderQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<InsuranceTotalsByProviderSummary>>>
    {
    }
    public class GetInsurancesByProviderQueryHandler : IRequestHandler<GetInsurancesByProviderQuery, Result<List<InsuranceTotalsByProviderSummary>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetInsurancesByProviderQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetInsurancesByProviderQueryHandler(IStringLocalizer<GetInsurancesByProviderQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<InsuranceTotalsByProviderSummary>>> Handle(GetInsurancesByProviderQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetInsuranceTotalsByProviderAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<InsuranceTotalsByProviderSummary>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
