using OfficeOpenXml.Table.PivotTable;

namespace MedHelpAuthorizations.Application.Features.Reports.CurrentSummary
{
    public class ExcelPivotTableConfiguration
    {
        public string SheetTitle { get; set; }
        public string PivotTableTitle { get; set; }
        public string RowFieldName { get; set; }
        public Action<ExcelPivotTable> ConfigurePivotTable { get; set; }

        // Parameterized constructor
        public ExcelPivotTableConfiguration(string sheetTitle, string rowFieldName, string pivotTableTitle, Action<ExcelPivotTable> configurePivotTable)
        {
            SheetTitle = sheetTitle;
            PivotTableTitle = pivotTableTitle;
            RowFieldName = rowFieldName;
            ConfigurePivotTable = configurePivotTable;
        }
    }
}
