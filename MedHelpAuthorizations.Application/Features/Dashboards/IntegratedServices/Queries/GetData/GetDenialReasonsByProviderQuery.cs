using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetDenialReasonsByProviderQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<DenialReasonsTotalsByProvider>>>
    {
    }
    public class GetDenialReasonsByProviderQueryHandler : IRequestHandler<GetDenialReasonsByProviderQuery, Result<List<DenialReasonsTotalsByProvider>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetDenialReasonsByProviderQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetDenialReasonsByProviderQueryHandler(IStringLocalizer<GetDenialReasonsByProviderQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<DenialReasonsTotalsByProvider>>> Handle(GetDenialReasonsByProviderQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetDenialReasonsByProviderAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<DenialReasonsTotalsByProvider>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
