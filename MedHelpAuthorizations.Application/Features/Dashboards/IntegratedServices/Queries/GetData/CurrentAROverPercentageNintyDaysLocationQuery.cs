using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Executive;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class CurrentAROverPercentageNintyDaysLocationQuery : IExecutiveDashboardQueryBase, IRequest<Result<List<CurrentAROverPercentageNintyDaysLocation>>>
    {
        public int ClientId { get; set; }

    }

    public class CurrentAROverPercentageNintyDaysLocationQueryHandler : IRequestHandler<CurrentAROverPercentageNintyDaysLocationQuery, Result<List<CurrentAROverPercentageNintyDaysLocation>>>
    {
        private readonly IExecutiveDashboardService _executiveService;
        public CurrentAROverPercentageNintyDaysLocationQueryHandler(IExecutiveDashboardService executiveService)
        {
            _executiveService = executiveService;
        }

        public async Task<Result<List<CurrentAROverPercentageNintyDaysLocation>>> Handle(CurrentAROverPercentageNintyDaysLocationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _executiveService.GetCurrentAROverPercentageNintyDaysLocationAsync(request) ?? new List<CurrentAROverPercentageNintyDaysLocation>();
                return await Result<List<CurrentAROverPercentageNintyDaysLocation>>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
