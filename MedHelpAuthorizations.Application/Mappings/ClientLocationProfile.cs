using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Base;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllByProviderId;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetByName;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientLocationProfile : Profile
    {
        public ClientLocationProfile()
        {
            CreateMap<ClientLocation, GetAllClientLocationsResponse>().ReverseMap();
            CreateMap<ClientLocationDto, ClientLocation>().ReverseMap();
            CreateMap<ClientLocation, GetClientLocationByIdResponse>().ReverseMap();
            CreateMap<ClientLocation, GetClientLocationsByClientIdResponse>().ReverseMap();
            CreateMap<ClientLocation, GetClientLocationsByProviderIdResponse>().ReverseMap();
            CreateMap<AddEditClientLocationCommand, ClientLocation>().ReverseMap();
            CreateMap<GetClientLocationByNameResponse, ClientLocation>().ReverseMap();      
        }
    }
}
