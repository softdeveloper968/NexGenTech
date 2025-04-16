
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;

public class ClaimStatusDashboardDetailsQueryBase : ClaimStatusDashboardQueryBase, IClaimStatusDashboardDetailsQuery
{
    public string FlattenedLineItemStatus { get; set; }
    public int ClaimStatusBatchId { get; set; }
    public int ClientLocationId { get; set; }
    public int ClientProviderId { get; set; }
    public string ProviderName { get; set; }
    public string LocationName { get; set; }
    public int? PatientId { get; set; }
    public string DashboardType { get; set; } = string.Empty;
}
