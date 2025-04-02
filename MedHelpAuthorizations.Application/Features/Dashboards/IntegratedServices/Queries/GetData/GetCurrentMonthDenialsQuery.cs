using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetCurrentMonthDenialsQuery : ICorporateDashboardQueryBase, IRequest<Result<List<MonthlyClientSummary>>>
    {
    }

    public class GetCurrentMonthDenialsQueryHandler : IRequestHandler<GetCurrentMonthDenialsQuery, Result<List<MonthlyClientSummary>>>
    {
        private readonly ICorporateDashboardService _corporateService;
        public GetCurrentMonthDenialsQueryHandler(ICorporateDashboardService corporateService)
        {
            _corporateService = corporateService;
        }

        public async Task<Result<List<MonthlyClientSummary>>> Handle(GetCurrentMonthDenialsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _corporateService.GetCurrentMonthDenialTotalsAsync(request) ?? new List<MonthlyClientSummary>();
                return await Result<List<MonthlyClientSummary>>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
