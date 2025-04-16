using MedHelpAuthorizations.Client.Shared.Models.DashboardPresets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base
{
    public class ComparisonDashboardQuery : IComparisonDashboardQuery
    {
        public DashboardPresets.PresetFilterTypeEnum PresetFilterType { get; set; } = DashboardPresets.PresetFilterTypeEnum.BilledOnDate;
        public DateTime? DateOfServiceFrom { get; set; }
        public DateTime? DateOfServiceTo { get; set; }
        public DateTime? ClaimBilledFrom { get; set; }
        public DateTime? ClaimBilledTo { get; set; }
        public string ClientInsuranceIds { get; set; } = string.Empty;
        public string ClientExceptionReasonCategoryIds { get; set; } = string.Empty;
        public string ClientAuthTypeIds { get; set; } = string.Empty;
        public string ClientProcedureCodes { get; set; } = string.Empty;
        public string ClientLocationIds { get; set; } = string.Empty;
        public string ClientProviderIds { get; set; } = string.Empty;
    }
}
