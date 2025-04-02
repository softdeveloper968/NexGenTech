using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetBase;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetAll
{
    public class GetAllClientInsuranceRpaConfigurationsResponse : GetClientInsuranceRpaConfigurationBaseResponse
    {
        public string DefaultTargetUrl { get; set; }
        public string AuthTypeName { get; set; }

    }
}
