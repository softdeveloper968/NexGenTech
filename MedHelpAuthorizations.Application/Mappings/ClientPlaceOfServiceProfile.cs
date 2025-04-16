using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientPlacesOfService.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.ClientPlacesOfService.Queries.GetById;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientPlaceOfServiceProfile : Profile
    {
        public ClientPlaceOfServiceProfile()
        {
            CreateMap<ClientPlaceOfService, GetAllClientPlacesOfServiceResponse>().ReverseMap();
            CreateMap<ClientPlaceOfService, GetClientPlaceOfServiceByIdResponse>().ReverseMap();
        }
    }
}
