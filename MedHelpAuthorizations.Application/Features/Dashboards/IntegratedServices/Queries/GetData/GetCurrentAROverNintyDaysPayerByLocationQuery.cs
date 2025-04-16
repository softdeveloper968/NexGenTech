using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetCurrentAROverNintyDaysPayerByLocationQuery : IRequest<Result<List<CurrentAROverPercentageNintyDaysPayer>>>
    {
        public int ClientId { get; set; }
        public int LocationId { get; set; }

    }

    public class GetCurrentAROverNintyDaysPayerByLocationQueryHandler : IRequestHandler<GetCurrentAROverNintyDaysPayerByLocationQuery, Result<List<CurrentAROverPercentageNintyDaysPayer>>>
    {
        private readonly IExecutiveDashboardService _executiveService;
        public GetCurrentAROverNintyDaysPayerByLocationQueryHandler(IExecutiveDashboardService executiveService)
        {
            _executiveService = executiveService;
        }

        public async Task<Result<List<CurrentAROverPercentageNintyDaysPayer>>> Handle(GetCurrentAROverNintyDaysPayerByLocationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _executiveService.GetCurrentAROverPercentageNintyDaysPayerAsync(request) ?? new List<CurrentAROverPercentageNintyDaysPayer>();
                return await Result<List<CurrentAROverPercentageNintyDaysPayer>>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
