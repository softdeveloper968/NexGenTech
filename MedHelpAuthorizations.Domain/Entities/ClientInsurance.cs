using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    [CustomReportTypeEntityHeader(entityName:CustomReportHelper._ClientInsurance, CustomTypeCode.Empty,false)]
    public class ClientInsurance : AuditableEntity<int>, IClientRelationship, IDataPipe<int>
	{
		public ClientInsurance()
		{
			ClientInsuranceFeeSchedules = new HashSet<ClientInsuranceFeeSchedule>();
			//InvoiceHistory = new HashSet<InvoiceHistory>();
			//Remittance = new HashSet<Remittance>();
		}

		[StringLength(125)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClientInsurance, CustomTypeCode.String, propertyName: CustomReportHelper.ClientInsuranceLookupName)]
        public string LookupName { get; set; }

		[StringLength(125)]
		public string Name { get; set; }

		//public int? InsuranceCategoryId { get; set; }
		public long? PhoneNumber { get; set; }

		public long? FaxNumber { get; set; }

		//[StringLength(14)]
		//public string OfficePhoneNumber { get; set; };

		[StringLength(30)]
		public string ExternalId { get; set; }

		[StringLength(30)]
		public string PayerIdentifier { get; set; }

		public int? RpaInsuranceId { get; set; }

		public int ClientId { get; set; }

		public bool RequireLocationInput { get; set; } = false;

		public string DfExternalId { get; set; }

		public DateTime? DfCreatedOn { get; set; }

		public DateTime? DfLastModifiedOn { get; set; }

		public bool AutoCalcPenalty { get; set; } = false ;

        #region Navigation Objects

        [ForeignKey("ClientId")]
		public virtual Client Client { get; set; }

		[ForeignKey("RpaInsuranceId")]
		public virtual RpaInsurance RpaInsurance { get; set; }

		// [ForeignKey("ClientFeeScheduleId")]
		//public virtual ClientFeeSchedule ClientFeeSchedule { get; set; }

		//[ForeignKey("AddressId")]
		//public virtual Address Address { get; set; }

		//[ForeignKey("InvoiceConfigurationTypeId")]
		//public virtual InvoiceConfigurationType InvoiceConfigurationType { get; set; }

		//[ForeignKey("InsuranceCategoryId")]
		//public virtual InsuranceCategory InsuranceCategory { get; set; }
		//public virtual ICollection<InvoiceHistory> InvoiceHistory { get; set; }
		//public virtual ICollection<Remittance> Remittance { get; set; }
		public virtual ICollection<EmployeeClientInsurance> EmployeeClientInsurances { get; set; }
		public virtual ICollection<ClientInsuranceFeeSchedule> ClientInsuranceFeeSchedules { get; set; }
		public virtual ICollection<ClientInsuranceAverageCollectionPercentage> ClientInsuranceAverageCollectionPercentages { get; set; } //EN-91

		#endregion
	}
}
