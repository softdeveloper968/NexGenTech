namespace MedHelpAuthorizations.Application.Features.Reports.CurrentSummary
{
    public class PivotTableDataField
    {
        public string FieldName { get; set; }
        public string Format { get; set; }
        public string FieldHeader { get; set; }
        public PivotTableFieldType Type { get; set; }
        public PivotTableDataField(string fieldName, string format, string fieldHeader, PivotTableFieldType type)
        {
            FieldName = fieldName;
            Format = format;
            FieldHeader = fieldHeader;
            Type = type;
        }
    }
}