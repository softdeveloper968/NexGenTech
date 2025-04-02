using MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Queries.GetClientLocationServiceTypes;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocationSpeciality.Queries.GetClientLocationSpecilalityQuery.cs;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class ClientLocationSpecialityManager : IClientLocationSpecialityManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ClientLocationSpecialityManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }
        public async Task<IResult<List<GetClientLocationSpecilalityResponse>>> GetClientLocationSpeciality(int locationId)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ClientLocationSpecialityEndPoint.GetClientLocationSpeciality(locationId));
            return await response.ToResult<List<GetClientLocationSpecilalityResponse>>();
        }
    }
}
