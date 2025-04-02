using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Client.Shared.Models.DashboardPresets;
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
    public class ClaimStatusDateLagQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ClaimStatusDateLagResponse>>>
    {
        public int ClaimStatusBatchId { get; set; } = 0;
        public int ClientLocationId { get; set; } = 0;
        public int ClientProviderId { get; set; } = 0;
        public string ProviderName { get; set; }
        public string LocationName { get; set; }
        public int? PatientId { get; set; }
    }
    public class ClaimStatusDateLagQueryHandler : IRequestHandler<ClaimStatusDateLagQuery, Result<List<ClaimStatusDateLagResponse>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ClaimStatusDateLagQueryHandler> _localizer;

        public ClaimStatusDateLagQueryHandler(IStringLocalizer<ClaimStatusDateLagQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }
        public async Task<Result<List<ClaimStatusDateLagResponse>>> Handle(ClaimStatusDateLagQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _claimStatusQueryService.GetClaimStatusDateLagAsync(query);

                return await Result<List<ClaimStatusDateLagResponse>>.SuccessAsync(response);

            }
            catch (Exception ex)
            {
                return await Result<List<ClaimStatusDateLagResponse>>.FailAsync($"Error returning Claim Status Date Lag Response: {ex.Message}");
            }
        }
    }
}
