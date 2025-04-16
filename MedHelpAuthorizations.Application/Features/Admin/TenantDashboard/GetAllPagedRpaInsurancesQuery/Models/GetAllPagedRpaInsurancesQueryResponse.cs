using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.GetAllPagedRpaInsurancesQuery.Models
{
    public class GetAllPagedRpaInsurancesQueryResponse
    {
        public int Id { get; set; }
        public string RPACode { get; set; }
        public string TargetUrl { get; set; }
        public string GroupName { get; set; }
        public string DefaultTargetUrl { get; set; }
        public int ClaimBilledOnWaitDays { get; set; }
        public int ApprovalWaitPeriodDays { get; set; }
        public DateTime? InActivatedOn { get; set; }
    }
}
