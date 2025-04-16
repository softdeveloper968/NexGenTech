using System.Collections.Generic;
using MedHelpAuthorizations.Domain.CustomAttributes;

namespace MedHelpAuthorizations.Application.Features.Reports.CustomReports.Query
{
    public class CustomReportChooseColumnResponseModel
    {
        /// <summary>
        /// Gets or sets the custom type code for the entity.
        /// </summary>
        public CustomTypeCode EntityType { get; set; }

        /// <summary>
        /// Gets or sets a list of entity properties.
        /// </summary>
        public List<string> EntityProperties { get; set; }

        /// <summary>
        /// Gets or sets a list of sub-entities.
        /// </summary>
        public List<string> SubEntities { get; set; }

        /// <summary>
        /// Gets or sets details of entity properties for the main entity.
        /// </summary>
        public List<CustomReportTypeColumnsHeaderForMainEntityAttribute> EntityPropertiesDetail { get; set; }

        /// <summary>
        /// Gets or sets details of the main entity.
        /// </summary>
        public CustomReportTypeEntityHeaderAttribute EntityDetail { get; set; }

        /// <summary>
        /// Gets or sets details of sub-entities.
        /// </summary>
        public List<CustomTypeSubEntityAttribute> SubEntityDetail { get; set; }

        /// <summary>
        /// Gets or sets details of entity properties for sub-entities.
        /// </summary>
        public List<CustomReportTypeColumnsHeaderForMainEntityAttribute> SubEntityPropertiesDetail { get; set; }
    }

    public class CustomReportTypeNestedAttributeColumns
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomReportTypeNestedAttributeColumns"/> class.
        /// </summary>
        public CustomReportTypeNestedAttributeColumns()
        {
        }

        /// <summary>
        /// Gets or sets the attribute for the nested property.
        /// </summary>
        public CustomTypeSubEntityAttribute NestedPropertyAttribute { get; set; }

        /// <summary>
        /// Gets or sets a list of columns for the nested attribute.
        /// </summary>
        public List<CustomReportTypeColumnsHeaderForMainEntityAttribute> NestedColumns { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the attribute has columns only.
        /// </summary>
        public bool HasColumnsOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the attribute has enum type columns.
        /// </summary>
        public bool HasEnumTypeColumns { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are nested enum type columns.
        /// </summary>
        public bool NestedEnumTypeColumns { get; set; }
    }

    /// <summary>
    /// Represents a custom report type entity.
    /// </summary>
    public class CustomReportTypeEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomReportTypeEntity"/> class.
        /// </summary>
        public CustomReportTypeEntity() { }

        /// <summary>
        /// Gets or sets the name of the main entity.
        /// </summary>
        public string MainEntityName { get; set; }

        /// <summary>
        /// Gets or sets a list of columns for the main entity.
        /// </summary>
        public List<CustomReportTypeColumnsHeaderForMainEntityAttribute> MainEntityColumns { get; set; }

        /// <summary>
        /// Gets or sets details of sub-entities.
        /// </summary>
        public List<CustomReportTypeNestedAttributeColumns> SubEntityDetails { get; set; }

        /// <summary>
        /// Gets or sets details for choosing columns.
        /// </summary>
        public List<CustomAttributeForEntitesDataItem> ChooseColumnsDetails { get; set; }

        /// <summary>
        /// Gets or sets details for setting filter columns.
        /// </summary>
        public List<CustomReportSetFilterColumns> SetFilterColumnsDetails { get; set; }
    }

    /// <summary>
    /// Represents custom report set filter columns.
    /// </summary>
    public class CustomReportSetFilterColumns : CustomAttributeForEntitesDataItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomReportSetFilterColumns"/> class.
        /// </summary>
        public CustomReportSetFilterColumns() { }
    }


}
