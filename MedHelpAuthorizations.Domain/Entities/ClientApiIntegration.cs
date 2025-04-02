using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
	public class ClientApiIntegration : AuditableEntity<int>
	{
		public ClientApiIntegration(int clientId, ApiIntegrationEnum apiIntegrationId, bool isActive = false)
		{

		}

		public bool IsActive { get; set; } = false;

		public int ClientId { get; set; }

		public ApiIntegrationEnum ApiIntegrationId { get; set; }


		[ForeignKey(nameof(ApiIntegrationId))]
		public virtual ApiIntegration ApiIntegration { get; set; }


		[ForeignKey(nameof(ClientId))]
		public virtual Client Client { get; set; }
	}
}

