using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
	public class ClientInsuranceRpaConfiguration : AuditableEntity<int>, ISoftDelete
    {
        public ClientInsuranceRpaConfiguration(int clientInsuranceId, TransactionTypeEnum transactionTypeId, int authTypeId, int clientLocationId)
        {
            ClientInsuranceId = clientInsuranceId;
            //RpaInsuranceId = rpaInsuranceId;
            TransactionTypeId = transactionTypeId;
            AuthTypeId = authTypeId;
            ClientLocationId = clientLocationId;

			#region Navigational Property Init

			//TransactionBatches = new HashSet<TransactionBatch>();

			#endregion
		}

        public ClientInsuranceRpaConfiguration()
        {
        }

       // public int ClientId { get; set; }
        public int ClientInsuranceId { get; set; }

        public int? ClientRpaCredentialConfigurationId { get; set; } //AA-23

		public int? AlternateClientRpaCredentialConfigurationId { get; set; } //AA-23

		public TransactionTypeEnum TransactionTypeId { get; set; }

        public int? AuthTypeId { get; set; }
        
        public bool IsDeleted { get; set; } = false;

        public string ExternalId { get; set; }

        public int DailyClaimLimit { get; set; } 

        public int CurrentDayClaimCount { get; set; }
		public int? ClientLocationId { get; set; }

		#region Navigational Property Access

		[ForeignKey("ClientInsuranceId")]
        public virtual ClientInsurance ClientInsurance { get; set; }


        [ForeignKey("TransactionTypeId")]
        public virtual TransactionType TransactionType { get; set; }


        [ForeignKey("AuthTypeId")]
        public virtual AuthType AuthType { get; set; }


        [ForeignKey("ClientRpaCredentialConfigurationId")]
        public virtual ClientRpaCredentialConfiguration ClientRpaCredentialConfiguration { get; set; }


		[ForeignKey("AlternateClientRpaCredentialConfigurationId")]
		public virtual ClientRpaCredentialConfiguration AlternateClientRpaCredentialConfiguration { get; set; }

        [ForeignKey("ClientLocationId")]
        public virtual ClientLocation ClientLocation { get; set; }
        #endregion
    }
}
