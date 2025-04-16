using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetFailed;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientInsuranceRpaFailedConfigurationsProfile : Profile
    {
        public ClientInsuranceRpaFailedConfigurationsProfile()
        {
            CreateMap<ClientInsuranceRpaConfiguration, GetFailedClientInsuranceRpaConfigurationsResponse>()
                .ForPath(x => x.ClientId, map => map.MapFrom(y => y.ClientInsurance.ClientId))
                .ForPath(x => x.ExternalId, map => map.MapFrom(y => y.ClientInsurance.ExternalId))
                .ForPath(x => x.TargetUrl, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance != null
                                                            && !string.IsNullOrWhiteSpace(y.ClientInsurance.RpaInsurance.TargetUrl)
                                                            ? y.ClientInsurance.RpaInsurance.TargetUrl
                                                            : (y.ClientRpaCredentialConfiguration != null ? y.ClientRpaCredentialConfiguration.RpaInsuranceGroup.DefaultTargetUrl : string.Empty)))
                .ForPath(x => x.Username, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.Username))
                .ForPath(x => x.Password, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.Password))
                .ForPath(x => x.RpaInsuranceId, map => map.MapFrom(y => y.ClientInsurance.RpaInsuranceId))
                .ForPath(x => x.RpaInsuranceGroupId, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance != null ? y.ClientInsurance.RpaInsurance.RpaInsuranceGroupId : null))
                .ForPath(x => x.FailureReported, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.FailureReported))
                .ForPath(x => x.FailureMessage, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.FailureMessage))
                .ForPath(x => x.ExpiryWarningReported, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.ExpiryWarningReported))
                .ForPath(x => x.UseOffHoursOnly, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.UseOffHoursOnly))
                .ForPath(x => x.IsCredentialInUse, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.IsCredentialInUse))
                .ForPath(x => x.ClientRpaCredentialConfigId, map => map.MapFrom(y => y.ClientRpaCredentialConfigurationId))
                .ForPath(x => x.ClientCode, map => map.MapFrom(y => y.ClientInsurance.Client.ClientCode))
                .ForPath(x => x.AuthTypeName, map => map.MapFrom(y => y.AuthType.Name))
                .ForPath(x => x.AuthTypeId, map => map.MapFrom(y => y.AuthType.Id))
                .ForPath(x => x.ReportFailureToEmail, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.ReportFailureToEmail))
                .ReverseMap();
        }
    }
}
