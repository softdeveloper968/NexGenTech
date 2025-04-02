using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetCurrentAROverPercentageNintyDaysClientQuery : ICorporateDashboardQueryBase, IRequest<Result<List<CurrentAROverPercentageNintyDaysClient>>>
    {
    }

    public class GetCurrentAROverPercentageNintyDaysQueryHandler : IRequestHandler<GetCurrentAROverPercentageNintyDaysClientQuery, Result<List<CurrentAROverPercentageNintyDaysClient>>>
    {
        private readonly ICorporateDashboardService _corporateService;
        public GetCurrentAROverPercentageNintyDaysQueryHandler(ICorporateDashboardService corporateService)
        {
            _corporateService = corporateService;
        }

        public async Task<Result<List<CurrentAROverPercentageNintyDaysClient>>> Handle(GetCurrentAROverPercentageNintyDaysClientQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _corporateService.GetCurrentAROverPercentageNintyDaysClientAsync(request) ?? new List<CurrentAROverPercentageNintyDaysClient>();
                return await Result<List<CurrentAROverPercentageNintyDaysClient>>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
