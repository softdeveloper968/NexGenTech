using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Contracts
{
	public class DfStagingAuditableEntity : IDfStagingAuditableEntity
	{
		[Key]
		public int StgId { get; set; }

		public bool? IsProcessedSuccessfully { get; set; }

		public string? ErrorMessage { get; set; }

		public string? StgCreatedBy { get; set; }

		public DateTime? StgCreatedOn { get; set; }

		public string? StgLastModifiedBy { get; set; }

		public DateTime? StgLastModifiedOn { get; set; }

		public string? TenantClientString { get; set; }
	}
}
