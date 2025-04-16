namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base
{
    public interface IClaimStatusDashboardDetailsQuery : IClaimStatusDashboardStandardQuery
    {
        public string FlattenedLineItemStatus { get; set; }
        public int? PatientId { get; set; }
    }
}
