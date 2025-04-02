using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.CustomAttributes;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Entities;


[ImportOrderAttribute(5)]
public partial class TblProvider : DfStagingAuditableEntity
{
	public int? ProviderId { get; set; }

	public string? FullName { get; set; }

	public string? Address { get; set; }

	public string? City { get; set; }

	public string? State { get; set; }

	public string? PostalCode { get; set; }

	public string? OfficePhone { get; set; }

	public string? LicenseNumber { get; set; }

	public string? SocialSecurityNumber { get; set; }

	public string? TaxId { get; set; }

	public string? SpecialtyName { get; set; }

	public string? FirstName { get; set; }

	public string? LastName { get; set; }

	public string? IsPhysiciansAssistant { get; set; }

	public string? Npi { get; set; }

	public string? FaxNumber { get; set; }

	public string? ExternalId { get; set; }

	public string? IsActive { get; set; }

	public string? TenantClientString { get; set; }
}
