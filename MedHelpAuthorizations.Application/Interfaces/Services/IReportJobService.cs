using MedHelpAuthorizations.Application.Features.IntegratedServices.Charges;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export.ARAgingReport;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export.ClaimStatus;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IReportJobService
    {
        Task<string> ARAgingReportExportDetailsAsync(ARAgingReportExportDetailsQuery request, string userId, int clientId, string conn, string tenantIdentifier);
        Task<string> ARAgingReportExportSummaryAsync(ARAgingReportExportSummaryQuery request, string userId, int clientId, string conn, string tenantIdentifier);
        Task<string> ExportCashValueForRevenueAsync(ExportCashValueForRevenueQuery request, string userId, int clientId, string conn, string tenantIdentifier);
        Task<string> ExportInitialClaimStatusInProcessDetails(ExportInitialClaimStatusInProcessDetailsQuery request, string userId, int clientId, string conn, string tenantIdentifier);
        Task<string> GenerateFinancialSummaryReportAsync(FinicalSummaryExportDetailQuery request, string userId, int clientId, string tenantIdentifier, string conn = null);
        Task<string> GetAvgDayToPayReportSummaryReportAsync(ExportAvgDayToPayReportDetailsQuery request, string userId, int clientId, string conn, string tenantIdentifier);
        Task<string> GetClaimStatusDenialsAsync(ExportClaimStatusDenialsReportQuery request, string userId, int clientId, string conn, string tenantIdentifier);
        Task<string> GetClaimStatusDetailsSummaryReportAsync(ExportClaimStatusDetailsQuery request, string userId, int clientId, string conn,string tenantIdentifier);
        Task<string> GetClaimStatusInProcessDetailsReportAsync(ExportClaimStatusInProcessDetailsQuery request, string userId, int clientId, string conn, string tenantIdentifier);
        Task<string> GetClaimStatusReportAsync(ExportClaimStatusReportQuery request, string userId, int clientId, string conn, string tenantIdentifier);
        Task<string> GetCustomPaymentAndProcedureReportAsync(ExportCustomPaymentAndProcedureCodeQuery request, string userId, int clientId, string conn, string tenantIdentifier);
        Task<string> GetExportClaimStatusDenialDetailsReportAsync(ExportClaimStatusDenialDetailsQuery request, string userId, int clientId, string conn, string tenantIdentifier);
        Task<string> GetInitialClaimStatusDenialDetailsReportAsync(ExportInitialClaimStatusDenialDetailsQuery request, string userId, int clientId, string conn, string tenantIdentifier);
        Task<string> GetInitialClaimStatusDenialsReportAsync(ExportInitialClaimStatusDenialsQuery request, string userId, int clientId, string conn, string tenantIdentifier);
        Task<string> GetInitialClaimStatusReportAsync(ExportInitialClaimStatusDetailsQuery request, string userId, int clientId, string conn, string tenantIdentifier);

        Task SendReportEmailAsync(string userId, string reportDownloadUrl, string fileName, string reportBase64String); //EN-714
    }
}
