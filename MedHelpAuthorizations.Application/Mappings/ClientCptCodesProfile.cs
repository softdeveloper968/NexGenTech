using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetById;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientCptCodesProfile : Profile
    {
        public ClientCptCodesProfile()
        {
			CreateMap<ClientCptCode, GetAllPagedClientCptCodesResponse>().ReverseMap();
			CreateMap<GetClientCptCodeByIdResponse, ClientCptCodeDto>().ReverseMap();
			CreateMap<GetClientCptCodeByIdResponse, ClientCptCode>().ReverseMap();
            CreateMap<ClientCptCode, AddEditClientCptCodeCommand>().ReverseMap();
            CreateMap<ClientCptCode, ClientCptCodeDto>().ReverseMap();
        }
    }
}
