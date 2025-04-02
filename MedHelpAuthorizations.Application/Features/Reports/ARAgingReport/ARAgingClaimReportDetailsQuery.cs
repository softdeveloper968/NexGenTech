using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Client.Shared.Models.DashboardPresets;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Reports.ARAgingReport
{
    public class ARAgingSummaryClaimReportDetailsQuery : IRequest<Result<List<ARAgingSummaryReportResponse>>>
    {
        public DashboardPresets.PresetFilterTypeEnum PresetFilterType { get; set; } = DashboardPresets.PresetFilterTypeEnum.BilledOnDate;
        public ARAgingReportDayGroupEnum ARAgingReportDayGroupEnum { get; set; }
        public string ClientInsuranceIds { get; set; } = string.Empty;
        public string ClientLocationIds { get; set; } = string.Empty;
        public string ClientProviderIds { get; set; } = string.Empty;
        public ARAgingSummaryClaimReportDetailsQuery() { }
    }

    public class ARAgingSummaryClaimReportDetailsQueryHandler : IRequestHandler<ARAgingSummaryClaimReportDetailsQuery, Result<List<ARAgingSummaryReportResponse>>>
    {
        private readonly IARAgingReportQueryService _agingSummaryReportQueryService;

        public ARAgingSummaryClaimReportDetailsQueryHandler(IARAgingReportQueryService agingSummaryReportQueryService) => _agingSummaryReportQueryService = agingSummaryReportQueryService;

        public async Task<Result<List<ARAgingSummaryReportResponse>>> Handle(ARAgingSummaryClaimReportDetailsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                string filterReportBy = "BilledOnDate";
                if (query.PresetFilterType == DashboardPresets.PresetFilterTypeEnum.ServiceDate)
                {
                    filterReportBy = "DateOfService";
                }

                ///Call AR Aging Report service and get data using stored procedure.
                var claimStatusReportList = await _agingSummaryReportQueryService.GetARAgingReportTotalsAsync(query, filterReportBy) ?? new List<ARAgingSummaryReportResponse>();
                return await Result<List<ARAgingSummaryReportResponse>>.SuccessAsync(claimStatusReportList);

            }
            catch (Exception ex)
            {
                return await Result<List<ARAgingSummaryReportResponse>>.FailAsync($"Error returning A/r Aging Summary Report Query: {ex.Message}");
            }
        }
    }
}