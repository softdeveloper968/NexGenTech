using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export.ClaimStatus;
using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using self_pay_eligibility_api.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Admin.Tenant.Queries
{
    public class GetInProcessClaimsReportQuery : IRequest<string>
    {
    }

    public class GetInProcessClaimsReportQueryHandler : IRequestHandler<GetInProcessClaimsReportQuery, string>
    {
        private readonly IAdminUnitOfWork _adminUnitOfWork;
        private readonly ITenantManagementService _tenantManagementService;
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IExcelService _excelService;

        public GetInProcessClaimsReportQueryHandler(IAdminUnitOfWork adminUnitOfWork, ITenantManagementService tenantManagementService, IClaimStatusQueryService claimStatusQueryService, IExcelService exceService)
        {
            _adminUnitOfWork = adminUnitOfWork;
            _tenantManagementService = tenantManagementService;
            _claimStatusQueryService = claimStatusQueryService;
            _excelService = exceService;
        }
        public async Task<string> Handle(GetInProcessClaimsReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tenants = await _tenantManagementService.GetAllActiveAsync();

                // List of sheet names for the export
                List<string> sheetsName = new();

                // Create a list containing the data to be exported, including summary and detail items.
                var exportDetails = new List<IEnumerable<ExportQueryResponse>>();

                List<Dictionary<string, Func<ExportQueryResponse, object>>> MappingsList = new List<Dictionary<string, Func<ExportQueryResponse, object>>>();

                foreach (var tenant in tenants ?? new List<Application.Multitenancy.TenantDto>())
                {
                    var connectionString = tenant.ConnectionString;
                    // Retrieve in-process details
                    var inProcessDetails = await _claimStatusQueryService.GetInProcessClaimsReportAsync(tenant);

                    // Retrieve export configuration for in-process details
                    var inProcessExcel = _claimStatusQueryService.GetExportInProcessReportExcel();
                    sheetsName.Add(tenant.TenantName);
                    exportDetails.Add(inProcessDetails);
                    MappingsList.Add(inProcessExcel.ToDictionary(summaryKey => summaryKey.Key, summary => (Func<ExportQueryResponse, object>)(exp => summary.Value((ExportQueryResponse)exp))));
                }
                // Export data to Excel with multiple sheets
                return await _excelService.ExportMultipleSheetsAsync(exportDetails, MappingsList, sheetsName, boldLastRow: false, applyBoldRowInFirstDataModel: true, applyBoldHeader: false, groupByKeySelector: x => x.PayerName, hasGroupByKeySelector: false).ConfigureAwait(true);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
