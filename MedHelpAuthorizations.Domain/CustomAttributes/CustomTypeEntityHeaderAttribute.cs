using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MedHelpAuthorizations.Domain.CustomAttributes
{
    /// <summary>
    /// Represents a custom report type entity header attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class CustomReportTypeEntityHeaderAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        [Required]
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether sub-entities exist.
        /// </summary>
        public bool SubEntityExist { get; set; } = false;

        /// <summary>
        /// Gets or sets the custom type code for the entity.
        /// </summary>
        public CustomTypeCode TypeCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomReportTypeEntityHeaderAttribute"/> class with specified parameters.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="typeCode">The custom type code for the entity.</param>
        /// <param name="hasSubEntityExist">A value indicating whether sub-entities exist.</param>
        public CustomReportTypeEntityHeaderAttribute(string entityName, CustomTypeCode typeCode, bool hasSubEntityExist)
        {
            EntityName = entityName;
            TypeCode = typeCode;
            SubEntityExist = hasSubEntityExist;
        }
    }


    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class CustomReportTypeColumnsHeaderForMainEntityAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the property name.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the entity name.
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the custom type code.
        /// </summary>
        public CustomTypeCode TypeCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the attribute has a relative date range.
        /// </summary>
        public bool HasRelativeDateRange { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the attribute has a custom date range.
        /// </summary>
        public bool HasCustomDateRange { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the attribute has a combined date range.
        /// </summary>
        public bool HasCombinedDateRange { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the attribute is selected in a custom report choose column.
        /// </summary>
        public bool SelectedCustomReportChooseColumn { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the attribute's column index has changed in a custom report.
        /// </summary>
        public bool SelectedCustomReportColumnIndexChanged { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the attribute is selected in a custom report set filter column.
        /// </summary>
        public bool SelectedCustomReportSetFilterColumn { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the attribute has a visible column.
        /// </summary>
        public bool HasVisibleColumn { get; set; } = false;
        public CustomReportTypeColumnsHeaderForMainEntityAttribute<ClaimStatusExceptionReasonCategoryEnum> EnumTypeProperties { get; set; }

        public CustomReportTypeColumnsHeaderForMainEntityAttribute(string entityName, CustomTypeCode typeCode, string propertyName, bool hasRelativeDateRange = false, bool hasCustomDateRange = false, bool hasDateRangeCombined = false)
        {
            PropertyName = propertyName;
            EntityName = entityName;
            TypeCode = typeCode;

            if (typeCode == CustomTypeCode.DateRangeType || typeCode == CustomTypeCode.DateTime)
            {
                HasCustomDateRange = hasCustomDateRange;
                HasRelativeDateRange = hasRelativeDateRange;
                HasCombinedDateRange = hasDateRangeCombined;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class CustomReportTypeColumnsHeaderForMainEntityAttribute<TEnum> : Attribute where TEnum : Enum
    {
        public string PropertyName { get; set; }
        public string EntityName { get; set; }
        public CustomTypeCode TypeCode { get; set; }
        public List<TEnum> CustomPropertyEnumDetails { get; set; }
        public bool HasPropertyEnumType { get; set; } = false;
        public bool SelectedCustomReportChooseColumn { get; set; } = false;
        public bool SelectedCustomReportSetFilterColumn { get; set; } = false;

        public CustomReportTypeColumnsHeaderForMainEntityAttribute(string entityName, CustomTypeCode typeCode, string propertyName, bool hasPropertyEnumType = false)
        {
            PropertyName = propertyName;
            EntityName = entityName;
            TypeCode = typeCode;

            if (hasPropertyEnumType)
            {
                CustomPropertyEnumDetails = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList(); ;
                HasPropertyEnumType = hasPropertyEnumType;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class CustomTypeSubEntityAttribute : Attribute
    {
        public string EntityName { get; set; }
        public string SubEntityName { get; set; }
        public CustomTypeCode CustomTypeCode { get; set; }

        public CustomTypeSubEntityAttribute(string entityName, string subEntityName, CustomTypeCode customTypeCode)
        {
            EntityName = entityName;
            SubEntityName = subEntityName;
            CustomTypeCode = customTypeCode;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class CustomTypeNestedSubEntityAttribute : Attribute
    {
        public string SubEntityName { get; set; }
        public string EntityName { get; set; }
        public CustomTypeCode CustomTypeCode { get; set; }

        public CustomTypeNestedSubEntityAttribute(string entityName, string subEntityName, CustomTypeCode customTypeCode)
        {
            EntityName = entityName;
            SubEntityName = subEntityName;
            CustomTypeCode = customTypeCode;
        }

    }
    public enum CustomTypeCode : int
    {
        Empty = 0,
        Object = 1,
        DBNull = 2,
        Boolean = 3,
        Char = 4,
        SByte = 5,
        Byte = 6,
        Int16 = 7,
        UInt16 = 8,
        Int32 = 9,
        UInt32 = 10,
        Int64 = 11,
        UInt64 = 12,
        Single = 13,
        Double = 14,
        Decimal = 15,
        DateTime = 16,
        String = 17,
        DateRangeType = 18,
        DateRangeTypeCombined = 19,
        FKRelationSubEntity = 20,
        EnumType = 21,
    }

    public class CustomPropertyInfo
    {
        public string MainEntityPropertyName { get; set; }
        public string SubEntityPropertyName { get; set; }
        public string PropertyName { get; set; }
        public Type PropertyType { get; set; }
        public bool HasNullablePropertyType { get; set; } = false;
    }


}