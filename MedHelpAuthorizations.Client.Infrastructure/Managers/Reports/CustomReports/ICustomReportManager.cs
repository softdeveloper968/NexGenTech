using MedHelpAuthorizations.Application.Features.Reports.CustomReports.Query;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Domain.CustomAttributes.CustomReport;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Reports.CustomReports
{
    public interface ICustomReportManager : IManager
    {
        Task<IResult<CustomReportTypeEntity>> GetFilterColumnsBasedOnReportType(CustomReportTypeEnum customReportType);
        Task<IResult<UpdatedClaimReportTypePreviewModel>> GetPreviewReportForClaimReportType(CustomPreviewsReportQuery previewReportPayload);
        Task<IResult<string>> ExportPreviewReport(ExportPreviewReportQuery query);
    }
}
