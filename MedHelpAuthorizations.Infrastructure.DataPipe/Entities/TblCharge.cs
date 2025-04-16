using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.CustomAttributes;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Models;

[ImportOrderAttribute(12)]
public partial class TblCharge : DfStagingAuditableEntity
{
    public string? ChargeId { get; set; }

    public string? ClaimNumber { get; set; }

    public string? PatientId { get; set; }

    public string? ResponsiblePartyId { get; set; }

    public string? ProcedureCode { get; set; }

    public string? PlaceOfServiceCode { get; set; }

    public string? Quantity { get; set; }

    public decimal? ChargeAmount { get; set; }

    public string? Description { get; set; }

    public string? PatientInsuranceCard1Id { get; set; }

    public string? Insurance1 { get; set; }

    public string? PatientInsuranceCard2Id { get; set; }

    public string? Insurance2 { get; set; }

    public string? PatientInsuranceCard3Id { get; set; }

    public string? Insurance3 { get; set; }

    public string? PatientFirstBillDate { get; set; }

    public string? PatientLastBillDate { get; set; }

    public string? BilledToInsuranceId { get; set; }

    public string? InsuranceFirstBilledOn { get; set; }

    public string? InsuranceLastBilledOn { get; set; }

    public string? RenderingProviderId { get; set; }

    public string? PlaceOfServiceId { get; set; }

    public string? Modifier1 { get; set; }

    public string? Modifier2 { get; set; }

    public string? Modifier3 { get; set; }

    public string? Modifier4 { get; set; }

    public string? IcdCode1 { get; set; }

    public string? IcdCode2 { get; set; }

    public string? IcdCode3 { get; set; }

    public string? IcdCode4 { get; set; }

    public string? DateOfServiceFrom { get; set; }

    public string? DateOfServiceTo { get; set; }

    public int LocationId { get; set; }

    public string? EntryDate { get; set; }

    public string? ModifiedDate { get; set; }

    public string? TenantClientString { get; set; }
}
