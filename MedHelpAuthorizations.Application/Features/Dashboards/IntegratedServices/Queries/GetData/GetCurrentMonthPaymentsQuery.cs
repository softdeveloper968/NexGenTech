using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetCurrentMonthPaymentsQuery : ICorporateDashboardQueryBase, IRequest<Result<List<MonthlyClientSummary>>>
    {
    }

    public class GetCurrentMonthPaymentsQueryHandler : IRequestHandler<GetCurrentMonthPaymentsQuery, Result<List<MonthlyClientSummary>>>
    {
        private readonly ICorporateDashboardService _corporateService;
        public GetCurrentMonthPaymentsQueryHandler(ICorporateDashboardService corporateService)
        {
            _corporateService = corporateService;
        }

        public async Task<Result<List<MonthlyClientSummary>>> Handle(GetCurrentMonthPaymentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _corporateService.GetCurrentMonthPaymentTotalsAsync(request) ?? new List<MonthlyClientSummary>();
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
