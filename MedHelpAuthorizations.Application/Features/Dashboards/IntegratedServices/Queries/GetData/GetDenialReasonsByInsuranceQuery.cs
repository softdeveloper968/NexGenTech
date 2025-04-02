using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetDenialReasonsByInsuranceQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<DenialReasonsByInsuranceSummary>>>
    {
    }
    public class GetDenialReasonsByInsuranceQueryHandler : IRequestHandler<GetDenialReasonsByInsuranceQuery, Result<List<DenialReasonsByInsuranceSummary>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetDenialReasonsByInsuranceQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetDenialReasonsByInsuranceQueryHandler(IStringLocalizer<GetDenialReasonsByInsuranceQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<DenialReasonsByInsuranceSummary>>> Handle(GetDenialReasonsByInsuranceQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetDenialByInsuranceAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<DenialReasonsByInsuranceSummary>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
