using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Queries.GetClientLocationServiceTypes;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientLocationServiceTypesProfile:Profile
    {
        public ClientLocationServiceTypesProfile()
        {
            CreateMap<ClientLocationTypeOfService, GetClientLocationServiceTypesResponse>().ReverseMap();
        }
    }
}
