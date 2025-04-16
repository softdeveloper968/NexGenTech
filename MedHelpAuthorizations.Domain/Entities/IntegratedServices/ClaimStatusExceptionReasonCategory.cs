// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClaimStatusExceptionReasonCategory.cs" company="Automated Integration Technologies">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MedHelpAuthorizations.Domain.Contracts;
    using MedHelpAuthorizations.Domain.CustomAttributes;
    using MedHelpAuthorizations.Domain.Entities.Enums;
    using MedHelpAuthorizations.Domain.IntegratedServices;
    using MedHelpAuthorizations.Shared.Helpers;

    [CustomReportTypeEntityHeader(CustomReportHelper._ClaimStatusExceptionReasonCategory, CustomTypeCode.Empty, false)]
    public class ClaimStatusExceptionReasonCategory : AuditableEntity<ClaimStatusExceptionReasonCategoryEnum>
    {
        public ClaimStatusExceptionReasonCategory()
        {
            this.ClaimStatusExceptionReasonCategoryMaps = new HashSet<ClaimStatusExceptionReasonCategoryMap>();
        }

        public ClaimStatusExceptionReasonCategory(ClaimStatusExceptionReasonCategoryEnum id, string code, string description)
        {
            Id = id;
            Code = code;    
            Description = description;
        }

        [StringLength(30)]
        public string Code { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(CustomReportHelper._ClaimStatusExceptionReasonCategory,CustomTypeCode.String,CustomReportHelper.ClaimStatusExceptionReasonCategoryDescription)]
        public string Description { get; set; }

        #region Navigation Objects

        public virtual ICollection<ClaimStatusExceptionReasonCategoryMap> ClaimStatusExceptionReasonCategoryMaps { get; set; }

        #endregion
    }
}
