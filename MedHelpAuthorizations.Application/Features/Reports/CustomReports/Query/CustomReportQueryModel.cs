using System.Collections.Generic;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Shared.Enums;

namespace MedHelpAuthorizations.Application.Features.Reports.CustomReports.Query
{
    /// <summary>
    /// Represents a custom report query model.
    /// </summary>
    public class CustomReportQueryModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomReportQueryModel"/> class.
        /// </summary>
        public CustomReportQueryModel() { }

        /// <summary>
        /// Gets or sets the custom report type.
        /// </summary>
        public CustomReportTypeEnum CustomReportType { get; set; }

        /// <summary>
        /// Gets or sets the columns chosen for the report.
        /// </summary>
        public List<string> ChooseColumns { get; set; }

        /// <summary>
        /// Gets or sets the columns with filter conditions.
        /// </summary>
        public List<string> SetFilterColumns { get; set; }

        /// <summary>
        /// Gets or sets the values of columns with filter conditions.
        /// </summary>
        public List<string> SetFilterColumnValues { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are custom filter options.
        /// </summary>
        public bool HasCustomFilterOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is an existing report preview.
        /// </summary>
        public bool IsExistingReportPreview { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the report has a heading.
        /// </summary>
        public bool HasReportHeading { get; set; }

        /// <summary>
        /// Gets or sets the report heading.
        /// </summary>
        public string ReportHeading { get; set; }

        /// <summary>
        /// Gets or sets the report name.
        /// </summary>
        public string ReportName { get; set; }

        /// <summary>
        /// Gets or sets the selected column value.
        /// </summary>
        public string SelectedColumnValue { get; set; }

        /// <summary>
        /// Gets or sets a list of selected column values.
        /// </summary>
        public List<SelectedColumnValues> SelectedColumnValues { get; set; }
    }

    public class SelectedColumnValues
    {
        public string ColumnName { get; set; }
        public object ColumnValue { get; set; }
    }

    public class CustomAttributeForEntitesDataItem : CustomReportTypeEntity
    {
        public string Name { get; set; }
        public bool SelectedCustomReportColumn { get; set; } = false;
        public bool SelectedCustomReportColumnIndexChanged { get; set; } = false;
        public bool SelectedCustomReportSetFilterColumn { get; set; } = false;
        public bool SelectedCustomReportColumnHasRequestedParam { get; set; } = false;
        public bool IsLocked { get; set; } = false;
        public bool HasVisibleColumn { get; set; } = false;
        public CustomTypeCode CustomPropertyType { get; set; }
        public string TransferSlot { get; set; }

        public CustomReportTypeColumnsHeaderForMainEntityAttribute MainEntityColumn { get; set; }
        public CustomReportTypeNestedAttributeColumns SubEntityColumn { get; set; }
        public bool HasSubEntityColumn { get; set; }
    }

    public class ChooseColumnsForCustomReport
    {
        public string EntityName { get; set; }
        public string Name { get; set; }
    }

    public class CustomReportSelectedColumns : CustomAttributeForEntitesDataItem
    {

    }

}
