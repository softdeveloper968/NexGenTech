using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientProviderLocation : AuditableEntity<int>, IDataPipe<int>
	{
        public ClientProviderLocation()
        {
        }
        public int ClientLocationId { get; set; }
        public int ClientProviderId { get; set; }
		public string DfExternalId { get; set; }
		public DateTime? DfCreatedOn { get; set; }
		public DateTime? DfLastModifiedOn { get; set; }


		#region Navigation Objects

		[ForeignKey("ClientLocationId")]
        public virtual ClientLocation ClientLocation { get; set; }
        
        [ForeignKey("ClientProviderId")]
        public virtual ClientProvider ClientProvider { get; set; }

        #endregion
    }

}
