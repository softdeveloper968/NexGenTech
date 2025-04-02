using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetExecutiveDenialRateQuery : IExecutiveDashboardQueryBase, IRequest<Result<List<ExecutiveClaimRate>>>
    {
        public int ClientId { get; set; }

    }

    public class GetExecutiveDenialRateQueryHandler : IRequestHandler<GetExecutiveDenialRateQuery, Result<List<ExecutiveClaimRate>>>
    {
        private readonly IExecutiveDashboardService _executiveService;
        public GetExecutiveDenialRateQueryHandler(IExecutiveDashboardService executiveService)
        {
            _executiveService = executiveService;
        }

        public async Task<Result<List<ExecutiveClaimRate>>> Handle(GetExecutiveDenialRateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _executiveService.GetDenialClaimRateAsync(request) ?? new List<ExecutiveClaimRate>();
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
