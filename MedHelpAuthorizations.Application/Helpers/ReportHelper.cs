using MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Helpers
{
    public class ReportHelper
    {
        public static bool AreListsEqual<T>(List<T> list1, List<T> list2)
        {
            if (list1 == null && list2 == null)
                return true;

            if (list1 == null || list2 == null)
                return false;

            if (list1.Count != list2.Count)
                return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (!list1[i].Equals(list2[i]))
                    return false;
            }

            return true;
        }

        #region String constants EN-584
        public const string Summary = "Summary";
        public const string AITAuthor = "Automated Integration Technologies";
        public const string PerStatus = "PerStatus";
        public const string PerPayer = "PerPayer";
        public const string PivotTableCreationError = "Error creating status summary pivot table.";
        public const string Count_of_Lineitem_Status = "Count of Lineitem Status";
        public const string Sum_of_Billed_Amt = "Sum of Billed Amt";
        public const string Count_of_Billed_Amt = "Count of Billed Amt";
        public const string Sum_of_Allowed_Amt = "Sum of Allowed Amt";
        public const string Sum_of_Lineitem_Paid_Amt = "Sum of Lineitem Paid Amt";
        public const string Sum_of_NonAllowed_Paid_Amt = "Sum of Non-Allowed Paid Amt";
        public const string Sum_of_Deductible_Amt = "Sum of Deductible Amt";
        public const string Sum_of_Copay_Amt = "Sum of Copay Amt";
        public const string Sum_of_Penality_Amt = "Sum of Penality Amt";
        public const string Payer_Summary = "Payer Summary";
        public const string Denial_Summary = "Denial Summary";
        public const string Payer_Denial_Summary = "Payer Denial Summary";
        public const string Exception_Category = "Exception Category";
        public const string Count_of_Exception_Category = "Count of Exception Category";
        public const string Count_of_Payer_Name = "Count of Payer Name";
        public const string Financial_Summary = "Financial Summary";

        #endregion
    }
}
