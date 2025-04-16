using System;
using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;

#nullable disable

namespace MedHelpAuthorizations.Domain.Entities
{
    public class Message : AuditableEntity<int>//, ITenant
    {
        public int MessageNo { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string Response { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public bool? Resolved { get; set; }
        public string MessageContent { get; set; }
        public int ClientId { get; set; }
        //public string TenantId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
    }
}
