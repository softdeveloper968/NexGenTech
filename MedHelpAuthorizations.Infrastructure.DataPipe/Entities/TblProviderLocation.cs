using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.CustomAttributes;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Entities;


[ImportOrderAttribute(6)]
public partial class TblProviderLocation : DfStagingAuditableEntity
{
	public int? ProviderLocationId { get; set; }

	public int? ProviderId { get; set; }

	public int? LocationId { get; set; }

	public string? TenantClientString { get; set; }
}
