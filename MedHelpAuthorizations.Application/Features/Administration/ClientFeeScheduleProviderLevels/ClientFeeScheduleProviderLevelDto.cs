
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientFeeScheduleProviderLevels
{
	public class ClientFeeScheduleProviderLevelDto
	{
		public int Id { get; set; }

		public int ClientFeeScheduleId { get; set; }

		public ProviderLevelEnum ProviderLevelId { get; set; }
	}
}
