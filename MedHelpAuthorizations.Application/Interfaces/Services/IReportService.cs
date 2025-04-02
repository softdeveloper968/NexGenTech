using MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters;
using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IReportService:IService
    {
        Task<List<GetAllReportsResponse>> GetAllReportsByCategory();
        Task<List<GetAllReportsResponse>> GetAllReportsByCategoryId(ReportCategoryEnum reportCategoryId);
    }
}