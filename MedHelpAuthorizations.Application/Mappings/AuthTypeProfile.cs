using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries.GetById;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class AuthTypeProfile : Profile
    {
        public AuthTypeProfile()
        {
            CreateMap<AddEditAuthTypeCommand, AuthType>().ReverseMap();
            CreateMap<GetAllPagedAuthTypesResponse, AuthType>().ReverseMap();
            CreateMap<GetAuthTypeByIdResponse, AuthType>().ReverseMap();            
        }
    }
}
