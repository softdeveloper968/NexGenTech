using MedHelpAuthorizations.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class MonthlyARData : MonthlyDataBase, IClientRelationship
    {
        public decimal Receivables { get; set; }
        public decimal PercentageOfAR { get; set; }
        public decimal DenialGoal { get; set; }
        public int ClientId { get; set; }
        public decimal DenialPercentageGoal { get; set; }
        public int? ClientLocationId { get; set; }
        public int? ClientProviderId { get; set; }
        public int? ClientInsuranceId { get; set; }
        public int CptCodeId { get; set; }
        public decimal TotalReceivables { get; set; }
        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; }

        public MonthlyARData(string date, decimal receivables, decimal percentageOfAR, decimal denialGoal,
                             decimal denialPercentageGoal, decimal change, string status)
            : base(date, change, status) // Using MonthYear as Date here
        {
            Receivables = receivables;
            PercentageOfAR = percentageOfAR;
            DenialGoal = denialGoal;
            DenialPercentageGoal = denialPercentageGoal;
        }
        //    public MonthlyARData(
        //string date,
        //decimal receivables,
        //decimal percentageOfAR,
        //decimal denialGoal,
        //decimal denialPercentageGoal,
        //decimal change,
        //string status,
        //int clientId,
        //int clientLocationId,
        //int clientProviderId,
        //int clientInsuranceId,
        //int cptCodeId,
        //int month,
        //int year)
        //: base(date, change, status)
        //    {
        //        Receivables = receivables;
        //        PercentageOfAR = percentageOfAR;
        //        DenialGoal = denialGoal;
        //        DenialPercentageGoal = denialPercentageGoal;
        //        ClientId = clientId;
        //    }

    }
}