using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetById;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Commands.Upsert;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Queries.GetByBatchId;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Queries.GetById;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Queries.GetAll;
using MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.ChargeEntry
{
    public class ChargeEntryTransactionManager : IChargeEntryTransactionManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;
        
        public ChargeEntryTransactionManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<int>> UpsertAsync(UpsertChargeEntryTransactionCommand command)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ChargeEntryTransactionsEndpoint.Save, command);
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllChargeEntryTransactionsResponse>>> GetAllAsync()
        {
            var response = await _tenantHttpClient.GetAsync(ChargeEntryTransactionsEndpoint.GetAll());
            return await response.ToResult<List<GetAllChargeEntryTransactionsResponse>>();
        }

        public async Task<IResult<GetChargeEntryTransactionByIdResponse>> GetByIdAsync(int id)
        {
            var response = await _tenantHttpClient.GetAsync($"{ChargeEntryTransactionsEndpoint.GetById(id)}");
            return await response.ToResult<GetChargeEntryTransactionByIdResponse>();
        }

        public async Task<IResult<List<GetChargeEntryTransactionsByBatchIdResponse>>> GetByChargeEntryBatchId(int chargeEntrysBatchId)
        {
            var response = await _tenantHttpClient.GetAsync($"{ChargeEntryTransactionsEndpoint.GetByChargeEntryBatchId(chargeEntrysBatchId)}");
            return await response.ToResult<List<GetChargeEntryTransactionsByBatchIdResponse>>();
        }
    }    
}
