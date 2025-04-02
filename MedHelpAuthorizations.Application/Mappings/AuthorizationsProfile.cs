using AutoMapper;
using MedHelpAuthorizations.Application.Features.Authorizations.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetByCriteriaPaged;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Reports.Queries.GetExpiringAuthorizations;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export;
using MedHelpAuthorizations.Domain.Entities;
using System.Linq;
using Authorization = MedHelpAuthorizations.Domain.Entities.Authorization;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class AuthorizationProfile : Profile
    {
        public AuthorizationProfile()
        {
            
            //GetByCriteriaPagedAuthorizationsResponse
            CreateMap<GetByCriteriaPagedAuthorizationsResponse, Authorization>()
                .ForPath(x => x.AuthType.Name, map => map.MapFrom(y => y.AuthTypeName))
                .ReverseMap()
                .ForPath(dest => dest.AuthTypeName, map => map.MapFrom(src => src.AuthType.Name));
            CreateMap<GetAllPagedAuthorizationsResponse, Authorization>()
                .ForPath(x => x.AuthType.Name, map => map.MapFrom(y => y.AuthTypeName))
                .ReverseMap()
                .ForPath(dest => dest.AuthTypeName, map => map.MapFrom(src => src.AuthType.Name));
            CreateMap<AddEditAuthorizationCommand, Authorization>()                
                .AfterMap((s, d) => d.AuthorizationClientCptCodes.ToList().ForEach(x => x.ClientCptCode = null))
                .ReverseMap();
            CreateMap<AddEditAuthorizationCommand, GetAuthorizationByIdResponse>().ReverseMap();
            CreateMap<GetAllPagedAuthorizationsResponse, Authorization>().ReverseMap();
            CreateMap<GetAuthorizationByIdResponse, Authorization>().ReverseMap();
            CreateMap<GetAllPagedAuthorizationsResponse, GetAuthorizationByIdResponse>().ReverseMap();

            //ExportExpiringAuthorizationQuery            
            CreateMap<GetPagedExpiringAuthorizationsQuery, ExportExpiringAuthorizationsQuery>().ReverseMap();
        }
    }
}
