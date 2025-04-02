using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetClientDenialReasonsByInsuranceDataQuery : ICorporateDashboardQueryBase, IRequest<Result<List<ClaimSummary>>>
    {
        public string TenantClientString { get; set; }
    }

    public class GetDenialReasonsByInsuranceDataQueryHandler : IRequestHandler<GetClientDenialReasonsByInsuranceDataQuery, Result<List<ClaimSummary>>>
    {
        private readonly ICorporateDashboardService _corporateService;
        public GetDenialReasonsByInsuranceDataQueryHandler(ICorporateDashboardService corporateService)
        {
            _corporateService = corporateService;
        }

        public async Task<Result<List<ClaimSummary>>> Handle(GetClientDenialReasonsByInsuranceDataQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _corporateService.GetDenialReasonsByInsuranceDataAsync(request) ?? new List<ClaimSummary>();

                // Return a successful result with the response data.
                return await Result<List<ClaimSummary>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
