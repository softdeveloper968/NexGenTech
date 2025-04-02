using MedHelpAuthorizations.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class MonthlyDenialData : MonthlyDataBase, IClientRelationship
    {
        public int ClientId { get; set; }
        public decimal Charges { get; set; }
        public decimal Denials { get; set; }
        public decimal PercentageOfCharges { get; set; }
        public decimal DenialPercentageGoal { get; set; }
        public int? ClientLocationId { get; set; }
        public int? ClientProviderId { get; set; }
        public int? ClientInsuranceId { get; set; }
        public int CptCodeId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get ; set; }

        public MonthlyDenialData(string date, decimal charges, decimal denials, decimal percentageOfCharges,
                                 decimal denialPercentageGoal, decimal change, string status)
            : base(date, change, status)
        {
            Charges = charges;
            Denials = denials;
            PercentageOfCharges = percentageOfCharges;
            DenialPercentageGoal = denialPercentageGoal;
        }
    }
}