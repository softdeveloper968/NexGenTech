using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class Document : AuditableEntity<int>, IDocument//, ITenant
    {
        public string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = false;
        public string URL { get; set; }
        public int? PatientId { get; set; }
        public int? AuthorizationId { get; set; }
        public int DocumentTypeId { get; set; }
        public DateTime? DocumentDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        //public string TenantId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [ForeignKey("AuthorizationId")]
        public virtual Authorization Authorization { get; set; }

        [ForeignKey("DocumentTypeId")]
        public virtual DocumentType DocumentType { get; set; }

    }
}