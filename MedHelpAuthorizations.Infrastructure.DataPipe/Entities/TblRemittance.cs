using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.CustomAttributes;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Entities;

[ImportOrderAttribute(11)]
public partial class TblRemittance : DfStagingAuditableEntity
{
	public string? RemittanceId { get; set; }

	public int? InsuranceId { get; set; }

	public decimal? UndistributedAmount { get; set; }

	public decimal? PaymentAmount { get; set; }

	public string? CheckNumber { get; set; }

	public string? PatientId { get; set; }

	public string? RemittanceFormType { get; set; }

	public string? RemittanceSource { get; set; }

	public int? LocationId { get; set; }

	public string? CheckDate { get; set; }

	public string? TenantClientString { get; set; }
}
