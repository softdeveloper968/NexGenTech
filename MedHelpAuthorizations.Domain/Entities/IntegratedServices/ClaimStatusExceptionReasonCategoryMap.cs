namespace MedHelpAuthorizations.Domain.IntegratedServices
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using MedHelpAuthorizations.Domain.Contracts;
    using MedHelpAuthorizations.Domain.Entities.Enums;
    using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

    public class ClaimStatusExceptionReasonCategoryMap : AuditableEntity<int>
    {
        public ClaimStatusExceptionReasonCategoryMap()
        {

        }

        public ClaimStatusExceptionReasonCategoryMap(ClaimStatusExceptionReasonCategoryEnum claimStatusExceptionReasonCategoryId, string claimStatusExceptionReasonText)
        {
            this.ClaimStatusExceptionReasonCategoryId = claimStatusExceptionReasonCategoryId;
            this.ClaimStatusExceptionReasonText = claimStatusExceptionReasonText;
        }

        [Required]
        public ClaimStatusExceptionReasonCategoryEnum ClaimStatusExceptionReasonCategoryId { get; set; }

        [Required]
        public string ClaimStatusExceptionReasonText { get; set; }

        //public int RpaInsuranceId { get; set; }

        #region Navigation Objects

        //[ForeignKey("RpaInsuranceId")]
        //public virtual RpaInsurance RpaInsurance { get; set; }


        [ForeignKey("ClaimStatusExceptionReasonCategoryId")]
        public virtual ClaimStatusExceptionReasonCategory ClaimStatusExceptionReasonCategory { get; set; }

        #endregion
    }
}
