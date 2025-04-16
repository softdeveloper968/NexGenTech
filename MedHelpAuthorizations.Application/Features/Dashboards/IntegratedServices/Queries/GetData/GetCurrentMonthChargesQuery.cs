using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetCurrentMonthChargesQuery : ICorporateDashboardQueryBase, IRequest<Result<List<MonthlyClientSummary>>>
    {
    }

    public class GetCurrentMonthChargesQueryHandler : IRequestHandler<GetCurrentMonthChargesQuery, Result<List<MonthlyClientSummary>>>
    {
        private readonly ICorporateDashboardService _corporateService;
        public GetCurrentMonthChargesQueryHandler(ICorporateDashboardService corporateService)
        {
            _corporateService = corporateService;
        }

        public async Task<Result<List<MonthlyClientSummary>>> Handle(GetCurrentMonthChargesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _corporateService.GetCurrentMonthChargesTotalsAsync(request) ?? new List<MonthlyClientSummary>();
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
