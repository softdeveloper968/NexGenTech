using MedHelpAuthorizations.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientNote : AuditableEntity<int>
    {
        public string Title { get; set; }
        public string Note { get; set; }

        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
    }
}
