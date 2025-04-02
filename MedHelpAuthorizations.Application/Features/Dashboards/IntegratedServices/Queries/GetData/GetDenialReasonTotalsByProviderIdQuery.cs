using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetDenialReasonTotalsByProviderIdQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ProviderDenialReasonTotal>>>
    {
    }
    public class GetDenialReasonTotalsByProviderIdQueryHandler : IRequestHandler<GetDenialReasonTotalsByProviderIdQuery, Result<List<ProviderDenialReasonTotal>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetDenialReasonTotalsByProviderIdQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetDenialReasonTotalsByProviderIdQueryHandler(IStringLocalizer<GetDenialReasonTotalsByProviderIdQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<ProviderDenialReasonTotal>>> Handle(GetDenialReasonTotalsByProviderIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetProviderDenialReasonTotalAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<ProviderDenialReasonTotal>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
