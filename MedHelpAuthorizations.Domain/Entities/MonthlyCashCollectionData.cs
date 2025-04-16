using MedHelpAuthorizations.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class MonthlyCashCollectionData : MonthlyDataBase, IClientRelationship
    {
        public decimal Payment { get; set; }
        public decimal CollectionGoal { get; set; }
        public int ClientId { get; set; }
        public int? ClientLocationId { get; set; }
        public int? ClientProviderId { get; set; }
        public int? ClientInsuranceId { get; set; }
        public int CptCodeId { get; set; }

        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; }

        public MonthlyCashCollectionData(string date, decimal payment, decimal collectionGoal, decimal change, string status)
            : base(date, change, status)
        {
            Payment = payment;
            CollectionGoal = collectionGoal;
        }
    }
}