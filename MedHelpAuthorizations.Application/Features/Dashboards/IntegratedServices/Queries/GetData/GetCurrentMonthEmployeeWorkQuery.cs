using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetCurrentMonthEmployeeWorkQuery : ICorporateDashboardQueryBase, IRequest<Result<List<MonthlyEmployeeSummary>>>
    {
    }

    public class GetCurrentMonthEmployeeWorkQueryHandler : IRequestHandler<GetCurrentMonthEmployeeWorkQuery, Result<List<MonthlyEmployeeSummary>>>
    {
        private readonly ICorporateDashboardService _corporateService;
        public GetCurrentMonthEmployeeWorkQueryHandler(ICorporateDashboardService corporateService)
        {
            _corporateService = corporateService;
        }

        public async Task<Result<List<MonthlyEmployeeSummary>>> Handle(GetCurrentMonthEmployeeWorkQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _corporateService.GetCurrentMonthEmployeeWorkAsync(request) ?? new List<MonthlyEmployeeSummary>();
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
