using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientFeeScheduleSpecialty :  AuditableEntity<int>
    {
        public int ClientFeeScheduleId { get; set; }

        public SpecialtyEnum SpecialtyId { get; set; }


        [ForeignKey("ClientFeeScheduleId")]
        public virtual ClientFeeSchedule ClientFeeSchedule { get; set; }


		[ForeignKey("SpecialtyId")]
		public virtual Specialty Specialty { get; set; }
	}
}
