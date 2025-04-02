using AutoMapper;
using MedHelpAuthorizations.Application.Models.IntegratedServices;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientApiIntegrationKeyProfile : Profile
    {

        public ClientApiIntegrationKeyProfile()
        {
            CreateMap<ClientApiIntegrationKey, ClientApiIntegrationKeyDto>().ReverseMap();
        }
    }
}
