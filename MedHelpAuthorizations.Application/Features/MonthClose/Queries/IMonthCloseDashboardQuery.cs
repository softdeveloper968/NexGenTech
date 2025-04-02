namespace MedHelpAuthorizations.Application.Features.MonthClose.Queries
{
    public interface IMonthCloseDashboardQuery
    {
        public int ClientId { get; set; }
        public string ClientLocationId { get; set; }
        public string ClientProviderId { get; set; }
        public string ClientInsuranceId { get; set; }
        public string CptCodeId { get; set; }
    }
}