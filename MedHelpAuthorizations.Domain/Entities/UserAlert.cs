using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class UserAlert : AuditableEntity<int>//, ITenant
    {      
        public string UserId { get; set; }

        public AlertTypeEnum AlertType { get; set; }

        public string PreviewText { get; set; }
        public string ResourceType { get; set; }

        public string ResourceId { get; set; }

        public bool IsViewed { get; set; }

        public bool IsRemoved { get; set; }

        //public string TenantId { get; set; }
    }
}
