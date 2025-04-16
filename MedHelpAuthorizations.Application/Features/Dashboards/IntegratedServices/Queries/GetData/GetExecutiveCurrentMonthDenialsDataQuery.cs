using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Models.Executive;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetExecutiveCurrentMonthDenialsDataQuery : IExecutiveDashboardQueryBase, IRequest<Result<List<ExecutiveSummary>>>
    {
        public int ClientId { get; set; }

    }
    public class GetExecutiveCurrentMonthDenialsDataQueryHandler : IRequestHandler<GetExecutiveCurrentMonthDenialsDataQuery, Result<List<ExecutiveSummary>>>
    {
        private readonly IExecutiveDashboardService _executiveService;
        public GetExecutiveCurrentMonthDenialsDataQueryHandler(IExecutiveDashboardService executiveService)
        {
            _executiveService = executiveService;
        }

        public async Task<Result<List<ExecutiveSummary>>> Handle(GetExecutiveCurrentMonthDenialsDataQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _executiveService.GetCurrentMonthDenialTotalsAsync(request) ?? new List<ExecutiveSummary>();
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
