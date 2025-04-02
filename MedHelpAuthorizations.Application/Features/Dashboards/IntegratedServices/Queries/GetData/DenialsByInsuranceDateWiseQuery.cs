using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class DenialsByInsuranceDateWiseQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<DenialClaimSummary>>>
    {

        public class DenialsByInsuranceDateWiseQueryHandler : IRequestHandler<DenialsByInsuranceDateWiseQuery, Result<List<DenialClaimSummary>>>
        {
            private readonly IClaimStatusQueryService _claimStatusQueryService;
            private readonly IStringLocalizer<DenialsByInsuranceDateWiseQueryHandler> _localizer;

            // Constructor for the query handler.
            public DenialsByInsuranceDateWiseQueryHandler(IStringLocalizer<DenialsByInsuranceDateWiseQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
            {
                _localizer = localizer;
                _claimStatusQueryService = claimStatusQueryService;
            }

            public async Task<Result<List<DenialClaimSummary>>> Handle(DenialsByInsuranceDateWiseQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    // Retrieve claim status revenue totals using the claimStatusQueryService.
                    var response = await _claimStatusQueryService.GetDenialsByInsuranceDateWiseAsync(query) ?? new List<DenialClaimSummary>();

                    // Return a successful result with the response data.
                    return await Result<List<DenialClaimSummary>>.SuccessAsync(response);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

    }
}
