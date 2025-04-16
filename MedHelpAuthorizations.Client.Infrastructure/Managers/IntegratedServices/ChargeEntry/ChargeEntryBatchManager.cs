using MedHelpAuthorizations.Shared.Wrapper;
using System.Net.Http;
using System.Threading.Tasks;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using System.Net.Http.Json;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetAll;
using MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetById;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Commands.Create;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Commands.Update;
using System.Collections.Generic;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetRecent;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.ChargeEntry
{
    public class ChargeEntryBatchManager : IChargeEntryBatchManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ChargeEntryBatchManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }
        public async Task<IResult<List<GetAllChargeEntryBatchesResponse>>> GetAllAsync()
        {
            var response = await _tenantHttpClient.GetAsync(ChargeEntryBatchesEndpoint.GetAll());
            return await response.ToResult<List<GetAllChargeEntryBatchesResponse>>();
        }

        public async Task<IResult<GetChargeEntryBatchByIdResponse>> GetByIdAsync(int id)
        {
            var response = await _tenantHttpClient.GetAsync($"{ChargeEntryBatchesEndpoint.GetById(id)}");
            return await response.ToResult<GetChargeEntryBatchByIdResponse>();
        }

        public async Task<IResult<int>> CreateAsync(CreateChargeEntryBatchCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ChargeEntryBatchesEndpoint.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> UpdateCompletedAsync(int batchId)
        {
            var response = await _tenantHttpClient.PutAsJsonAsync(ChargeEntryBatchesEndpoint.UpdateCompleted(), new UpdateChargeEntryBatchCommand() { Id = batchId, IsCompleted = true });
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> UpdateAbortedAsync(int batchId)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ChargeEntryBatchesEndpoint.UpdateAborted(), new UpdateChargeEntryBatchCommand() { Id = batchId, IsAborted = true });
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> UpdateProcessStartDateTimeAsync(UpdateChargeEntryBatchCommand request)
        {
            var response = await _tenantHttpClient.PutAsJsonAsync(ChargeEntryBatchesEndpoint.UpdateProcessStartDateTime(), request);
            return await response.ToResult<int>();
        }
        public async Task<IResult<List<GetChargeEntryUnprocessedBatchesResponse>>> GetAllUnprocessedAsync()
        {
            var response = await _tenantHttpClient.GetAsync(ChargeEntryBatchesEndpoint.GetAllUnprocessed());
            return await response.ToResult<List<GetChargeEntryUnprocessedBatchesResponse>>();
        }
        public async Task<IResult<List<GetRecentChargeEntryBatchesByClientIdResponse>>> GetRecentForClientIdAsync()
        {
            var response = await _tenantHttpClient.GetAsync(ChargeEntryBatchesEndpoint.GetRecentForClientId());
            return await response.ToResult<List<GetRecentChargeEntryBatchesByClientIdResponse>>();
        }
    }
}
