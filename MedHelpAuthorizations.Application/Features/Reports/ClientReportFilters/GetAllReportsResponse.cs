using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters
{
    public class GetAllReportsResponse
    {
        public string Name { get; set; }
        public ReportCategoryEnum ReportCategory { get; set; }
    }
}
