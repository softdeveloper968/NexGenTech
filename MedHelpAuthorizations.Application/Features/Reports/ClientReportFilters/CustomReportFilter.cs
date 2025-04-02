using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Reports.CustomReports.Query;
using MedHelpAuthorizations.Shared.Enums;

namespace MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters
{
    /// <summary>
    /// Save CustomReport Payload
    /// </summary>
    public class SaveCustomReportPayload
    {
        public SaveCustomReportPayload() { }
        /// <summary>
        /// Custom Report Title
        /// </summary>
        public string CustomReportTitle { get; set; }
        /// <summary>
        /// Custom Report Type
        /// <list type="bullet">
        /// <item>Claim</item>
        /// <item>AR Aging</item>
        /// <item>Daily Claim Report</item>
        /// <item>Claim Type</item>
        /// </list>
        /// </summary>
        public CustomReportTypeEnum ReportType { get; set; }
        /// <summary>
        /// List of selected control names.
        /// </summary>
        public List<string> SelectedControlNames { get; set; }

        /// <summary>
        /// Details for choosing columns.
        /// </summary>
        public List<CustomAttributeForEntitesDataItem> ChooseColumnsDetails { get; set; }

        /// <summary>
        /// List of selected columns to be displayed.
        /// </summary>
        public List<CustomReportSelectedColumns> DisplaySelectedColumns { get; set; }

        /// <summary>
        /// Details for setting filter columns.
        /// </summary>
        public List<CustomReportSetFilterColumns> SetFilterColumnsDetails { get; set; }

        /// <summary>
        /// Custom model for setting filters.
        /// </summary>
        public Dictionary<string, object> SetFiltersCustomModel { get; set; }

        /// <summary>
        /// SQL query for previewing the report.
        /// </summary>
        public string SQLQueryForPreviewReport { get; set; } = string.Empty;

        /// <summary>
        /// Custom date query for the WHERE clause.
        /// </summary>
        public string CustomDateQueryForWhereClause { get; set; } = string.Empty;
    }
}
