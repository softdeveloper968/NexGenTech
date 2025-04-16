using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.SortDefinitions
{
    public static partial class SortDefinitions
    {
        public static string RPAInsurancesDefinitionsPrefix = "RPAInsurances";
        public static Dictionary<string, string> RPAInsurancesDefinitions = new Dictionary<string, string>()
        {
            {"id","Id" },
            {"rpaCode","RPACode"},
            {"targetUrl","TargetUrl" },
            {"groupName" ,"GroupName"},
            {"defaultTargetUrl","DefaultTargetUrl" },
            {"claimBilledOnWaitDays","ClaimBilledOnWaitDays" },
            {"approvalWaitPeriodDays","ApprovalWaitPeriodDays" },
            {"inActivatedOn","InActivatedOn" }
        };
    }
}
