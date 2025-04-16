namespace MedHelpAuthorizations.Application.Interfaces.Services.ExternalApis
{
	public interface IExternalApiJobService
	{
		Task<bool> ProcessApiClaimStatus();
		Task<bool> TestUhcServiceGetClaimSummaryByClaimNumber();
		Task<bool> TestUhcServiceGetClaimSummaryByMember();
		Task<bool> TestUhcServiceGetClaimDetailByClaimNumber();
		Task<bool> TestUhcServiceGetClaimDetailByMember();
	}
}