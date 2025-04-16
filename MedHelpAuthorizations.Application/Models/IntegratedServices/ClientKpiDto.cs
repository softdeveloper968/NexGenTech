
namespace MedHelpAuthorizations.Application.Models.IntegratedServices
{
    public class ClientKpiDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int DailyClaimCount { get; set; } = 0;
        public decimal MonthlyCashCollection { get; set; } = default(decimal);
        public int? VolumeCredentialDenials { get; set; } = default(int?);
        public decimal? CredentialDenialsCashValue { get; set; } = default(decimal?);
        public decimal? ClaimDenialPercentage { get; set; } = default(decimal?);
        public decimal? DemographicDenialPercentage { get; set; } = default(int?);
        public decimal? CodingDenialPercentage { get; set; } = default(int?);
        public int? AverageSubmitDays { get; set; } = default(int?);
        public int? AverageDaysInReceivables { get; set; } = default(int?);
        public decimal? AR90DaysInsurancePercentage { get; set; } = default(decimal?);
        public decimal? AR90DaysSelfPayPercentage { get; set; } = default(decimal?);
        public decimal? AR180DaysInsurancePercentage { get; set; } = default(decimal?);
        public decimal? AR180DaysSelfPayPercentage { get; set; } = default(decimal?);
        public decimal? CleanClaimRate { get; set; } = default(decimal?);
        public int RemainingDailyClaimCount { get; set; }
        public decimal AveragePerClaimAmount => DailyClaimCount != 0 ? (MonthlyCashCollection / (DailyClaimCount * 22)) : 0.0m;

        #region Chart KPIs
        public decimal? CodingAccuracy { get; set; }
        public decimal? DocumentationAccuracy { get; set; }
        public decimal? ChartCompletionTiming { get; set; }
        public decimal? OrganizationalPassRate { get; set; }
        public decimal? ComplianceAccuracy { get; set; }
        #endregion

        #region Provider KPIs
        public decimal? ScheduledAppointments { get; set; }
        public decimal? DailyCompletedVisits { get; set; }
        public decimal? NoShow { get; set; }
        public decimal? OpenCharts { get; set; }
		#endregion

		#region Additional Billilng Kpis
		public int? Visits { get; set; }
        public decimal? Charges { get; set; }
        public decimal? DenialRate { get; set; }
		public decimal? CollectionPercentage { get; set; }
		public decimal? CashCollections { get; set; }
		public decimal? Over90Days { get; set; }
		public decimal? DaysInAR { get; set; }
		public decimal? BDRate { get; set; }
		#endregion
	}
}
