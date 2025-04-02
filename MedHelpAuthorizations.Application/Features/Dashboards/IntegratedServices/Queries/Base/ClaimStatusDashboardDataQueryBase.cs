using MedHelpAuthorizations.Application.Features.Patients.Queries.GetPatientsByCriteria;
using MedHelpAuthorizations.Client.Shared.Models.DashboardPresets;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Shared.Models.IntegratedServices.DashboardPresets;
using System.Text.Json.Serialization;
using static MedHelpAuthorizations.Client.Shared.Models.DashboardPresets.DashboardPresets;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;

public class ClaimStatusDashboardQueryBase : IClaimStatusDashboardQueryBase, ICloneable, IEquatable<ClaimStatusDashboardQueryBase>
{
    public ClaimStatusEnum? ClaimLineItemStatusId = null;
    public int ClaimStatusBatchId { get; set; } = 0;
    public int ClientLocationId { get; set; } = 0;
    public int ClientProviderId { get; set; } = 0;
    public string ProviderName { get; set; }
    public string LocationName { get; set; }
    public DateTime? ReceivedFrom { get; set; } = null;
    public DateTime? ReceivedTo { get; set; } = null;
    public DateTime? DateOfServiceFrom { get; set; } = null;
    public DateTime? DateOfServiceTo { get; set; } = null;
    public DateTime? TransactionDateFrom { get; set; } = null;
    public DateTime? TransactionDateTo { get; set; } = null;
    public DateTime? ClaimBilledFrom { get; set; } = null;
    public DateTime? ClaimBilledTo { get; set; } = null;
    public string CommaDelimitedLineItemStatusIds { get; set; } = string.Empty;
    public DateGroupingTypeEnum? DateGroupingType { get; set; } = null;

    /// multi-select client insurance Ids
    public string ClientInsuranceIds { get; set; } = string.Empty;// AA-120
    public string ExceptionReasonCategoryIds { get; set; } = string.Empty;// AA-120
    public string AuthTypeIds { get; set; } = string.Empty;// AA-120
    public string ProcedureCodes { get; set; } = string.Empty;// AA-120
    public string ClientLocationIds { get; set; } = string.Empty;// AA-120
    public string ClientProviderIds { get; set; } = string.Empty;// AA-120
    public int? PatientId { get; set; }
    public GetPatientsByCriteriaResponse Patient { get; set; } = new(); //EN-219
    public PresetFilterTypeEnum PresetFilterType { get; set; } = PresetFilterTypeEnum.BilledOnDate;
    public CustomPresetDateRangesEnum CustomPresetDateRanges { get; set; } = CustomPresetDateRangesEnum.ThisWeek;
    public DashboardPresets.PresetDateRangesEnum PresetDateRangeSelection { get; set; } = PresetDateRangesEnum.ThisWeek;

    public string FlattenedLineItemStatus { get; set; }
    public string DashboardType { get; set; } = string.Empty;

    public bool HasGroupByKeySelector { get; set; } = false;
    public ClaimStatusTypeEnum? ClaimStatusType { get; set; } = null;
    public string ClaimStatusTypeValue { get; set; } = string.Empty;
    public bool? HasProcedureDashboard { get; set; } = false;
    [JsonIgnore]
    public bool UseBasicFilter { get; set; } = true;
    public string FileName { get; set; } = string.Empty;

    public object Clone()
    {
        return (ClaimStatusDashboardQueryBase)this.MemberwiseClone();
    }

    public bool Equals(ClaimStatusDashboardQueryBase other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return AuthTypeIds == other.AuthTypeIds
               && ExceptionReasonCategoryIds == other.ExceptionReasonCategoryIds
               && ProcedureCodes == other.ProcedureCodes
               && ReceivedFrom == other.ReceivedFrom
               && ReceivedTo == other.ReceivedTo
               && DateOfServiceFrom == other.DateOfServiceFrom
               && DateOfServiceTo == other.DateOfServiceTo
               && TransactionDateFrom == other.TransactionDateFrom
               && TransactionDateTo == other.TransactionDateTo
               && ClaimBilledFrom == other.ClaimBilledFrom
               && ClaimBilledTo == other.ClaimBilledTo
               && ClientInsuranceIds == other.ClientInsuranceIds
               && PatientId == other.PatientId
               && ClientProviderIds == other.ClientProviderIds;
    }

    //public override bool Equals(object obj)
    //{
    //    if (ReferenceEquals(null, obj)) return false;
    //    if (ReferenceEquals(this, obj)) return true;
    //    if (obj.GetType() != this.GetType()) return false;
    //    return Equals((ClaimStatusTotal)obj);
    //}

    public override int GetHashCode()
    {
        return HashCode.Combine(AuthTypeIds, ExceptionReasonCategoryIds, ProcedureCodes, DateOfServiceFrom, ClaimBilledFrom, ClaimBilledTo, ReceivedFrom, TransactionDateFrom);
    }
}