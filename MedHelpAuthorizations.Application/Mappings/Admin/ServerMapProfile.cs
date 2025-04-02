using AutoMapper;
using MedHelpAuthorizations.Application.Features.Admin.Server.Commands;
using MedHelpAuthorizations.Application.Requests.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Mappings.Admin
{
    public class ServerMapProfile : Profile
    {
        public ServerMapProfile()
        {
            CreateMap<AddEditServerCommand, AddEditServerRequest>().ReverseMap();
        }
    }
}
