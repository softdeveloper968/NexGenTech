using MedHelpAuthorizations.Domain.Contracts;

namespace MedHelpAuthorizations.Domain.Entities
{
    /// <summary>
    /// Represents the average collection percentage for a specific client insurance in a given quarter and year.
    /// </summary>
    public class ClientInsuranceAverageCollectionPercentage : AuditableEntity<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientInsuranceAverageCollectionPercentage"/> class.
        /// </summary>
        public ClientInsuranceAverageCollectionPercentage() { }

        /// <summary>
        /// Gets or sets the unique identifier of the associated client insurance.
        /// </summary>
        public int ClientInsuranceId { get; set; }

        /// <summary>
        /// Gets or sets the quarter for which the average collection percentage is calculated.
        /// </summary>
        public int Quarter { get; set; }

        /// <summary>
        /// Gets or sets the year for which the average collection percentage is calculated.
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Gets or sets the calculated collection percentage for the specified quarter, year, and client insurance.
        /// </summary>
        public decimal CollectionPercentage { get; set; }
    }

}
