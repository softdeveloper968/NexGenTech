using MedHelpAuthorizations.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class EmployeeClientInsurance : AuditableEntity<int>
    {
        public int EmployeeClientId { get; set; }

        public int ClientInsuranceId { get; set; }


        [ForeignKey("EmployeeClientId")]
        public virtual EmployeeClient EmployeeClient { get; set; }

        [ForeignKey("ClientInsuranceId")]
        public virtual ClientInsurance ClientInsurance { get; set; }
    }
}
