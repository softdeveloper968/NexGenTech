using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetDenialsByInsuranceQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<DenialTotalsByInsuranceSummary>>>
    {
    }
    public class GetDenialsByInsuranceQueryHandler : IRequestHandler<GetDenialsByInsuranceQuery, Result<List<DenialTotalsByInsuranceSummary>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetDenialsByInsuranceQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetDenialsByInsuranceQueryHandler(IStringLocalizer<GetDenialsByInsuranceQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<DenialTotalsByInsuranceSummary>>> Handle(GetDenialsByInsuranceQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetDenialTotalsByPayerAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<DenialTotalsByInsuranceSummary>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
