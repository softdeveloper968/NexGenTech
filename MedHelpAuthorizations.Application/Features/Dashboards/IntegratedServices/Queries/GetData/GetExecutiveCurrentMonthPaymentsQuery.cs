using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Executive;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetExecutiveCurrentMonthPaymentsQuery : IExecutiveDashboardQueryBase, IRequest<Result<List<ExecutiveSummary>>>
    {
        public int ClientId { get; set; }

    }

    public class GetExecutiveCurrentMonthPaymentsQueryHandler : IRequestHandler<GetExecutiveCurrentMonthPaymentsQuery, Result<List<ExecutiveSummary>>>
    {
        private readonly IExecutiveDashboardService _executiveService;
        public GetExecutiveCurrentMonthPaymentsQueryHandler(IExecutiveDashboardService executiveService)
        {
            _executiveService = executiveService;
        }

        public async Task<Result<List<ExecutiveSummary>>> Handle(GetExecutiveCurrentMonthPaymentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _executiveService.GetCurrentMonthPaymentTotalsAsync(request) ?? new List<ExecutiveSummary>();
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
