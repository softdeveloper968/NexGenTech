using MedHelpAuthorizations.Domain.Common.Contracts;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Contracts
{
	public interface IDfStagingAuditableEntity : IStgEntity
	{
		public bool? IsProcessedSuccessfully { get; set; }

		public string ErrorMessage { get; set; }

		string StgCreatedBy { get; set; }

		DateTime? StgCreatedOn { get; set; }

		string StgLastModifiedBy { get; set; }

		DateTime? StgLastModifiedOn { get; set; }

		string? TenantClientString { get; set; }
	}
}
