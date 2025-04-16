using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class Cardholder : AuditableEntity<int>, IDataPipe<int>, IClientRelationship
	{
        public Cardholder()
        {
            InsuranceCards = new HashSet<InsuranceCard>();
        }

		public DateTime? SignatureOnFile { get; set; }
        
        public int PersonId { get; set; }

        //public string TenantId { get; set; }

        public int ClientId { get; set; }

        public string DfExternalId { get; set; }

		public DateTime? DfCreatedOn { get; set; }

		public DateTime? DfLastModifiedOn { get; set; }


		[ForeignKey("ClientId")]
        public virtual Client Client { get; set; }


        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        public virtual ICollection<InsuranceCard> InsuranceCards { get; set; }

	}
}
