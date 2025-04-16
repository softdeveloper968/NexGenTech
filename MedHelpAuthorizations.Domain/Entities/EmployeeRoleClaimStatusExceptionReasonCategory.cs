using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class EmployeeRoleClaimStatusExceptionReasonCategory : AuditableEntity<int>
    {
            public EmployeeRoleClaimStatusExceptionReasonCategory() 
            { 
            }
            public EmployeeRoleEnum EmployeeRoleId { get; set; }

            public ClaimStatusExceptionReasonCategoryEnum ClaimStatusExceptionReasonCategoryId { get; set; }

            [ForeignKey("EmployeeRoleId")]
            public virtual EmployeeRole EmployeeRole { get; set; }


            [ForeignKey("ClaimStatusExceptionReasonCategoryId")]
            public virtual ClaimStatusExceptionReasonCategory ClaimStatusExceptionReasonCategory { get; set; }        
    }
}