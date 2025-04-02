namespace MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.Base
{
	public class FeeScheduleCriteriaModel
	{
		public int ClientFeeScheduleId { get; set; }
		public string ClientInsuranceIds { get; set; }
		public DateTime? ClientFeeScheduleStartDate { get; set; }
		public DateTime? ClientFeeScheduleEndDate { get; set; }
		public string SpecialtyIds { get; set; }
		public string ProviderLevelIds { get; set; }
	}
}
