using MedHelpAuthorizations.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientKpi : AuditableEntity<int>
    {
        public int ClientId { get; set; }

        public int DailyClaimCount { get; set; } = 0;

        public decimal MonthlyCashCollection { get; set; } = decimal.Zero;

        public int? VolumeCredentialDenials { get; set; }

        public decimal? CredentialDenialsCashValue { get; set; }

        public decimal? ClaimDenialPercentage { get; set; } 

        public decimal? DemographicDenialPercentage { get; set; }

        public decimal? CodingDenialPercentage { get; set; }

        public int? AverageSubmitDays { get; set; }

        public int? AverageDaysInReceivables { get; set; }

        public decimal? AR90DaysInsurancePercentage { get; set; }

        public decimal? AR90DaysSelfPayPercentage { get; set; }

        public decimal? AR180DaysInsurancePercentage { get; set; }

        public decimal? AR180DaysSelfPayPercentage { get; set; }

        public decimal? CleanClaimRate { get; set; }

		public int RemainingDailyClaimCount
		{
			get
			{
				if (Client != null && Client.EmployeeClients != null)
				{
					int assignedClaimCountSum = Client.EmployeeClients.Sum(x => x.AssignedAverageDailyClaimCount);
					return DailyClaimCount - assignedClaimCountSum;
				}
				return 0; // Return a suitable default value or handle the situation as needed.
			}
		}

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

		public decimal AveragePerClaimAmount => DailyClaimCount != 0 ? (MonthlyCashCollection / (DailyClaimCount * 22)) : 0.0m;


        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
    }
}
