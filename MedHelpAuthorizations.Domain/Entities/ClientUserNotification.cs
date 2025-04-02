using MedHelpAuthorizations.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientUserNotification : AuditableEntity<int>
    {
        public string FileName { get; set; }
        public string UserId { get; set; }
        public int ClientId { get; set; }
        public string FileUrl { get; set; }
        public bool IsDownload {  get; set; } = false;
        public string FileStatus { get; set; }
        public string ErrorMessage { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
    }
}
