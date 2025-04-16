using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class ClaimStatusWorkstationNotes : AuditableEntity<int>//, ITenant
    {
        public int? ClaimStatusTransactionId { get; set; }
        public int? ClientId { get; set; }
        public string NoteContent { get; set; }
        public DateTime? NoteTs { get; set; }
       // public string TenantId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("ClaimStatusTransactionId")]
        public virtual ClaimStatusTransaction ClaimStatusTransaction { get; set; }
    }
}
