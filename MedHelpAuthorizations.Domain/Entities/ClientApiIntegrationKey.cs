using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientApiIntegrationKey : AuditableEntity<int>//, ITenant
    {
        public int ClientId { get; set; }

        public bool IsActive { get; set; } = true;
        public ApiIntegrationEnum ApiIntegrationId { get; set; }

        [StringLength(128)]
        public string ApiKey { get; set; }

        [StringLength(128)]
        public string ApiSecret { get; set; }

        public string ApiUrl { get; set; }

        [StringLength(36)]
        public string ApiVersion { get; set; }

        [StringLength(24)]
        public string ApiUsername { get; set; }

        [StringLength(24)]
        public string ApiPassword { get; set; }

        //public string TenantId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }


        [ForeignKey("ApiIntegrationId")]
        public virtual ApiIntegration ApiIntegration { get; set; }
    }
}
