using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocationInsuranceIdentifier.Base;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocationInsuranceIdentifier.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocationInsuranceIdentifier.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocationInsuranceIdentifier.Queries.GetByLocationId;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientLocationInsuranceIdentifierProfile : Profile
    {
        public ClientLocationInsuranceIdentifierProfile()
        {
            CreateMap<ClientLocationInsuranceIdentifier, ClientLocationInsuranceIdentifierDto>().ReverseMap();
            CreateMap<ClientLocationInsuranceIdentifier, GetClientLocationInsuranceIdentifierByIdResponse>().ReverseMap();
            CreateMap<ClientLocationInsuranceIdentifier, AddEditClientLocationInsuranceIdentifierCommand>().ReverseMap();
            CreateMap<ClientLocationInsuranceIdentifier, GetClientLocationInsuranceIdentifierByLocationIdResponse>();
            //CreateMap<ClientLocationInsuranceIdentifier, GetClientLocationInsuranceIdentifierByClientIdResponse>().ReverseMap();
        }
    }
}
