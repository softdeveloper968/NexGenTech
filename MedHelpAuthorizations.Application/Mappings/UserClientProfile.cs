using AutoMapper;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.UserClients.Queries.GetByUserId;
using MedHelpAuthorizations.Application.Responses.Identity;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class UserClientProfile : Profile
    {
        public UserClientProfile()
        {
            CreateMap<UserClient, GetByUserIdQueryResponse>()
                .ForMember(x => x.ClientId, map => map.MapFrom(y => y.Client.Id))
                .ForMember(x => x.ClientName, map => map.MapFrom(y => y.Client.Name))
                .ForMember(x => x.ClientCode, map => map.MapFrom(y => y.Client.ClientCode))
                .ReverseMap();

            CreateMap<UserClient, ClientUserResponse>()
               .ForMember(x => x.ClientId, map => map.MapFrom(y => y.Client.Id))
               .ForMember(x => x.ClientUserId, map => map.MapFrom(y => y.Id))
               .ForMember(x => x.ClientCode, map => map.MapFrom(y => y.Client.ClientCode))
               .ReverseMap();
        }
    }
}
