using MedHelpAuthorizations.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class MonthlyReceivablesData : MonthlyDataBase, IClientRelationship
    {
        public decimal Receivables { get; set; }
        public int DaysInAR { get; set; }
        public decimal PercentageOfCharges { get; set; }
        public decimal DenialPercentageGoal { get; set; }
        public int ClientId { get; set; }
        public int? ClientLocationId { get; set; }
        public int? ClientProviderId { get; set; }
        public int? ClientInsuranceId { get; set; }
        public int CptCodeId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; }

        public MonthlyReceivablesData(string date, decimal receivables, int daysInAR, decimal percentageOfCharges,
                                       decimal denialPercentageGoal, decimal change, string status)
            : base(date, change, status)
        {
            Receivables = receivables;
            DaysInAR = daysInAR;
            PercentageOfCharges = percentageOfCharges;
            DenialPercentageGoal = denialPercentageGoal;
        }
    }
}