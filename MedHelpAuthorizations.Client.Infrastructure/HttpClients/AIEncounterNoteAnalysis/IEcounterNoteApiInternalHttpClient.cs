using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.HttpClients.AIEncounterNoteAnalysis
{
    public interface IEcounterNoteApiInternalHttpClient
    {
        HttpClient Client { get; }
        public Task<HttpResponseMessage> GetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri);
        public Task<HttpResponseMessage> PostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content);
        public Task<HttpResponseMessage> PutAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content);
        public Task<HttpResponseMessage> PatchAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content);
        public Task<HttpResponseMessage> DeleteAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri);
        public Task<TValue?> GetFromJsonAsync<TValue>([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri);
        public Task<HttpResponseMessage> PostAsJsonAsync<TValue>([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, TValue value);
        public Task<HttpResponseMessage> PutAsJsonAsync<TValue>([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, TValue value);
    }
}
