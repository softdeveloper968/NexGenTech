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
    public class GetMonthlyDaysInARQuery : ICorporateDashboardQueryBase, IRequest<Result<MonthlyDaysInAR>>
    {
    }

    public class GetMonthlyDaysInARQueryHandler : IRequestHandler<GetMonthlyDaysInARQuery, Result<MonthlyDaysInAR>>
    {
        private readonly ICorporateDashboardService _corporateService;
        public GetMonthlyDaysInARQueryHandler(ICorporateDashboardService corporateService)
        {
            _corporateService = corporateService;
        }

        public async Task<Result<MonthlyDaysInAR>> Handle(GetMonthlyDaysInARQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _corporateService.GetMonthlyDaysInARAsync(request) ?? new MonthlyDaysInAR();
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
