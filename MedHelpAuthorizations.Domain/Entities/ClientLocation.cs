using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    [CustomReportTypeEntityHeader(entityName:CustomReportHelper._ClientLocation,CustomTypeCode.Empty, false)]
    public partial class ClientLocation : AuditableEntity<int>, IClientRelationship, IDataPipe<int>
	{
        public ClientLocation()
        {
            // Schedule = new HashSet<Schedule>();
            ClientProviderLocations = new HashSet<ClientProviderLocation>();
            EmployeeClientLocations = new HashSet<EmployeeClientLocation>();
            ClientLocationInsuranceIdentifiers = new HashSet<ClientLocationInsuranceIdentifier>();
        }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClientLocation, CustomTypeCode.String, propertyName: CustomReportHelper.ClientLocationName)]
        public string Name { get; set; }

        public long? OfficePhoneNumber { get; set; } = null;

        public long? OfficeFaxNumber { get; set; } = null;

        public int? AddressId { get; set; }

        public int ClientId { get; set; }
        public int? EligibilityLocationId { get; set; }

        [StringLength(10)]
        public string Npi { get; set; }
        //public string TenantId { get; set; }


        [StringLength(36)]
        public string ExternalId { get; set; }

		public string DfExternalId { get; set; }

		public DateTime? DfCreatedOn { get; set; }

		public DateTime? DfLastModifiedOn { get; set; }


		#region Navigation Objects

		[ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
        //public virtual ICollection<Schedule> Schedule { get; set; }
        public virtual ICollection<ClientProviderLocation> ClientProviderLocations { get; set; }
        public virtual ICollection<EmployeeClientLocation> EmployeeClientLocations { get; set; }
        public virtual ICollection<ClientLocationInsuranceIdentifier> ClientLocationInsuranceIdentifiers { get; set; }

        #endregion
    }
}
