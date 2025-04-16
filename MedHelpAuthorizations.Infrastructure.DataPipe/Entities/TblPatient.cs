using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.CustomAttributes;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Entities;


[ImportOrderAttribute(8)]
public partial class TblPatient : DfStagingAuditableEntity
{
	public int? PatientId { get; set; }

	public int? ResponsiblePartyId { get; set; }

	public string? LastName { get; set; }

	public string? FirstName { get; set; }

	public string? MiddleName { get; set; }

	public string? Gender { get; set; }

	public string? AddressStreetLine1 { get; set; }

	public string? City { get; set; }

	public string? State { get; set; }

	public string? PostalCode { get; set; }

	public string? HomePhoneNumber { get; set; }

	public string? SocialSecurityNumber { get; set; }

	public string? DateOfBirth { get; set; }

	public int? PrimaryInsuranceId { get; set; }

	public int? RenderingProviderId { get; set; }

	public int? SecondaryInsuranceId { get; set; }

	public int? TertiaryInsuranceId { get; set; }

	public string? CreatedOn { get; set; }

	public string? LastModifiedOn { get; set; }

	public string? ExternalId { get; set; }

	public string? TenantClientString { get; set; }
}
