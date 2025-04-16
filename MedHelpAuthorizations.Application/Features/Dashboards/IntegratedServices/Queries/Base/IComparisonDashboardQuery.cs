using MedHelpAuthorizations.Client.Shared.Models.DashboardPresets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base
{
    public interface IComparisonDashboardQuery
    {
        public DashboardPresets.PresetFilterTypeEnum PresetFilterType { get; set; }
        public DateTime? DateOfServiceFrom { get; set; }
        public DateTime? DateOfServiceTo { get; set; }
        public DateTime? ClaimBilledFrom { get; set; }
        public DateTime? ClaimBilledTo { get; set; }
        public string ClientInsuranceIds { get; set; }
        public string ClientExceptionReasonCategoryIds { get; set; }
        public string ClientAuthTypeIds { get; set; }
        public string ClientProcedureCodes { get; set; }
        public string ClientLocationIds { get; set; }
        public string ClientProviderIds { get; set; }
    }
}
