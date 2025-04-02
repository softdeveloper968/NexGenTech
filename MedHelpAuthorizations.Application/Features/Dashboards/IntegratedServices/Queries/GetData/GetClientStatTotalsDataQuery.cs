using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetClientStatTotalsDataQuery : ICorporateDashboardQueryBase, IRequest<Result<ClaimStatusDashboardData>>
    {
        public string TenantClientString { get; set; }
    }

    public class GetClilentStatTotalsDataQueryHandler : IRequestHandler<GetClientStatTotalsDataQuery, Result<ClaimStatusDashboardData>>
    {
        private readonly ICorporateDashboardService _corporateService;
        public GetClilentStatTotalsDataQueryHandler(ICorporateDashboardService corporateService)
        {
            _corporateService = corporateService;
        }

        public async Task<Result<ClaimStatusDashboardData>> Handle(GetClientStatTotalsDataQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _corporateService.GetClientStatTotalsDataAsync(request) ?? new ClaimStatusDashboardData();
                return await Result<ClaimStatusDashboardData>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
