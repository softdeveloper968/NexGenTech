using MedHelpAuthorizations.Application.Features.Reports.CurrentSummary;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport
{
    public class ReportCreationOptions
    {
        /// <summary>
        /// The data to be included in the report.
        /// </summary>
        public IEnumerable<ExportQueryResponse> Data { get; set; }
        /// <summary>
        /// A dictionary of functions to map data for the report.
        /// </summary>
        public Dictionary<string, Func<ExportQueryResponse, object>> MapperFunc { get; set; }
        /// <summary>
        /// The name of the worksheet in the Excel report.
        /// </summary>
        public string SheetName { get; set; }
        /// <summary>
        /// Optional password to secure the Excel file.
        /// </summary>
        public string PasswordString { get; set; }
        /// <summary>
        /// Optional function to group data by a specific key.
        /// </summary>
        public Func<ExportQueryResponse, object> GroupByKeySelector { get; set; }
        /// <summary>
        /// Flag indicating whether a group by key selector is provided.
        /// </summary>
        public bool HasGroupByKeySelector { get; set; } = false;
        /// <summary>
        /// Optional list of pivot table configurations.
        /// </summary>
        public List<ExcelPivotTableConfiguration> PivotTableConfigurations { get; set; }
        /// <summary>
        /// Optional dictionary to order the pivot table sheets.
        /// </summary>
        public Dictionary<string, string> PivotTableSheetOrdering { get; set; }
        /// <summary>
        /// Flag indicating whether the last row should be bolded.
        /// </summary>
        public bool BoldLastRow { get; set; } = false;
        /// <summary>
        /// Flag indicating whether the first row should be bolded.
        /// </summary>
        public bool ApplyBoldRowInFirstDataModel { get; set; } = true;
        /// <summary>
        /// Flag indicating whether the header should be bolded.
        /// </summary>
        public bool ApplyBoldHeader { get; set; } = true;
    }
}
