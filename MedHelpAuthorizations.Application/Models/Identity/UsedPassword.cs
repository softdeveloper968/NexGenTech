using MedHelpAuthorizations.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Application.Models.Identity
{
    public class UsedPassword : AuditableEntity<int>
    {
        public string UserId { get; set; }
        public string HashedPassword { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
