using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum DepartmentEnum
    {
        /// <summary>
        /// Front Desk
        /// </summary>
        [Description("Registor")]
        Registor = 1,

        /// <summary>
        /// Medical
        /// </summary>
        [Description("Medical")]
        Medical = 2,

        /// <summary>
        /// Billing
        /// </summary>
        [Description("Billing")]
        Billing = 3,

        /// <summary>
        /// Credentialing
        /// </summary>
        [Description("Credentialing")]
        Credentialing,
        
        [Description("Charge Entry")]
        ChargeEntry,

        [Description("Cash Posting")]
        CashPosting
    }
}
