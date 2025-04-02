using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class TransactionType : AuditableEntity<TransactionTypeEnum>
    {
        public TransactionType() { }

        public TransactionType(TransactionTypeEnum id, string code, string description) 
        {
            Id = id; 
            Code = code; 
            Description = description;
        }

        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(80)]
        public string Description { get; set; }
    }
}
