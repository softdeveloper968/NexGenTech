using AutoMapper;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{

    public class ClaimStatusDashboardInitialQuery : ClaimStatusDashboardQueryBase, IRequest<Result<ClaimStatusDashboardResponse>>, IClaimStatusDashboardInitialQuery
    {

    }

    public class ClaimStatusDashboardInitialQueryHandler : IRequestHandler<ClaimStatusDashboardInitialQuery, Result<ClaimStatusDashboardResponse>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ClaimStatusDashboardInitialQueryHandler> _localizer;
        private readonly IMapper _mapper;

        public ClaimStatusDashboardInitialQueryHandler(IStringLocalizer<ClaimStatusDashboardInitialQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService,IMapper mapper)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
            _mapper = mapper;
        }

        public async Task<Result<ClaimStatusDashboardResponse>> Handle(ClaimStatusDashboardInitialQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _claimStatusQueryService.GetInitialClaimsStatusTotalsAsync(query);
                return await Result<ClaimStatusDashboardResponse>.SuccessAsync(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Result<ClaimStatusDashboardResponse>.FailAsync(e.Message);
            }
        }
    }
}