using System.Net.Http;

namespace MedHelpAuthorizations.Client.Infrastructure.HttpClients
{
    public interface ISelfPayEligibilityHttpClient
    {
        HttpClient Client { get; }
    }
}
