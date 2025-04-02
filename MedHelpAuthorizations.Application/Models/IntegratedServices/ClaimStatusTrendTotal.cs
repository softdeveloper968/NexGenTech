using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System;

namespace MedHelpAuthorizations.Shared.Models.IntegratedServices
{
    public class ClaimStatusTrendTotal : IIntegratedServicesDashboardDataQueryResult, IEquatable<ClaimStatusTrendTotal>, ICloneable
    {
        public DateTime? DateOfServiceFrom { get; set; }
        public DateTime? ClaimBilledOn { get; set; }
        public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; }
        public ClaimStatusExceptionReasonCategoryEnum? ExceptionReasonCategoryId { get; set; }
        public int Quantity { get; set; }
        public decimal ChargedSum { get; set; }
        public decimal PaidAmountSum { get; set; }
        public string ProcedureCode { get; set; }
        public string ServiceType { get; set; }
        public int? ServiceTypeId { get; set; }
        public string ClientInsuranceLookupName { get; set; }
        public int? ClientInsuranceId { get; set; }
        public DateTime? FirstDateOfWeekBilledOn { get; set; }
        public DateTime? FirstDateOfWeekServiceDate { get; set; }
        public int? WeekNumberBilledOn { get; set; }
        public int? WeekYearBilledOn { get; set; }
        public int? WeekNumberServiceDate { get; set; }
        public int? WeekYearServiceDate { get; set; }
        public string ProviderName { get; set; }
        public string LocationName { get; set; }
        public int ClientProviderId { get; set; }
        public int ClientLocationId { get; set; }
        public decimal AllowedAmountSum { get; set; } //AA-130
        public decimal WriteOffAmountSum { get; set; } //AA-130
        public decimal ContractualAmountSum { get; set; } //AA-130
        public decimal ARBalanceAmountSum { get; set; } //AA-130
        public object Clone()
        {
            return (ClaimStatusTrendTotal)this.MemberwiseClone();
        }

        public bool Equals(ClaimStatusTrendTotal other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return DateOfServiceFrom == other.DateOfServiceFrom
                   && ClaimBilledOn == other.ClaimBilledOn
                   && ClaimLineItemStatusId == other.ClaimLineItemStatusId
                   && ExceptionReasonCategoryId == other.ExceptionReasonCategoryId
                   && Quantity == other.Quantity
                   && ChargedSum == other.ChargedSum
                   && PaidAmountSum == other.PaidAmountSum
                   && ProcedureCode == other.ProcedureCode
                   && ServiceType == other.ServiceType
                   && ClientInsuranceLookupName == other.ClientInsuranceLookupName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ClaimStatusTotal)obj);
        }

        public override int GetHashCode()
        {
            //return HashCode.Combine(ClaimLineItemStatus, ClaimStatusExceptionReasonCategory, Quantity, ChargedSum, PaidAmountSum);
            return HashCode.Combine(ClientInsuranceLookupName, ClaimLineItemStatusId, ProcedureCode, Quantity, ChargedSum, PaidAmountSum, DateOfServiceFrom, ServiceType);
        }
    }
}
