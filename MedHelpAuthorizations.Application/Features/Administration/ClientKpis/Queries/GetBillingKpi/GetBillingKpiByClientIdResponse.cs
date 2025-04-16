namespace MedHelpAuthorizations.Application.Features.Administration.ClientKpis.Queries.GetBillingKpi
{
    public class GetBillingKpiByClientIdResponse
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        //public int? DailyClaimCountGoal { get; set; }
        public decimal? CleanClaimRateGoal { get; set; }
        public decimal? DenialRateGoal { get; set; }
        public decimal? ChargesGoal { get; set; }
        public decimal? CollectionPercentageGoal { get; set; }
        public decimal? CashCollectionsGoal { get; set; }
        public decimal? Over90DaysGoal { get; set; }
        //public decimal? AverageDaysInReceivablesGoal { get; set; }
        public decimal? BDRateGoal { get; set; }
        //public int? DailyClaimCountValue { get; set; }
        public decimal? CleanClaimRateValue { get; set; }
        public decimal? DenialRateValue { get; set; }
        public decimal? ChargesValue { get; set; }
        public decimal? CollectionPercentageValue { get; set; }
        public decimal? CashCollectionsValue { get; set; }
        public decimal? Over90DaysValue { get; set; }
        //public decimal? AverageDaysInReceivablesValue { get; set; }
        public decimal? BDRateValue { get; set; }

        #region Additional Billilng Kpis

        public decimal? VisitsGoal { get; set; }
		public int? VisitsValue { get; set; }
		public decimal? DaysInARGoal { get; set; }
		public decimal? DaysInARValue { get; set; }
		#endregion
	}
}
