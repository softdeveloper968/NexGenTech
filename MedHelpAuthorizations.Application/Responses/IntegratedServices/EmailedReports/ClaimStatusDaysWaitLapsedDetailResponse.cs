using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;

namespace MedHelpAuthorizations.Application.Responses.IntegratedServices.EmailedReports
{
    public class ClaimStatusDaysWaitLapsedDetailResponse : ClaimStatusDashboardInProcessDetailsResponseBase
    {
        public int? ClaimStatusTransactionId { get; set; }
        public int? ClaimStatusTransactionLineItemStatusChangẹId { get; set; }
        public string ClaimLineItemStatus { get; set; }
        public string StatusLastCheckedOn { get; set; }
        public string DaysLapsed { get; set; }
    }
}
