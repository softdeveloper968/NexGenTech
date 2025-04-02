
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientFeeScheduleSpecialties
{
	public class ClientFeeScheduleSpecialtyDto
	{
		public int Id { get; set; }

		public int ClientFeeScheduleId { get; set; }

		public SpecialtyEnum SpecialtyId { get; set; }

		//public string SpecialtyName { get; set; }
	}
}
