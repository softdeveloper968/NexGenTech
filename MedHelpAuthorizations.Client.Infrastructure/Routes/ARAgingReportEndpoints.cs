using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class ARAgingReportEndpoints
    {
        public static string GetARAgingSummaryClaimReportByCriteria = "api/v1/tenant/Reports/GetARAgingReportByCriteria";
        public static string GetARAgingTotalsByCriteria = "api/v1/tenant/Reports/GetARAgingTotalsByCriteria";
        public static string ExportReverseAnalysisData = "api/v1/tenant/Reports/ExportReverseAnalysisData";
    }
}
