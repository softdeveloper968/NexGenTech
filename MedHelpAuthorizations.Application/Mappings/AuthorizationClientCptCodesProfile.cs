using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetById;
using MedHelpAuthorizations.Domain.Entities;


namespace MedHelpAuthorizations.Application.Mappings
{
    public class AuthorizationClientCptCodesProfile : Profile
    {

        //CreateMap<ClientCptCode, GetAllPagedClientCptCodesResponse>().ReverseMap();
        //CreateMap<GetClientCptCodeByIdResponse, ClientCptCode>().ReverseMap();
        //CreateMap<ClientCptCode, AddEditClientCptCodeCommand>().ReverseMap();
        public AuthorizationClientCptCodesProfile()
        {
            //CreateMap<ClientCptCode, GetClientCptCodeByIdResponse>().ReverseMap();
            CreateMap<GetClientCptCodeByIdResponse, AuthorizationClientCptCode>()
                .ForPath(a => a.ClientCptCode, vm => vm.MapFrom(c => c))
                .ForPath(a => a.ClientCptCodeId, vm => vm.MapFrom(c => c.Id)).ReverseMap();
            CreateMap<GetAllPagedClientCptCodesResponse, AuthorizationClientCptCode>()
                .ForPath(a => a.ClientCptCode, vm => vm.MapFrom(c => c))
                .ForPath(a => a.ClientCptCodeId, vm => vm.MapFrom(c => c.Id));
            CreateMap<GetAllPagedClientCptCodesResponse, ClientCptCode>().ReverseMap(); 
            //CreateMap<AuthorizationClientCptCode, GetClientCptCodeByIdResponse>()
            //    .ForPath(a => a.Code, vm => vm.MapFrom(c => c.ClientCptCode.Code))
            //    .ForPath(a => a.Id, vm => vm.MapFrom(c => c.ClientCptCodeId))
            //    .ForPath(a => a.Description, vm => vm.MapFrom(c => c.ClientCptCode.Description))
            //    .ForPath(a => a.ShortDescription, vm => vm.MapFrom(c => c.ClientCptCode.ShortDescription))
            //    .ForPath(a => a.CodeVersion, vm => vm.MapFrom(c => c.ClientCptCode.CodeVersion))
            //    .ForPath(a => a.CptCodeGroupId, vm => vm.MapFrom(c => c.ClientCptCode.CptCodeGroupId));

            //CreateMap<AuthorizationClientCptCode, AddEditClientCptCodeCommand>().ReverseMap();
        }
    }
}
