using System.ComponentModel.DataAnnotations;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class AdjustmentType : AuditableEntity<AdjustmentTypeEnum>
    {
        public AdjustmentType() { }
        public AdjustmentType(AdjustmentTypeEnum id, string name) 
        {
            Id = id;
            Name = name;
        }

        [StringLength(6)]
        public string Name { get; set; }
    }
}
