using System.Collections.Generic;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Helpers;

namespace MedHelpAuthorizations.Domain.Entities
{
    [CustomReportTypeEntityHeader(entityName:CustomReportHelper._AuthType,typeCode:CustomTypeCode.Empty,false)]
    public class AuthType : AuditableEntity<int>
    {
        public AuthType()
        {
            ClientAuthTypes = new HashSet<ClientAuthType>();
            ClientInsuranceRpaConfigurations = new HashSet<ClientInsuranceRpaConfiguration>();
            ClaimStatusBatches = new HashSet<ClaimStatusBatch>();
        }
        [CustomReportTypeColumnsHeaderForMainEntity(entityName:CustomReportHelper._AuthType,CustomTypeCode.String,propertyName:CustomReportHelper.AuthTypeName)]
        public string Name { get; set; }
        [CustomReportTypeColumnsHeaderForMainEntity(entityName:CustomReportHelper._AuthType,CustomTypeCode.String,propertyName:CustomReportHelper.AuthTypeDescription)]
        public string Description { get; set; }

        public virtual ICollection<ClientAuthType> ClientAuthTypes { get; set; }
        public virtual ICollection<ClientInsuranceRpaConfiguration> ClientInsuranceRpaConfigurations { get; set; }
        public virtual ICollection<ClaimStatusBatch> ClaimStatusBatches { get; set; }

    }
}
