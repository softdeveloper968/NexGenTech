using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Commands.Create;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Commands.Update;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetById;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetByRpaInsurance;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetByUserrnameAndUrl;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetFailed;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientRpaCredentialConfigurations.Commands;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Mappings
{
	public class ClientInsuranceRpaConfigurationProfile : Profile
    {
        public ClientInsuranceRpaConfigurationProfile()
        {
            CreateMap<CreateClientInsuranceRpaConfigurationCommand, ClientInsuranceRpaConfiguration>().ReverseMap();
            CreateMap<UpdateClientInsuranceRpaConfigurationCommand, ClientInsuranceRpaConfiguration>().ReverseMap();

            CreateMap<GetClientInsuranceRpaConfigurationByIdResponse, ClientInsuranceRpaConfiguration>();
            CreateMap<ClientInsuranceRpaConfiguration, GetClientInsuranceRpaConfigurationByIdResponse>()
                .ForPath(x => x.ClientId, map => map.MapFrom(y => y.ClientInsurance.ClientId))
                .ForPath(x => x.ExternalId, map => map.MapFrom(y => y.ClientInsurance.ExternalId))
                .ForPath(x => x.TargetUrl, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance != null
                                                            && !string.IsNullOrWhiteSpace(y.ClientInsurance.RpaInsurance.TargetUrl)
                                                            ? y.ClientInsurance.RpaInsurance.TargetUrl
                                                            : (y.ClientRpaCredentialConfiguration != null ? y.ClientRpaCredentialConfiguration.RpaInsuranceGroup.DefaultTargetUrl : string.Empty)))
                .ForPath(x => x.Username, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.Username))
                .ForPath(x => x.Password, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.Password))
                .ForPath(x => x.RpaInsuranceId, map => map.MapFrom(y => y.ClientInsurance.RpaInsuranceId))
                .ForPath(x => x.FailureReported, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.FailureReported))
                .ForPath(x => x.FailureMessage, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.FailureMessage));

            //CreateMap<GetClientInsuranceRpaConfigurationByCriteriaResponse, ClientInsuranceRpaConfiguration>();
            CreateMap<ClientInsuranceRpaConfiguration, GetClientInsuranceRpaConfigurationByCriteriaResponse>()
                .ForPath(x => x.ClientId, map => map.MapFrom(y => y.ClientInsurance.ClientId))
                .ForPath(x => x.ExternalId, map => map.MapFrom(y => y.ClientInsurance.ExternalId))
                .ForPath(x => x.TargetUrl, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance != null
                                                            && !string.IsNullOrWhiteSpace(y.ClientInsurance.RpaInsurance.TargetUrl)
                                                            ? y.ClientInsurance.RpaInsurance.TargetUrl
                                                            : (y.ClientRpaCredentialConfiguration != null ? y.ClientRpaCredentialConfiguration.RpaInsuranceGroup.DefaultTargetUrl : string.Empty)))
                .ForPath(x => x.Username, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.Username))
                .ForPath(x => x.Password, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.Password))
                .ForPath(x => x.AlternateUsername, map => map.MapFrom(y => y.AlternateClientRpaCredentialConfiguration.Username))
                .ForPath(x => x.AlternatePassword, map => map.MapFrom(y => y.AlternateClientRpaCredentialConfiguration.Password))
                .ForPath(x => x.RpaInsuranceId, map => map.MapFrom(y => y.ClientInsurance.RpaInsuranceId))
                .ForPath(x => x.RpaInsuranceGroupId, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance != null ? y.ClientInsurance.RpaInsurance.RpaInsuranceGroupId : null))
                .ForPath(x => x.OtpForwardFromEmail, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.OtpForwardFromEmail))
                .ForPath(x => x.FailureReported, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.FailureReported))
                .ForPath(x => x.FailureMessage, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.FailureMessage))
                .ForPath(x => x.ExpiryWarningReported, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.ExpiryWarningReported))
                .ForPath(x => x.UseOffHoursOnly, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.UseOffHoursOnly))
                .ForPath(x => x.IsCredentialInUse, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.IsCredentialInUse))
                .ForPath(x => x.IsAlternateCredentialInUse, map => map.MapFrom(y => y.AlternateClientRpaCredentialConfiguration.IsCredentialInUse))
                .ForPath(x => x.ClientRpaCredentialConfigId, map => map.MapFrom(y => y.ClientRpaCredentialConfigurationId))
                .ForPath(x => x.AlternateClientRpaCredentialConfigId, map => map.MapFrom(y => y.AlternateClientRpaCredentialConfigurationId));

            CreateMap<ClientInsuranceRpaConfiguration, GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>()
                .ForPath(x => x.ClientCode, map => map.MapFrom(y => y.ClientInsurance.Client.ClientCode))
                .ForPath(x => x.ClientInsuranceLookupName, map => map.MapFrom(y => y.ClientInsurance.LookupName))
                .ForPath(x => x.RpaGroupName, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.RpaInsuranceGroup != null ? y.ClientRpaCredentialConfiguration.RpaInsuranceGroup.Name : string.Empty))
                .ForPath(x => x.Username, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.Username))
                .ForPath(x => x.AuthTypeName, map => map.MapFrom(y => y.AuthType != null ? y.AuthType.Name : string.Empty))
                .ForPath(x => x.ReportFailureToEmail, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration != null ? y.ClientRpaCredentialConfiguration.ReportFailureToEmail : string.Empty))
                .ForPath(x => x.CreatedOn, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.CreatedOn))
                .ForPath(x => x.LastModifiedOn, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.LastModifiedOn))
                .ForPath(x => x.FailureReported, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.FailureReported))
                .ForPath(x => x.FailureMessage, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.FailureMessage))
                .ForPath(x => x.TargetUrl, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance != null
                                                            && !string.IsNullOrWhiteSpace(y.ClientInsurance.RpaInsurance.TargetUrl)
                                                            ? y.ClientInsurance.RpaInsurance.TargetUrl
                                                            : (y.ClientRpaCredentialConfiguration != null ? y.ClientRpaCredentialConfiguration.RpaInsuranceGroup.DefaultTargetUrl : string.Empty)))
                .ForPath(x => x.ExpiryWarningReported, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.ExpiryWarningReported))
            .ForPath(x => x.Password, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.Password));

            CreateMap<GetClientInsuranceRpaConfigurationsByUsernameAndUrlResponse, ClientInsuranceRpaConfiguration>();
            CreateMap<ClientInsuranceRpaConfiguration, GetClientInsuranceRpaConfigurationsByUsernameAndUrlResponse>()
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
                .ForPath(x => x.ClientRpaCredentialConfigId, map => map.MapFrom(y => y.ClientRpaCredentialConfigurationId));

            CreateMap<GetClientInsuranceRpaConfigurationByRpaInsuranceResponse, ClientInsuranceRpaConfiguration>();
            CreateMap<ClientInsuranceRpaConfiguration, GetClientInsuranceRpaConfigurationByRpaInsuranceResponse>()
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
                .ForPath(x => x.ClientRpaCredentialConfigId, map => map.MapFrom(y => y.ClientRpaCredentialConfigurationId));

            CreateMap<GetAllClientInsuranceRpaConfigurationsResponse, ClientInsuranceRpaConfiguration>();

            CreateMap<ClientInsuranceRpaConfiguration, GetAllClientInsuranceRpaConfigurationsResponse>()
                .ForPath(x => x.ClientId, map => map.MapFrom(y => y.ClientInsurance.ClientId))
                .ForPath(x => x.DefaultTargetUrl, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance != null ? y.ClientInsurance.RpaInsurance.RpaInsuranceGroup.DefaultTargetUrl : string.Empty))

                .ForMember(x => x.Username, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.Username))
                .ForMember(x => x.TargetUrl, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance != null ? y.ClientInsurance.RpaInsurance.RpaInsuranceGroup.DefaultTargetUrl : string.Empty))
                .ForMember(x => x.FailureReported, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.FailureReported))
                .ForMember(x => x.ExpiryWarningReported, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.ExpiryWarningReported))
                .ForPath(x => x.Password, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.Password))
                .ForPath(x => x.RpaInsuranceId, map => map.MapFrom(y => y.ClientInsurance.RpaInsuranceId))
                .ForPath(x => x.RpaInsuranceGroupId, map => map.MapFrom(y => y.ClientInsurance.RpaInsurance != null ? y.ClientInsurance.RpaInsurance.RpaInsuranceGroupId : null))
                .ForPath(x => x.FailureReported, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.FailureReported))
                .ForPath(x => x.FailureMessage, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.FailureMessage))
                .ForPath(x => x.ExpiryWarningReported, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.ExpiryWarningReported))
                .ForPath(x => x.UseOffHoursOnly, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.UseOffHoursOnly))
                .ForPath(x => x.IsCredentialInUse, map => map.MapFrom(y => y.ClientRpaCredentialConfiguration.IsCredentialInUse))
                .ForPath(x => x.ClientRpaCredentialConfigId, map => map.MapFrom(y => y.ClientRpaCredentialConfigurationId))
                .ReverseMap();


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

            CreateMap<ResetClientRpaCredentialConfigCommand, ClientRpaCredentialConfiguration>().ReverseMap();
		}
    }
}
