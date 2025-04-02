using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetFailed;
using MedHelpAuthorizations.Application.Features.UserAlerts.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.UserAlerts.Queries.GetById;
using MedHelpAuthorizations.Application.Features.UserAlerts.Queries.GetByUserId;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class UserAlertProfile : Profile
    {
        public UserAlertProfile()
        {
            CreateMap<AddEditUserAlertCommand, UserAlert>()
                .ReverseMap();
            CreateMap<GetUserAlertsByIdResponse, UserAlert>()
                .ReverseMap();
            CreateMap<GetUserAlertByUserIdResponse, UserAlert>() //GetUserAlertsByUserIdResponse
               .ReverseMap();

            CreateMap<AddEditUserAlertCommand, GetUserAlertByUserIdResponse>()
                .ReverseMap();

            CreateMap<AddEditUserAlertCommand, GetUserAlertsByIdResponse>()
                .ReverseMap();

            //TODO : Create map
            //CreateMap<AddEditUserAlertCommand, GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>()
            //    .ForPath(x => , map => map.MapFrom(y => y))
            //    .ForMember(x => x.Person, map => map.MapFrom(y => y))
            //    .ReverseMap();
        }
    }
}
