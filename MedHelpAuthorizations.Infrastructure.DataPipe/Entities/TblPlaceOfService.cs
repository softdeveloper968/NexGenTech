using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.CustomAttributes;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Entities;


[ImportOrderAttribute(4)]
public partial class TblPlaceOfService : DfStagingAuditableEntity
{
	public int? PlaceOfServiceId { get; set; }

	public string? Name { get; set; }

	public string? Address { get; set; }

	public string? City { get; set; }

	public string? State { get; set; }

	public string? PostalCode { get; set; }

	public int? PlaceOfServiceCode { get; set; }

	public string? OfficePhone { get; set; }

	public string? FaxPhone { get; set; }

	public long? Npi { get; set; }

	public int? ExternalId { get; set; }

	public int? LocationId { get; set; }

	public string? TenantClientString { get; set; }
}
