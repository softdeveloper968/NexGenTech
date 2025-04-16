using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Corporate;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetDenialRateQuery : ICorporateDashboardQueryBase, IRequest<Result<List<ClaimRate>>>
    {
    }

    public class GetDenialRateQueryHandler : IRequestHandler<GetDenialRateQuery, Result<List<ClaimRate>>>
    {
        private readonly ICorporateDashboardService _corporateService;
        public GetDenialRateQueryHandler(ICorporateDashboardService corporateService)
        {
            _corporateService = corporateService;
        }

        public async Task<Result<List<ClaimRate>>> Handle(GetDenialRateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _corporateService.GetDenialClaimRateAsync(request) ?? new List<ClaimRate>();
                return await Result<List<ClaimRate>>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
