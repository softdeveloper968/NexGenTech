using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.CustomAttributes;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Entities;

[ImportOrderAttribute(10)]
public partial class TblPatientInsuranceCard : DfStagingAuditableEntity
{
	public int? PatientId { get; set; }

	public int? InsuranceId { get; set; }

	public int? CardHolderId { get; set; }

	public string? InsuranceCardOrder { get; set; }

	public string? GroupId { get; set; }

	public string? MemberNumber { get; set; }

	public string? EffectiveStartDate { get; set; }

	public string? EffectiveEndDate { get; set; }

	public decimal? CoPay { get; set; }

	public decimal? CoInsurance { get; set; }

	public string? ActiveDate { get; set; }

	public string? InactiveDate { get; set; }

	public int? InactivePosition { get; set; }

	public string? CardHolderRelationship { get; set; }

	public string? PlanType { get; set; }

	public string? TenantClientString { get; set; }

	public int? Id { get; set; }
}
