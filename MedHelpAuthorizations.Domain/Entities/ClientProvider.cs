using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    [CustomReportTypeEntityHeader(CustomReportHelper._ClientProvider, CustomTypeCode.Empty,false)]
    public class ClientProvider : AuditableEntity<int>, IProvider, IClientRelationship, IDataPipe<int>
	{
        public ClientProvider()
        {
            //ScheduleResources = new HashSet<ScheduleResource>();
            //PatientLedgerChargeAttendingProvider = new HashSet<PatientLedgerCharge>();
            //PatientLedgerChargeSupervisingProvider = new HashSet<PatientLedgerCharge>();
            Patients = new HashSet<Patient>();
            ClientProviderLocations = new HashSet<ClientProviderLocation>();

        }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public int PersonId { get; set; }

        [Required]
        public SpecialtyEnum SpecialtyId { get; set; }

		public ProviderLevelEnum? ProviderLevelId { get; set; }

		public string Credentials { get; set; }

        [StringLength(10)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClientProvider, CustomTypeCode.String, propertyName: CustomReportHelper.ProviderNpi)]
        public string Npi { get; set; }

        public int? SupervisingProviderId { get; set; }

        [StringLength(6)]
        public string Upin { get; set; }

        [StringLength(9)]
        public string TaxId { get; set; }

        public string TaxonomyCode { get; set; }
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClientProvider, CustomTypeCode.String, propertyName: CustomReportHelper.ProviderLicense)]
        public string License { get; set; }

        public string ExternalId { get; set; }

        public int ScheduledVisitsPerDayKpi { get; set; } = 0;

        public int PatientsSeenPerDayKpi { get; set; } = 0;

        public int DaysToBillKpi { get; set; } = 0;

        public decimal NoShowRateKpi { get; set; } = 0.0m;

		public string DfExternalId { get; set; }

		public DateTime? DfCreatedOn { get; set; }

		public DateTime? DfLastModifiedOn { get; set; }


		#region Navigation Objects

		[ForeignKey("ClientId")]
        public virtual Client Client { get; set; }


        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }


        [ForeignKey("SpecialtyId")]
        public virtual Specialty Specialty { get; set; }


		[ForeignKey("ProviderLevelId")]
		public virtual ProviderLevel ProviderLevel { get; set; }


		[ForeignKey("SupervisingProviderId")]
        public virtual ClientProvider SupervisingProvider { get; set; }

        public virtual ICollection<Patient> Patients { get; set; }

        //public virtual ICollection<ReferringProvider> ReferringProvider { get; set; }

        //public virtual ICollection<PatientLedgerCharge> PatientLedgerChargeAttendingProvider { get; set; }
        //public virtual ICollection<PatientLedgerCharge> PatientLedgerChargeSupervisingProvider { get; set; }

        //public virtual ICollection<ScheduleResource> ScheduleResources { get; set; }
        public virtual ICollection<ClientProviderLocation> ClientProviderLocations { get; set; }

		#endregion

	}
}

