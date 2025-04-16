using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
	public class ImportDocumentMessage : AuditableEntity<int>
	{
		public int ClaimStatusBatchId { get; set; }
		public int InputDocumentId { get; set; }
		public InputDocumentMessageTypeEnum MessageType { get; set; }
		public string Message { get; set; }
        public int? ClaimStatusBatchClaimId { get; set; }

        #region Navigation Objects
        [ForeignKey("InputDocumentId")]
		public virtual InputDocument InputDocument { get; set; }

        [ForeignKey("ClaimStatusBatchClaimId")]
        public virtual ClaimStatusBatchClaim ClaimStatusBatchClaim { get; set; }
        #endregion
    }
}
