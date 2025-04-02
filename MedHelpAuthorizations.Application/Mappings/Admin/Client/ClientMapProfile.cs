using AutoMapper;
using MedHelpAuthorizations.Application.Features.Admin.Client.Commands;
using MedHelpAuthorizations.Application.Requests.Admin.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Mappings.Admin.Client
{
    public class ClientMapProfile : Profile
    {
        public ClientMapProfile()
        {
            CreateMap<AddEditAdminClientCommand, AddEditAdminClientRequest>().ReverseMap();
        }
    }
}
