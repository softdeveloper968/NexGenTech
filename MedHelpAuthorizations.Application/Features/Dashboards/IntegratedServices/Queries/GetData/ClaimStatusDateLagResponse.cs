using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ClaimStatusDateLagResponse
    {
        public string ClaimNumber { get; set; }
        public int Quantity { get; set; }
        public string ClaimLevelMd5Hash { get; set; }
        public int ServiceToBilledDateLag { get; set; } = 0;
        public int ServiceToPaymentDateLag { get; set; } = 0;
        public int BilledToPaymentDateLag { get; set; } = 0;
        public decimal AvgServiceToBilledDateLag { get; set; } = 0.00m;
        public decimal AvgServiceToPaymentDateLag { get; set; } = 0.00m;
        public decimal AvgBilledToPaymentDateLag { get; set; } = 0.00m;     
    }
}
