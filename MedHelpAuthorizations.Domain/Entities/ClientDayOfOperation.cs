using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientDayOfOperation : AuditableEntity<int>
    {
        public int DayOfWeekId { get; set; }
        public int ClientId { get; set; }
        public ClientDayOfOperation() { }

        #region
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        #endregion

    }
}
