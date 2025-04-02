using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.Charges
{
    public class GetCashProjectionByDayResponse
    {
        /// <summary>
        /// Gets or sets the count of claims.
        /// </summary>
        public int ClaimCount { get; set; } = 0;

        /// <summary>
        /// Gets or sets the check number associated with the data.
        /// </summary>
        public string CheckNumber { get; set; }

        /// <summary>
        /// Gets or sets the date of the check.
        /// </summary>
        public string CheckDate { get; set; }

        /// <summary>
        /// Gets or sets the total paid amounts.
        /// </summary>
        public decimal PaidTotals { get; set; } = 0.00m;

        /// <summary>
        /// Gets or sets the total revenue amounts.
        /// </summary>
        public decimal RevenueTotals { get; set; } = 0.00m;

        /// <summary>
        /// Gets or sets the MD5 hash associated with the claim.
        /// </summary>
        public string ClaimLevelMd5Hash { get; set; }

        /// <summary>
        /// Gets or sets the ID of the client's insurance.
        /// </summary>
        public int ClientInsuranceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the payer.
        /// </summary>
        public string PayerName { get; set; }

        /// <summary>
        /// Gets or sets the account number.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets the external identifier.
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the concatenated name of the patient in "Last, First" format.
        /// </summary>
        public string PatientLastCommaFirst { get; set; }
    }
}
