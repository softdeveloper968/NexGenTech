using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Enums
{
    public enum ClaimTransactionDialogTypesEnum : int
    {
        [Description("MarkAsPaidStatus")]
        MarkAsPaidStatus = 0,
        [Description("ChangeClaimTransactionStatus")]
        ChangeClaimTransactionStatus = 1,
        [Description("AddEditNotes")]
        AddNotes = 2,
        [Description("AddEditReasonNotes")]
        AddEditReasonNotes = 3,
        [Description("EditNote")]
        EditNote = 4,

    }
}
