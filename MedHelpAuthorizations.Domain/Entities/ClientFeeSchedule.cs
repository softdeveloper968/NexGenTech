using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientFeeSchedule : AuditableEntity<int>, IClientRelationship
    {

        public ClientFeeSchedule() 
        { 
            ClientFeeScheduleEntries = new HashSet<ClientFeeScheduleEntry>();
            ClientInsuranceFeeSchedules = new HashSet<ClientInsuranceFeeSchedule>();
        }

        public int ClientId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }


		#region Navigation Properties

		[ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

		public virtual ICollection<ClientInsuranceFeeSchedule> ClientInsuranceFeeSchedules { get; set; }

		public virtual ICollection<ClientFeeScheduleEntry> ClientFeeScheduleEntries { get; set; }

        public virtual ICollection<ClientFeeScheduleProviderLevel> ClientFeeScheduleProviderLevels { get; set; }

        public virtual ICollection<ClientFeeScheduleSpecialty> ClientFeeScheduleSpecialties { get; set; }

        public ImportStatusEnum ImportStatus { get; set; }

		#endregion
	}
}
