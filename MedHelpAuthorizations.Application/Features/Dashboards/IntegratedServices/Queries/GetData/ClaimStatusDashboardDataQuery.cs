using System.Threading;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ClaimStatusDashboardQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<ClaimStatusDashboardResponse>>
    {
    }

    public class ClaimStatusDashboardQueryHandler : IRequestHandler<ClaimStatusDashboardQuery, Result<ClaimStatusDashboardResponse>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ClaimStatusDashboardQueryHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public ClaimStatusDashboardQueryHandler(IStringLocalizer<ClaimStatusDashboardQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService, ICurrentUserService currentUserService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
            _currentUserService = currentUserService;
        }

        public async Task<Result<ClaimStatusDashboardResponse>> Handle(ClaimStatusDashboardQuery query, CancellationToken cancellationToken)
        {
            var response = await _claimStatusQueryService.GetClaimsStatusTotalsAsync(query, _clientId);

            return await Result<ClaimStatusDashboardResponse>.SuccessAsync(response);
        }
    }
}