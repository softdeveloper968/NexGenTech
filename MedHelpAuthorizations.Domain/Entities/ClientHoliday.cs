using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientHoliday : AuditableEntity<int>
    {
        public HolidaysEnum HolidayId { get; set; }
        public int ClientId { get; set; }
        public ClientHoliday() { }

        #region
        [ForeignKey("HolidayId")]
        public virtual Holiday Holiday { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        #endregion

    }
}
