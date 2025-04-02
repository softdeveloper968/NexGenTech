using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Client.Shared.Models.DashboardPresets;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ARAgingDataQuery : IRequest<Result<ARAgingDataResponse>>
    {
        public DashboardPresets.PresetFilterTypeEnum PresetFilterType { get; set; } = DashboardPresets.PresetFilterTypeEnum.BilledOnDate;
        public ARAgingReportDayGroupEnum ARAgingReportDayGroupEnum { get; set; }
        public string ClientInsuranceIds { get; set; } = string.Empty;
        public string ClientLocationIds { get; set; } = string.Empty;
        public string ClientProviderIds { get; set; } = string.Empty;
        public string ExceptionReasonCategoryIds { get; set; } = string.Empty;
        public string AuthTypeIds { get; set; } = string.Empty;
        public string ProcedureCodes { get; set; } = string.Empty;
        public ARAgingDataQuery() { }
    }
    public class ARAgingDataQueryHandler : IRequestHandler<ARAgingDataQuery, Result<ARAgingDataResponse>>
    {
        private readonly IARAgingReportQueryService _agingSummaryReportQueryService;

        public ARAgingDataQueryHandler(IARAgingReportQueryService agingSummaryReportQueryService) => _agingSummaryReportQueryService = agingSummaryReportQueryService;

        public async Task<Result<ARAgingDataResponse>> Handle(ARAgingDataQuery query, CancellationToken cancellationToken)
        {
            try
            {
                string filterReportBy = StoreProcedureTitle.BilledOnDate;
                if (query.PresetFilterType == DashboardPresets.PresetFilterTypeEnum.ServiceDate)
                {
                    filterReportBy = StoreProcedureTitle.DateOfService;
                }

                ///Call AR Aging Report service and get data using stored procedure.
                var claimStatusReportList = await _agingSummaryReportQueryService.GetARAgingChartTotalsAsync(query, filterReportBy) ?? new ARAgingDataResponse();
                return await Result<ARAgingDataResponse>.SuccessAsync(claimStatusReportList);

            }
            catch (Exception ex)
            {
                return await Result<ARAgingDataResponse>.FailAsync($"Error returning A/r Aging Totals Query: {ex.Message}");
            }
        }
    }
}
