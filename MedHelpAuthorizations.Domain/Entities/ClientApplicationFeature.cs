using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Shared.Enums;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientApplicationFeature : AuditableEntity<int>//, ITenant
    {
        public ApplicationFeatureEnum ApplicationFeatureId { get; set; }
        public int ClientId { get; set; }
       // public string TenantId { get; set; }

        [ForeignKey("ApplicationFeatureId")]
        public virtual ApplicationFeature ApplicationFeature { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
    }
}
