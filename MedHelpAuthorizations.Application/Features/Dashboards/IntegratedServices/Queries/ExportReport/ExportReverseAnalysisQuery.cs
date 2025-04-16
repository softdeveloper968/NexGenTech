using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Client.Shared.Models.DashboardPresets;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static MedHelpAuthorizations.Shared.Constants.Permission.Permissions;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport
{
    public class ExportReverseAnalysisQuery : IRequest<string>
    {
        public DashboardPresets.PresetFilterTypeEnum PresetFilterType { get; set; } = DashboardPresets.PresetFilterTypeEnum.BilledOnDate;
        public ARAgingReportDayGroupEnum ARAgingReportDayGroupEnum { get; set; }
        public string ClientInsuranceIds { get; set; } = string.Empty;
        public string ClientLocationIds { get; set; } = string.Empty;
        public string ClientProviderIds { get; set; } = string.Empty;
        public string ExceptionReasonCategoryIds { get; set; } = string.Empty;
        public string AuthTypeIds { get; set; } = string.Empty;
        public string ProcedureCodes { get; set; } = string.Empty;
    }
    /// <summary>
    /// Handles the export functionality for Reverse Analysis data.
    /// </summary>
    public class ExportReverseAnalysisQueryHandler : IRequestHandler<ExportReverseAnalysisQuery, string>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportReverseAnalysisQueryHandler> _localizer;
        private readonly IARAgingReportQueryService _agingSummaryReportQueryService;

        // Constructor for ExportReverseAnalysisQuery.
        // Parameters:
        //   - claimStatusQueryService: Service for executing Claim Status queries.
        //   - excelService: Service for Excel-related operations.
        //   - localizer: Localizer for handling string localization.
        public ExportReverseAnalysisQueryHandler(IClaimStatusQueryService claimStatusQueryService, IARAgingReportQueryService agingSummaryReportQueryService, IExcelService excelService, IStringLocalizer<ExportReverseAnalysisQueryHandler> localizer)
        {
            _claimStatusQueryService = claimStatusQueryService;
            _excelService = excelService;
            _localizer = localizer;
            _agingSummaryReportQueryService = agingSummaryReportQueryService;
        }

        /// <summary>
        /// Handles the export request for Reverse Analysis data.
        /// </summary>
        /// <param name="request">The export query containing filter parameters.</param>
        /// <param name="cancellationToken">Token for canceling the operation.</param>
        /// <returns></returns>
        public async Task<string> Handle(ExportReverseAnalysisQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string filterReportBy = StoreProcedureTitle.BilledOnDate;
                if (request.PresetFilterType == DashboardPresets.PresetFilterTypeEnum.ServiceDate)
                {
                    filterReportBy = StoreProcedureTitle.DateOfService;
                }

                var query = new ARAgingDataQuery()
                {
                    PresetFilterType = request.PresetFilterType,
                    ARAgingReportDayGroupEnum = request.ARAgingReportDayGroupEnum,
                    ClientInsuranceIds = request.ClientInsuranceIds,
                    ClientLocationIds = request.ClientLocationIds,
                    ClientProviderIds = request.ClientProviderIds,
                    ExceptionReasonCategoryIds = request.ExceptionReasonCategoryIds,
                    AuthTypeIds = request.AuthTypeIds,
                    ProcedureCodes = request.ProcedureCodes
                };
                ///Call AR Aging Report service and get data using stored procedure.
                ARAgingDataResponse claimStatusReportList = await _agingSummaryReportQueryService.GetARAgingChartTotalsAsync(query, filterReportBy) ?? new ARAgingDataResponse();
                var arAgingDetails = claimStatusReportList.ARAgingData?.Select(ar => new ExportQueryResponse
                {
                    Quantity = (int)ar.Quantity,
                    ChargedSum = ar.ChargedSum,
                    LocationName = ar.LocationName,
                    ProviderName = ar.ProviderName,
                    ClaimBilledOnString = ar.ClaimBilledOn,
                    DateOfServiceFromString = ar.DateOfServiceFrom,
                    ClaimLineItemStatusId = ar.ClaimLineItemStatusId,
                    MyProperty = ar.MyProperty
                }) ?? new List<ExportQueryResponse>();///EN-308

                // Get the Excel report mappings for the summary data.
                var excelReport = _claimStatusQueryService.GetExcelReverseAnalysisReport();

                // Export data to Excel using the Excel service
                var exportData = await _excelService.ExportAsync(arAgingDetails, workSheetName: _localizer["Export Details"], mappers: excelReport, sheetName: _localizer["Reverse Analysis Report"]).ConfigureAwait(true);

                // Return the exported data as a string
                return exportData;
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, or rethrow if necessary
                throw;
            }
        }
    }
}
