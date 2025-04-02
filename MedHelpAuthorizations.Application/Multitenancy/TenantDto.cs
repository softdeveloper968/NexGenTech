namespace MedHelpAuthorizations.Application.Multitenancy;

public class TenantDto
{
    public int Id { get; set; } = default!;
    public string Identifier { get; set; } 
    public string TenantName { get; set; } = default!;
    public string? ConnectionString { get; set; }
    public string AdminEmail { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime ValidUpto { get; set; }
    public string? Issuer { get; set; }
    public bool IsProductionTenant { get; set; }
}