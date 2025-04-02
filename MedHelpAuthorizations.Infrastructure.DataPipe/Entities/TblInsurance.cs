using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.CustomAttributes;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Models;

[ImportOrderAttribute(3)]
public partial class TblInsurance : DfStagingAuditableEntity
{
    public int? InsuranceId { get; set; }

    public string? Name { get; set; }

    public string? AddressStreetLine1 { get; set; }

    public string? AddressStreetLine2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string? Phone { get; set; }

    public string? Active { get; set; }
}
