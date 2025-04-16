using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.CustomAttributes;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Models;

[ImportOrderAttribute(9)]
public partial class TblCardHolder : DfStagingAuditableEntity
{
    public int? CardHolderId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string? Employer { get; set; }

    public string? Gender { get; set; }

    public string? DateOfBirth { get; set; }

}
