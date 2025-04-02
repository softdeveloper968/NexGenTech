using MedHelpAuthorizations.Application.Features.Reports.ARAgingReport;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ARAgingDataResponse
    {
        public List<ARAgingData> ARAgingData { get; set; }

        // ChargedSum
        public decimal AgeGroup_0_30 { get; set; } = 0.0m;
        public decimal AgeGroup_31_60 { get; set; } = 0.0m;
        public decimal AgeGroup_61_90 { get; set; } = 0.0m;
        public decimal AgeGroup_91_120 { get; set; } = 0.0m;
        public decimal AgeGroup_121_150 { get; set; } = 0.0m;
        public decimal AgeGroup_151_180 { get; set; } = 0.0m;
        public decimal AgeGroup_181_plus { get; set; } = 0.0m;

        // Quantity
        public int AgeGroup_0_30_Qty { get; set; } = 0;
        public int AgeGroup_31_60_Qty { get; set; } = 0;
        public int AgeGroup_61_90_Qty { get; set; } = 0;
        public int AgeGroup_91_120_Qty { get; set; } = 0;
        public int AgeGroup_121_150_Qty { get; set; } = 0;
        public int AgeGroup_151_180_Qty { get; set; } = 0;
        public int AgeGroup_181_plus_Qty { get; set; } = 0;
    }
    public class ARAgingData : ARAgingTotals
    {
        public decimal Quantity { get; set; } = 0.0m;
        public decimal ChargedSum { get; set; } = 0.0m;
        public string LocationName { get; set; } = string.Empty;
        public string ProviderName { get; set; } = string.Empty;
        public string ClaimBilledOn { get; set; }
        public string DateOfServiceFrom { get; set; }
        public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; }
        public DateTime MyProperty { get; set; }
    }
}
