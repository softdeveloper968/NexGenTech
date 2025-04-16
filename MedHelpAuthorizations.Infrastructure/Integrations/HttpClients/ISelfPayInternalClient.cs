using System.Net.Http;

namespace MedHelpAuthorizations.Infrastructure.Integrations.HttpClients
{
    public interface ISelfPayInternalClient
    {
        HttpClient Client { get; }

    }
}
