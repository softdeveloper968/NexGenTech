using AutoMapper;
using MedHelpAuthorizations.Application.Requests.Identity;
using MedHelpAuthorizations.Application.Responses.Identity;

namespace MedHelpAuthorizations.Client.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<PermissionResponse, PermissionRequest>().ReverseMap();
            CreateMap<RoleClaimsResponse, RoleClaimsRequest>().ReverseMap();
        }
    }
}