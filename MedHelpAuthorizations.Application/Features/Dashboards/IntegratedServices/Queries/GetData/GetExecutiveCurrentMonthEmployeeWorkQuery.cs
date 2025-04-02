using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetExecutiveCurrentMonthEmployeeWorkQuery : IExecutiveDashboardQueryBase, IRequest<Result<List<MonthlyEmployeeSummary>>>
    {
        public int ClientId { get; set; }

    }

    public class GetExecutiveCurrentMonthEmployeeWorkQueryHandler : IRequestHandler<GetExecutiveCurrentMonthEmployeeWorkQuery, Result<List<MonthlyEmployeeSummary>>>
    {
        private readonly IExecutiveDashboardService _executiveService;
        public GetExecutiveCurrentMonthEmployeeWorkQueryHandler(IExecutiveDashboardService executiveService)
        {
            _executiveService = executiveService;
        }

        public async Task<Result<List<MonthlyEmployeeSummary>>> Handle(GetExecutiveCurrentMonthEmployeeWorkQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _executiveService.GetCurrentMonthEmployeeWorkAsync(request) ?? new List<MonthlyEmployeeSummary>();
                return await Result<List<MonthlyEmployeeSummary>>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
