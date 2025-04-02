using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientApiKey.Commands.AddEdit;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientApiKeyProfile : Profile
    {
        public ClientApiKeyProfile()
        {
            CreateMap<AddEditClientApiKeyCommand, ClientApiIntegrationKey>().ReverseMap();
            CreateMap<Features.Admin.ClientApiKey.Commands.AddEditAdminClientApiKeyCommand, ClientApiIntegrationKey>().ReverseMap();
        }
    }
}
