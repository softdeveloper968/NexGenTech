using AutoMapper;
using MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class EmployeeClientUserReportFilterProfile : Profile
    {
        public EmployeeClientUserReportFilterProfile()
        {
            CreateMap<EmployeeClientUserReportFilter, EmployeeClientReportFilterDTO>().ReverseMap();
            CreateMap<EmployeeClientUserReportFilter, AddEmployeeClientUserReportFilterCommand>().ReverseMap();
        }
    }
}
