using MedHelpAuthorizations.Application.Features.ResponsibleParties.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.ResponsibleParties.GetByCriteria;
using MedHelpAuthorizations.Application.Features.ResponsibleParties.GetById;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.ResponsibleParty
{
    public class ResponsiblePartyManager : IResponsiblePartyManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ResponsiblePartyManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }


        public async Task<IResult<GetResponsiblePartyByIdResponse>> GetByIdAsync(int id)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ResponsiblePartyEndPoints.GetById(id));
            return await response.ToResult<GetResponsiblePartyByIdResponse>();
        }

        public async Task<PaginatedResult<GetByCritieriaResponsiblePartyResponse>> GetByAccountNumerAsync(string accNumber)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ResponsiblePartyEndPoints.AccNumber(accNumber));
            return await response.ToPaginatedResult<GetByCritieriaResponsiblePartyResponse>();
        }

        public async Task<PaginatedResult<GetByCritieriaResponsiblePartyResponse>> GetByExternalIdAsync(string externalId)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ResponsiblePartyEndPoints.ExternalId(externalId));
            return await response.ToPaginatedResult<GetByCritieriaResponsiblePartyResponse>();
        }
        public async Task<PaginatedResult<GetByCritieriaResponsiblePartyResponse>> GetByCriteriaAsync(GetByCritieriaResponsiblePartyQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ResponsiblePartyEndPoints.GetByCriteria(request));
            return await response.ToPaginatedResult<GetByCritieriaResponsiblePartyResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditResponsiblePartyCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ResponsiblePartyEndPoints.Save(), request);
            return await response.ToResult<int>();
        }
    }
}
