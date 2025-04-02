using AutoMapper;
using MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientReportFilterProfile : Profile
    {
        public ClientReportFilterProfile()
        {
            CreateMap<ClientUserReportFilter, GetClientReportFilterResponse>().ReverseMap();
            CreateMap<ClientUserReportFilter, AddEditClientReportFiltersCommand>().ReverseMap();
        }
    }
}
