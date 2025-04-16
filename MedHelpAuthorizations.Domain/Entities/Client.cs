using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class Client : AuditableEntity<int>
    {
        public Client()
        {
            ClientAuthTypes = new HashSet<ClientAuthType>();
            ClientApplicationFeatures = new HashSet<ClientApplicationFeature>();
            ClientApiIntegrationKeys = new HashSet<ClientApiIntegrationKey>();
            InputDocuments = new HashSet<InputDocument>();
            Providers = new HashSet<ClientProvider>(); 
            EmployeeClients = new HashSet<EmployeeClient>();
            ClientSpecialties = new HashSet<ClientSpecialty>();
        }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string ClientCode { get; set; }

        public int? TaxId { get; set; }

        public int? NpiNumber { get; set; }

        public int? AddressId { get; set; }

        public long? PhoneNumber { get; set; }

        public long? FaxNumber { get; set; }

        public int? ClientQuestionnaireId { get; set; }

        public int? ClientKpiId { get; set; }

        public bool IsActive { get; set; } = true;

        //Claim Batches created before this date should be handled by a separate bot controller with separate rpaConfiguration credentials.
        public DateTime? InitialAnalysisEndOn { get; set; }

        public int? AutoLogMinutes { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }


        [ForeignKey("ClientQuestionnaireId")]
        public ClientQuestionnaire ClientQuestionnaire { get; set; }


        [ForeignKey("ClientKpiId")]
        public virtual ClientKpi ClientKpi { get; set; }

		public SourceSystemEnum? SourceSystemId { get; set; }

		public virtual ICollection<ClientProvider> Providers { get; set; }

        public virtual ICollection<ClientAuthType> ClientAuthTypes { get; set; }

        public virtual ICollection<ClientApplicationFeature> ClientApplicationFeatures { get; set; }

        public virtual ICollection<ClientApiIntegrationKey> ClientApiIntegrationKeys { get; set; }

        public virtual ICollection<InputDocument> InputDocuments { get; set; }

        public virtual ICollection<DocumentType> DocumentTypes { get; set; }

        public virtual ICollection<EmployeeClient> EmployeeClients { get; set; }

        public virtual ICollection<ClientLocation> ClientLocations { get; set; }

        public virtual ICollection<ClientInsurance> ClientInsurances { get; set; }

        public virtual ICollection<Person> Person { get; set; }

        public virtual ICollection<ClientFeeSchedule> ClientFeeSchedules { get; set; }

        public virtual ICollection<ClientSpecialty> ClientSpecialties { get; set;}

        public virtual ICollection<ClientHoliday> ClientHolidays { get; set; }

        public virtual ICollection<ClientDayOfOperation> ClientDaysOfOperation { get; set; }
    }
}
