using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ProviderComparisonQuery : ComparisonDashboardQuery, IRequest<Result<List<ProviderComparisonResponse>>>
    {
    }
    public class ProviderComparisonQueryHandler : IRequestHandler<ProviderComparisonQuery, Result<List<ProviderComparisonResponse>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ProviderComparisonQueryHandler> _localizer;

        public ProviderComparisonQueryHandler(IStringLocalizer<ProviderComparisonQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<ProviderComparisonResponse>>> Handle(ProviderComparisonQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _claimStatusQueryService.GetProviderComparisonDataAsync(query);

                var result =  await Result<List<ProviderComparisonResponse>>.SuccessAsync(response);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
