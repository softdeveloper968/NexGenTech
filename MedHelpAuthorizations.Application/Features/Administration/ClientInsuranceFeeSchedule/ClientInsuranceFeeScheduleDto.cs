
using MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Base;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsuranceFeeSchedule
{
	public class ClientInsuranceFeeScheduleDto
	{
		public int Id { get; set; }

		public int ClientFeeScheduleId { get; set; }

		public int ClientInsuranceId { get; set; }

		public bool IsActive { get; set; }

		public ClientInsuranceDto ClientInsurance { get; set;}

		public ClientFeeScheduleDto ClientFeeSchedule { get; set; }
	}
}
