using System;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{

    public class ClaimStatusTrendsQuery : ClaimStatusDashboardQueryBase, IRequest<Result<ClaimStatusTrendsResponse>>, IClaimStatusDashboardInitialQuery
    {

    }

    public class ClaimStatusTrendsQueryHandler : IRequestHandler<ClaimStatusTrendsQuery, Result<ClaimStatusTrendsResponse>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ClaimStatusTrendsQueryHandler> _localizer;

        public ClaimStatusTrendsQueryHandler(IStringLocalizer<ClaimStatusTrendsQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<ClaimStatusTrendsResponse>> Handle(ClaimStatusTrendsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _claimStatusQueryService.GetInitialClaimsStatusTrendsAsync(query);
                return await Result<ClaimStatusTrendsResponse>.SuccessAsync(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Result<ClaimStatusTrendsResponse>.FailAsync(e.Message);
            }
        }
    }
}