using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClaimStatusTotalResult : AuditableEntity<int>
    {
        public int ClientId { get; set; }
        public int ClientInsuranceId { get; set; }
        public int? ClaimLineItemStatusId { get; set; }
        public ClaimStatusExceptionReasonCategoryEnum ClaimStatusExceptionReasonCategoryId { get; set; }
        public int ClientCptCodeId { get; set; }
        public int Quantity { get; set; }
        public decimal ChargedSum { get; set; }
        public decimal PaidAmountSum { get; set; }
        public decimal AllowedAmountSum { get; set; }
        public decimal NonAllowedAmountSum { get; set; }
        public int? ClientLocationId { get; set; }
        public int? ClientProviderId { get; set; }
        public decimal WriteOffAmountSum { get; set; }
        public DateTime BatchProcessDate { get; set; }
        public DateTime? DateOfServiceFrom { get; set;}
        public DateTime? DateOfServiceTo { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? ClaimReceivedDate { get; set; }
        public DateTime? BilledOnDate { get; set; } 
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("ClientInsuranceId")]
        public virtual ClientInsurance ClientInsurance { get; set; }

        [ForeignKey("ClientCptCodeId")]
        public virtual ClientCptCode ClientCptCode { get; set; }

        [ForeignKey("ClientLocationId")]
        public virtual ClientLocation ClientLocation { get; set; }

        [ForeignKey("ClientProviderId")]
        public virtual ClientProvider ClientProvider { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
    }
}
