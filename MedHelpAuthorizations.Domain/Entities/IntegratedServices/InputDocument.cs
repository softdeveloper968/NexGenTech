using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class InputDocument : AuditableEntity<int>, IDocument//, ITenant
    {
        public InputDocument(string title, InputDocumentTypeEnum inputDocumentTypeId)
        {
            Title = title;
            InputDocumentTypeId = inputDocumentTypeId;
            ClaimStatusBatches = new HashSet<ClaimStatusBatch>();

		}

        public InputDocument()
        {
			ClaimStatusBatches = new HashSet<ClaimStatusBatch>();
		}

        public int? ClientId { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; } = string.Empty;

        public int? ClientInsuranceId { get; set; } = null;

        [Required]
        public string URL { get; set; }
        
        public int ByteLength { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public InputDocumentTypeEnum InputDocumentTypeId { get; set; }

        [Required]
        public DateTime? DocumentDate { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDeleted { get; set; } = false;
		public ImportStatusEnum ImportStatus { get; set; }
        public int? AttemptedImportCount { get; set; }
        public int? ActualImportCount { get; set; }
        public int? AuthTypeId { get; set; } //EN-317
        public int? ClientLocationId { get; set; } //EN-457
        public string ErrorMessage { get; set; } //EN-652

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("ClientInsuranceId")]
        public virtual ClientInsurance ClientInsurance { get; set; }

        [ForeignKey("InputDocumentTypeId")]
        public virtual InputDocumentType InputDocumentType { get; set; }

        [ForeignKey("ClientLocationId")]
        public virtual ClientLocation ClientLocation { get; set; }

        public HashSet<ClaimStatusBatch> ClaimStatusBatches { get; set; }
		public virtual ICollection<ImportDocumentMessage> ImportDocumentMessages { get; set; }
	}
}
