using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class WriteOffType : AuditableEntity<WriteOffTypeEnum> //AA-231
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
