using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.CustomAttributes;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Models;

[ImportOrderAttribute(2)]
public partial class TblLocation : DfStagingAuditableEntity
{
    public int? LocationId { get; set; }

    public string? LocationName { get; set; }

    public string? AddressStreetLine1 { get; set; }

    public string? AddressStreetLine2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string? TenantClientString { get; set; }
}
