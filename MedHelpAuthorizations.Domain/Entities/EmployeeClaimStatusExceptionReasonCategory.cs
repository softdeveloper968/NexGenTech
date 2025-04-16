using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class EmployeeClaimStatusExceptionReasonCategory : AuditableEntity<int>
    {
        public int EmployeeClientId { get; set; }
        public ClaimStatusExceptionReasonCategoryEnum? ClaimStatusExceptionReasonCategoryId { get; set; }

        [ForeignKey("EmployeeClientId")]
        public virtual EmployeeClient EmployeeClient { get; set; }

        [ForeignKey("ClaimStatusExceptionReasonCategoryId")]
        public virtual ClaimStatusExceptionReasonCategory ClaimStatusExceptionReasonCategory { get; set; }

    }
}
