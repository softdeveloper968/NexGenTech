using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum EmployeeLevelEnum
    {
        [Description("Executive Level")]
        Executive = 1,

        [Description("Supervisor Level")]
        SupervisorLevel = 2,

        [Description("Manager Level")]
        ManagerLevel = 3,

        [Description("Non-Management Level")]
        NonManagementLevel = 4
    }
}
