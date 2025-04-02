using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Features.Reports.ARAgingReport;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export.ARAgingReport;
using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IARAgingReportQueryService : IService
    {
        Task<List<ARAgingSummaryReportResponse>> GetARAgingReportTotalsAsync(ARAgingSummaryClaimReportDetailsQuery filters, string filterReportBy = "BilledOnDate");
        Task<ARAgingDataResponse> GetARAgingChartTotalsAsync(ARAgingDataQuery filters, string filterReportBy = "BilledOnDate"); //AA-130
        Task<List<ExportQueryResponse>> GetARAgingReportExportDetailsAsync(ARAgingReportExportDetailsQuery request, int filterDayGroupby, string filterReportBy = "BilledOnDate", string connStr = null);
        Task<List<ExportQueryResponse>> GetARAgingReportExportSummaryAsync(ARAgingReportExportDetailsQuery request, int filterDayGroupby, string filterReportBy = "BilledOnDate", string connStr = null);
        ExportSummaryReportResponse GetGrandTotal(List<ExportSummaryReportResponse> summaryData);
        Dictionary<string, Func<ExportQueryResponse, object>> GetExcelARAgingReportDetails();
        Dictionary<string, Func<ExportQueryResponse, object>> GetExcelARAgingReportSummary();
        void GetFilterDayGroupBy(ARAgingReportDayGroupEnum aRAgingReportDayGroupEnum, out int filterDayGroupby);
        List<Dictionary<string, Func<ExportQueryResponse, object>>> CombineTwoExportReportDetailModels(Dictionary<string, Func<ExportQueryResponse, object>> excelSummaryReportDetails, Dictionary<string, Func<ExportQueryResponse, object>> excelReportDetails);
        List<ExportQueryResponse> UpdateExportDetails(List<ExportQueryResponse> summaryData);
    }
}
