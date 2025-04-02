using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Queries.GetByClientId;
using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries.GetAllPaged;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Base;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientAuthTypeProfile : Profile
    {
        public ClientAuthTypeProfile()
        {
            CreateMap<ClientAuthType, ClientAuthTypeDto>()
              .ForPath(d => d.AuthTypeId, map => map.MapFrom(s => s.AuthTypeId))
              .ForPath(d => d.ClientId, map => map.MapFrom(s => s.ClientId))
              .ReverseMap(); 

            CreateMap<AuthType, GetAllPagedAuthTypesResponse>().ReverseMap();
            CreateMap<GetClientAuthTypesByClientIdResponse, ClientAuthType>().ReverseMap();                
        }
    }
}
