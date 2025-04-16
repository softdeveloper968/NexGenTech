using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientFeeScheduleProviderLevel :  AuditableEntity<int>
    {
        public int ClientFeeScheduleId { get; set; }

        public ProviderLevelEnum ProviderLevelId { get; set; }


        [ForeignKey("ClientFeeScheduleId")]
        public virtual ClientFeeSchedule ClientFeeSchedule { get; set; }


		[ForeignKey("ProviderLevelId")]
		public virtual ProviderLevel ProviderLevel { get; set; }
	}
}
