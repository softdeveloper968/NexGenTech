using MedHelpAuthorizations.Application.Features.Administration.ClientFeeScheduleProviderLevels;
using MedHelpAuthorizations.Application.Features.Administration.ClientFeeScheduleSpecialties;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsuranceFeeSchedule;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.Base
{
	public class ClientFeeScheduleBase
	{
		public int Id { get; set; }

		public int? ClientId { get; set; }

		public string Name { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public List<ClientFeeScheduleProviderLevelDto> ClientFeeScheduleProviderLevels { get; set; } = new List<ClientFeeScheduleProviderLevelDto>();

		public List<ClientFeeScheduleSpecialtyDto> ClientFeeScheduleSpecialties { get; set; } = new List<ClientFeeScheduleSpecialtyDto>();

		//public List<ClientInsuranceDto> ClientInsurances { get; set; } = new List<ClientInsuranceDto>();

		public List<ClientInsuranceFeeScheduleDto> ClientInsuranceFeeSchedules { get; set; } = new List<ClientInsuranceFeeScheduleDto>();

		public ImportStatusEnum ImportStatus { get; set; }
	}

	//public class ClientInsuranceName
	//{
	//	public int ClientInsuranceId { get; set; }

	//	public string InsuranceName { get; set; }
	//}
}

