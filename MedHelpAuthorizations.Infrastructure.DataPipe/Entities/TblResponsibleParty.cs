using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.CustomAttributes;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Entities;

[ImportOrderAttribute(7)]
public partial class TblResponsibleParty : DfStagingAuditableEntity
{
	public int? ResponsiblePartiesId { get; set; }

	public string? LastName { get; set; }

	public string? FirstName { get; set; }

	public string? AddressStreetLine1 { get; set; }

	public string? AddressStreetLine2 { get; set; }

	public string? City { get; set; }

	public string? State { get; set; }

	public string? PostalCode { get; set; }

	public string? Email { get; set; }

	public string? MobilePhone { get; set; }

	public string? TenantClientString { get; set; }
}
