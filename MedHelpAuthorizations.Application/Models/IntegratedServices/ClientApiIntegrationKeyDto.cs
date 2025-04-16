using MedHelpAuthorizations.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Models.IntegratedServices
{
    public class ClientApiIntegrationKeyDto
    {
        public int Id { get; set; }

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
    }
}
