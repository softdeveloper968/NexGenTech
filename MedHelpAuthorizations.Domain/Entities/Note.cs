using System;
using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;

#nullable disable

namespace MedHelpAuthorizations.Domain.Entities
{
    public class Note : AuditableEntity<int>//, ITenant
    {
        public int AuthorizationId { get; set; }
        public string NoteUserId { get; set; }
        public DateTime? NoteTs { get; set; }
        public int ClientId { get; set; }
        public string NoteContent { get; set; }
        //public string TenantId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("AuthorizationId")]
        public virtual Authorization Authorization { get; set; }
    }
}
