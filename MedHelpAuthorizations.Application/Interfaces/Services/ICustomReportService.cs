using MedHelpAuthorizations.Application.Features.Reports.CustomReports.Query;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Domain.CustomAttributes.CustomReport;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface ICustomReportService
    {
        CustomReportTypeEntity GetContextDetailForClaimReportType();
        string GenerateDynamicSQLQuery(CustomPreviewsReportQuery customPreviewsReport, out string columnsForSQLQuery, int pageNumber = 1, int pageSize = 30, bool allowLimitPagination = true);
        Task<UpdatedClaimReportTypePreviewModel> ExecutionClaimReportTypeSQLQuery(string claimSQLquery, string columnsForSQLQuery, bool allowLimitPagination = true);
        void GetChooseDisplayColumns(string headerEntityName, CustomReportTypeEntity customAttributeEntityDetails, out List<CustomAttributeForEntitesDataItem> chooseColumnsDetails);
        void GetSetFilterDisplayColumns(string headerEntityName, CustomReportTypeEntity customAttributeEntityDetails, out List<CustomReportSetFilterColumns> setFilterColumnsDetails);
        Task<string> ExecutionPreviewClaimReportTypeSQLQuery(string claimSQLquery, bool includeColumns = false);
    }
}
