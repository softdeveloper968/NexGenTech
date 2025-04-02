using AutoMapper;
using MedHelpAuthorizations.Application.Features.Authorizations.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Mappings
{
    public class AuthorizationProfile : Profile
    {
        public AuthorizationProfile()
        {
            CreateMap<AddEditAuthorizationCommand, GetAuthorizationByIdResponse>().ReverseMap();
        }
    }
}
