using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientDocument : AuditableEntity<int>
    {
        public ClientDocument() { }
        public string Title { get; set; }
        public string Comments { get; set; }
        public string URL { get; set; }
        public int ClientId { get; set; }
        public System.DateTime? DocumentDate { get; set; } = DateTime.UtcNow;
        [Required]
        public string FileName { get; set; }
        public int ByteLength { get; set; }
        public UploadType UploadType { get; set; } = UploadType.Document;
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

    }
}
