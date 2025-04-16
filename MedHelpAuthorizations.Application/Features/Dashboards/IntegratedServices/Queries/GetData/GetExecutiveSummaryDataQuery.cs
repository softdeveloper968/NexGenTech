using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Models.Executive;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetExecutiveSummaryDataQuery : IExecutiveDashboardQueryBase, IRequest<Result<List<ExecutiveSummary>>>
    {
        public int ClientId { get; set; }
    }

    public class GetExecutiveSummaryDataQueryHandler : IRequestHandler<GetExecutiveSummaryDataQuery, Result<List<ExecutiveSummary>>>
    {
        private readonly IExecutiveDashboardService _executiveDashboardService;
        public GetExecutiveSummaryDataQueryHandler(IExecutiveDashboardService executiveDashboardService)
        {
            _executiveDashboardService = executiveDashboardService;
        }

        public async Task<Result<List<ExecutiveSummary>>> Handle(GetExecutiveSummaryDataQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _executiveDashboardService.GetExecutiveCurrentMonthDataAsync(request) ?? new List<ExecutiveSummary>();
                return await Result<List<ExecutiveSummary>>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
