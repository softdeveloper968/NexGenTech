namespace MedHelpAuthorizations.Application.Features.IntegratedServices.Charges
{
    public class CashValueForRevenueDetails
    {
        /// <summary>
        /// Gets or sets the cash value.
        /// </summary>
        public decimal CashValue { get; set; } = 0.00m;
        
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

        /// <summary>
        /// Last name of the patient
        /// </summary>
        public string PatientLastName { get; set; }

        /// <summary>
        /// First name of the person
        /// </summary>
        public string PatientFirstName { get; set; }

        /// <summary>
        /// Location associated
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Provider associated
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Estimated payment value
        /// </summary>
        public decimal EstimatedPayment { get; set; }

        /// <summary>
        /// Procedure code
        /// </summary>
        public string ProcedureCode { get; set; }
    }
}
