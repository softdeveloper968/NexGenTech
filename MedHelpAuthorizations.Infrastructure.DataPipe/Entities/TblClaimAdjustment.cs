using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.CustomAttributes;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Models;

[ImportOrderAttribute(14)]
public partial class TblClaimAdjustment : DfStagingAuditableEntity
{
    public int? ClaimAdjustmentsId { get; set; }

    public string? ClaimChargeId { get; set; }

    public int? ClaimNumber { get; set; }

    public string? AdjustmentCodeId { get; set; }

    public string? RemittenceId { get; set; }

    public string? Description { get; set; }

    public decimal? Amount { get; set; }

    public string? EntryDate { get; set; }

    public string? ModifiedDate { get; set; }

    public string? TenantClientString { get; set; }
}
