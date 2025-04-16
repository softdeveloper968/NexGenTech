namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IApiKeyValidationService
    {
        bool IsValidApiKey(string apiKey);
    }
}
