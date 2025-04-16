using AutoMapper;
using MedHelpAuthorizations.Application.Features.Admin.Tenant.Commands;
using MedHelpAuthorizations.Application.Requests.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Mappings.Admin
{
    public class TenantMapProfile: Profile
    {
        public TenantMapProfile()
        {
            CreateMap<AddEditTenantCommand, AddEditTenantRequest>().ReverseMap();
        }
    }
}
