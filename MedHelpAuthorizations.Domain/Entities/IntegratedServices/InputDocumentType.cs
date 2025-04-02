using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class InputDocumentType : AuditableEntity<InputDocumentTypeEnum>
    {
        public InputDocumentType() { }
        public InputDocumentType(InputDocumentTypeEnum id, string code, string description)
        {
            Id = id; 
            Code = code; 
            Description = description;
        }

        [StringLength(16)]
        public string Code { get; set; }

        [StringLength(48)]
        public string Description { get; set; }
    }
}
