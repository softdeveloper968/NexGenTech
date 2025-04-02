using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ProviderComparisonResponse : ClaimStatusTotal
    {
        public int? Visits { get; set; }
        public int? EstablishedPTVisits { get; set; }
        public int? NewPTVisits { get; set; }
        public decimal? Charges { get; set; }
        public decimal? AvgChargeAmt { get; set; }
        public decimal? AvgEstablishedPTOVLevel { get; set; }
        public decimal? AvgNewPTOVLevel { get; set; }
        public int? Denial { get; set; }
        public decimal? DenialAmt { get; set; }
    }
}
