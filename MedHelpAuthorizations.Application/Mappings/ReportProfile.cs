using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClaimStatus.Queries.GetEmployeeClaimStatusByEmployeeID;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<Report, GetAllReportsResponse>().ReverseMap();
            CreateMap<EmployeesClaimStatusResponseModel, ExportQueryResponse>().ReverseMap();
        }
    }
}
