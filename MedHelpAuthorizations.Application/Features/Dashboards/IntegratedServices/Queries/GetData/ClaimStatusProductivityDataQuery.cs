using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ClaimStatusProductivityDataQuery : ClaimStatusDashboardQueryBase, IRequest<Result<ClaimStatusProductivityDataResponse>>, IClaimStatusDashboardInitialQuery
    {
        //public int? ClientProviderId { get; set; } = 0;
        //public int? ClientLocationId { get; set; } = 0;

        ////multi-select client insurance Ids
        //public string ClientInsuranceIds { get; set; } = string.Empty;
        //public string ExceptionReasonCategoryIds { get; set; } = string.Empty;
        //public string AuthTypeIds { get; set; } = string.Empty;
        //public string ProcedureCodes { get; set; } = string.Empty;
        //public string ClientLocationIds { get; set; } = string.Empty;
        //public string ClientProviderIds { get; set; } = string.Empty;
    }
    public class ClaimStatusProductivityDataQueryHandler : IRequestHandler<ClaimStatusProductivityDataQuery, Result<ClaimStatusProductivityDataResponse>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ClaimStatusProductivityDataQueryHandler> _localizer;

        public ClaimStatusProductivityDataQueryHandler(IStringLocalizer<ClaimStatusProductivityDataQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<ClaimStatusProductivityDataResponse>> Handle(ClaimStatusProductivityDataQuery query, CancellationToken cancellationToken)
        {
            try
            {
                //var response = await _claimStatusQueryService.GetClaimsStatusProductivityDataAsync(query);
                var response = new ClaimStatusProductivityDataResponse();
                return await Result<ClaimStatusProductivityDataResponse>.SuccessAsync(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Result<ClaimStatusProductivityDataResponse>.FailAsync(e.Message);
            }
        }
    }

}
    