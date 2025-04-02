namespace MedHelpAuthorizations.Application.Features.Administration.ClientKpis.Queries.GetClientKpiByClientId
{
	public class GetClientKpiByClientIdQueryResponse
	{
		public int Id { get; set; }
		public int ClientId { get; set; }
		public int? DailyClaimCount { get; set; }
		public decimal? MonthlyCashCollection { get; set; }
		public int? VolumeCredentialDenials { get; set; }
		public decimal? CredentialDenialsCashValue { get; set; }
		public decimal? ClaimDenialPercentage { get; set; }
		public int? DemographicDenialPercentage { get; set; }
		public int? CodingDenialPercentage { get; set; }
		public int? AverageSubmitDays { get; set; }
		public int? AverageDaysInReceivables { get; set; }
		public decimal? AR90DaysInsurancePercentage { get; set; }
		public decimal? AR90DaysSelfPayPercentage { get; set; }
		public decimal? AR180DaysInsurancePercentage { get; set; }
		public decimal? AR180DaysSelfPayPercentage { get; set; }
		public decimal? CleanClaimRate { get; set; }
	}
}
