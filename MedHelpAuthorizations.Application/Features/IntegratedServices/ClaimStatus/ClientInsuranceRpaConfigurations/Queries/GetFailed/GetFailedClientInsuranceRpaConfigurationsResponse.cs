using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetFailed
{
    public class GetFailedClientInsuranceRpaConfigurationsResponse : GetClientInsuranceRpaConfigurationBaseResponse
    {   
        public string ClientCode { get; set; }
        public string ClientInsuranceLookupName { get; set; }
        public string AuthTypeName { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ReportFailureToEmail { get; set; }
        public string ExpiryWarningReported { get; set; }
    }
}
