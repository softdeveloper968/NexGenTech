using MedHelpAuthorizations.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientInsuranceFeeSchedule :  AuditableEntity<int>
    {
        public int ClientFeeScheduleId { get; set; }
        public int ClientInsuranceId { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("ClientFeeScheduleId")]
        public virtual ClientFeeSchedule ClientFeeSchedule { get; set; }

        [ForeignKey("ClientInsuranceId")]
        public virtual ClientInsurance ClientInsurance { get; set; }
    }
}
