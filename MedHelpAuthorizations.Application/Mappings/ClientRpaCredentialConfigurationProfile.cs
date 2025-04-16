using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientRpaCredentialConfigurations.Commands;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientRpaCredentialConfigurationProfile : Profile
    {
        public ClientRpaCredentialConfigurationProfile()
        {
            CreateMap<CreateClientRpaCredentialConfigCommand, ClientRpaCredentialConfiguration>().ReverseMap();
            CreateMap<UpdateClientRpaCredentialConfigCommand, ClientRpaCredentialConfiguration>().ReverseMap();

        }
    }
}
