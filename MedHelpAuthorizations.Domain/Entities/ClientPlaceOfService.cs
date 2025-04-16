using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientPlaceOfService : AuditableEntity<int>, IDataPipe<int>, IClientRelationship
	{
        public ClientPlaceOfService()
        {
            // PatientLedgerCharge = new HashSet<PatientLedgerCharge>();
        }

        public string Name { get; set; }

        public string LookupName { get; set; }

        public PlaceOfServiceCodeEnum PlaceOfServiceCodeId { get; set; }

        public string Npi { get; set; }

        public int? AddressId { get; set; }

        public int ClientId { get; set; }

		public string DfExternalId { get; set; }

		public DateTime? DfCreatedOn { get; set; }

		public DateTime? DfLastModifiedOn { get; set; }


		#region Navigation Objects


		[ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        //public virtual ICollection<PatientLedgerCharge> PatientLedgerCharge { get; set; }

        [ForeignKey("PlaceOfServiceCodeId")]
        public virtual PlaceOfServiceCode PlaceOfServiceCode { get; set; }


        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }

        #endregion

    }
}
