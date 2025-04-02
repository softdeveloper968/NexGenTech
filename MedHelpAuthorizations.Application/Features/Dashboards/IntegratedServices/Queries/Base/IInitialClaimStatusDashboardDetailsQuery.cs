namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base
{
    public interface IInitialClaimStatusDashboardDetailsQuery : IClaimStatusDashboardInitialQuery
    {
        public string FlattenedLineItemStatus { get; set; }
        public int? PatientId { get; set; }
    }
}
