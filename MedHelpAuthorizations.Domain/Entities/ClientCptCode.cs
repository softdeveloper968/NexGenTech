using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientCptCode : AuditableEntity<int>, IClientRelationship
    {
        public ClientCptCode()
        {
            //PatientLedgerCharge = new HashSet<PatientLedgerCharge>();
        }

        public decimal? ScheduledFee { get; set; } = 0.00m;
        public int? CptCodeGroupId { get; set; }
        public int ClientId { get; set; }
        public string LookupName { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Code { get; set; }
        public string CodeVersion { get; set; }
        public TypeOfServiceEnum? TypeOfServiceId { get; set; }
        //public string TenantId { get; set; }
        #region Navigation Objects

        [ForeignKey("TypeOfServiceId")]
        public virtual TypeOfService TypeOfService { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }


		//[ForeignKey("CptCodeId")]
		//public virtual CptCode CptCode { get; set; }

		#endregion

	}
}
