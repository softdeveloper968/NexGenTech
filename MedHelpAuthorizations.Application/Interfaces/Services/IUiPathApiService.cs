using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Models.IntegratedServices.ChargeEntry;
using MedHelpAuthorizations.Shared.Responses.IntegratedServices.UiPath;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IUiPathApiService
    {
        Task<TokenResponse> Authenticate();

        Task<HttpResponseMessage> StartProcess(StartInfo startInfoObject);
    }
}
