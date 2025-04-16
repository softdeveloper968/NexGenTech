using AutoMapper;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Features.Administration.Client_ApplicationFeatures;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientApplicationFeatureProfile : Profile
    {
        public ClientApplicationFeatureProfile()
        {
            CreateMap<ClientApplicationFeatureDto, ClientApplicationFeature>().ReverseMap();             
        }
    }
}
