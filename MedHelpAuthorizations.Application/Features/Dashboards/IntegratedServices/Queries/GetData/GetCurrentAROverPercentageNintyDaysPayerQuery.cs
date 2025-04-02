using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetCurrentAROverPercentageNintyDaysPayerQuery : ICorporateDashboardQueryBase, IRequest<Result<List<CurrentAROverPercentageNintyDaysPayer>>>
    {
        [Required]
        public int ClientId { get; set; }
    }

    public class GetCurrentAROverPercentageNintyDaysPayerQueryHandler : IRequestHandler<GetCurrentAROverPercentageNintyDaysPayerQuery, Result<List<CurrentAROverPercentageNintyDaysPayer>>>
    {
        private readonly ICorporateDashboardService _corporateService;
        public GetCurrentAROverPercentageNintyDaysPayerQueryHandler(ICorporateDashboardService corporateService)
        {
            _corporateService = corporateService;
        }

        public async Task<Result<List<CurrentAROverPercentageNintyDaysPayer>>> Handle(GetCurrentAROverPercentageNintyDaysPayerQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _corporateService.GetCurrentAROverPercentageNintyDaysPayerAsync(request) ?? new List<CurrentAROverPercentageNintyDaysPayer>();
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
