using MedHelpAuthorizations.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class ClientEndOfMonthTotal : AuditableEntity<int>
    {
        public int ClientId { get; set; }
        public int? ClientLocationId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal ARTotal { get; set; }
        public decimal ARTotalAbove90Days { get; set; }
        public decimal ARTotalAbove180Days { get; set; }
        public int ARTotalVisits { get; set; }
        public int ARTotalVisitsAbove90Days { get; set; }
        public int ARTotalVisitsAbove180Days { get; set; }
        public int MonthlyDaysInAR { get; set; }

        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; }

        [ForeignKey(nameof(ClientLocationId))]
        public virtual ClientLocation ClientLocation { get; set; }
    }
}
