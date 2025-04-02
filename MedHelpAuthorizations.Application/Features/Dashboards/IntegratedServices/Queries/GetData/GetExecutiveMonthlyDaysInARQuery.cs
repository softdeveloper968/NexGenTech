using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetExecutiveMonthlyDaysInARQuery : IExecutiveDashboardQueryBase, IRequest<Result<MonthlyDaysInAR>>
    {
        public int ClientId { get; set; }
    }
    public class GetExecutiveMonthlyDaysInARQueryHandler : IRequestHandler<GetExecutiveMonthlyDaysInARQuery, Result<MonthlyDaysInAR>>
    {
        private readonly IExecutiveDashboardService _executiveService;
        public GetExecutiveMonthlyDaysInARQueryHandler(IExecutiveDashboardService executiveService)
        {
            _executiveService = executiveService;
        }

        public async Task<Result<MonthlyDaysInAR>> Handle(GetExecutiveMonthlyDaysInARQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _executiveService.GetMonthlyDaysInARAsync(request) ?? new MonthlyDaysInAR();
                return await Result<MonthlyDaysInAR>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
