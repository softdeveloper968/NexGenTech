using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetExecutiveCleanClaimRateQuery : IExecutiveDashboardQueryBase, IRequest<Result<List<ExecutiveClaimRate>>>
    {
        public int ClientId { get; set; }

    }

    public class GetExecutiveCleanCLaimRateQueryHandler : IRequestHandler<GetExecutiveCleanClaimRateQuery, Result<List<ExecutiveClaimRate>>>
    {
        private readonly IExecutiveDashboardService _executiveService;
        public GetExecutiveCleanCLaimRateQueryHandler(IExecutiveDashboardService executiveService)
        {
            _executiveService = executiveService;
        }

        public async Task<Result<List<ExecutiveClaimRate>>> Handle(GetExecutiveCleanClaimRateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _executiveService.GetCleanClaimRateAsync(request) ?? new List<ExecutiveClaimRate>();
                return await Result<List<ExecutiveClaimRate>>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
