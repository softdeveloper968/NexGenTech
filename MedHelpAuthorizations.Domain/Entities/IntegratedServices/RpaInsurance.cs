using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class RpaInsurance : AuditableEntity<int>, ISoftDelete, IName
    {
        public RpaInsurance(string name, string code, int rpaInsuranceGroupId, ApiIntegrationEnum? apiIntegrationId)
        {
            Code = code;
            Name = name;
            RpaInsuranceGroupId = rpaInsuranceGroupId;
			ApiIntegrationId = apiIntegrationId;
		}

        public RpaInsurance(string name, string code)
        {
            Code = code;
            Name = name;

            ClientInsuranceRpaConfigurations = new HashSet<ClientInsuranceRpaConfiguration>();
            ClientInsurances = new HashSet<ClientInsurance>();
            ClaimStatusBatches = new HashSet<ClaimStatusBatch>();
        }

        public RpaInsurance(int id, int rpaInsuranceGroupId, string name, string code)
        {
            Code = code;
            Name = name;
            Id = id;
            RpaInsuranceGroupId = rpaInsuranceGroupId;
            ClientInsuranceRpaConfigurations = new HashSet<ClientInsuranceRpaConfiguration>();
            ClientInsurances = new HashSet<ClientInsurance>();
            ClaimStatusBatches = new HashSet<ClaimStatusBatch>();
        }

        [StringLength(12)]
        public string Code { get; set; }


        [StringLength(25)]
        public string Name { get; set; }

        public int ClaimBilledOnWaitDays { get; set; } = 4;

        public int ApprovalWaitPeriodDays { get; set; } = 6;

        public DateTime? InactivatedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int? RpaInsuranceGroupId { get; set; } //AA-23

        public string TargetUrl { get; set; } //AA-23

        public ApiIntegrationEnum? ApiIntegrationId { get; set; }

        #region Navigation Access

        public virtual ICollection<ClientInsuranceRpaConfiguration> ClientInsuranceRpaConfigurations { get; set; }

        public virtual ICollection<ClaimStatusBatch> ClaimStatusBatches { get; set; }

        public virtual ICollection<ClientInsurance> ClientInsurances { get; set; }


        [ForeignKey("RpaInsuranceGroupId")]
        public virtual RpaInsuranceGroup RpaInsuranceGroup { get; set; } //AA-23


		[ForeignKey("ApiIntegrationId")]
		public virtual ApiIntegration ApiIntegration { get; set; } //AA-23

		#endregion
	}
}
