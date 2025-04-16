namespace MedHelpAuthorizations.Application.Features.IntegratedServices.Charges
{
    public class GetCashValueForRevenueByDayResponse
    {
        /// <summary>
        /// Gets or sets the count of claims.
        /// </summary>
        public int ClaimCount { get; set; } = 0;
        /// <summary>
        /// Gets or sets the cash value.
        /// </summary>
        public decimal CashValue { get; set; } = 0.00m;
        /// <summary>
        /// Gets or sets the total revenue.
        /// </summary>
        public decimal RevenueTotals { get; set; } = 0.00m;
        /// <summary>
        /// Gets or sets the MD5 hash of the claim level.
        /// </summary>
        public string ClaimLevelMd5Hash { get; set; }
        /// <summary>
        /// Gets or sets the ID of the client's insurance.
        /// </summary>
        public int ClientInsuranceId { get; set; }
        /// <summary>
        /// Gets or sets the ID of the client's insurance.
        /// </summary>
        public string PayerName { get; set; }
        /// <summary>
        /// Gets or sets the service date.
        /// </summary>
        public string ServiceDate { get; set; }
        /// <summary>
        /// Gets or sets the billed date.
        /// </summary>
        public string BilledDate { get; set; }
    }
}
