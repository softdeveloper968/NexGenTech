namespace MedHelpAuthorizations.Application.Features.IntegratedServices.Charges
{
    public class ClaimLevelAverageCollectionPercentage
    {
        /// <summary>
        /// A string representing the MD5 hash associated with the claim level.
        /// </summary>
        public string ClaimLevelMd5Hash { get; set; }
        /// <summary>
        /// A nullable decimal representing the total amount paid for line items in the claim.
        /// </summary>
        public decimal? LineItemPaidAmount { get; set; }
        /// <summary>
        /// A nullable decimal representing the total charge amount for line items in the claim.
        /// </summary>
        public decimal? LineItemChargeAmount { get; set; }
        /// <summary>
        ///  An integer representing the unique identifier of the client insurance associated with the claim.
        /// </summary>
        public int ClientInsuranceId { get; set; }
    }
}
