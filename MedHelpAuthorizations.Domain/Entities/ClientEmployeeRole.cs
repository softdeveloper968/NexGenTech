using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientEmployeeRole : AuditableEntity<int>
    {
        public ClientEmployeeRole() { }

        public int EmployeeClientId { get; set; }

        public EmployeeRoleEnum EmployeeRoleId { get; set; }


        [ForeignKey("EmployeeClientId")]
        public virtual EmployeeClient EmployeeClient { get; set; }

        [ForeignKey("EmployeeRoleId")]
        public virtual EmployeeRole EmployeeRole { get; set; }
    }
}
