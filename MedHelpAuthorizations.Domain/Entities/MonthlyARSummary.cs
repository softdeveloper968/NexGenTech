using MedHelpAuthorizations.Domain.Contracts;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class MonthlyARSummary : AuditableEntity<int>
    {
        public int ClientId { get; set; }
        public int Month { get; set; }  
        public int Year { get; set; } 
        public int DaysInAR { get; set; } 
    }
}
