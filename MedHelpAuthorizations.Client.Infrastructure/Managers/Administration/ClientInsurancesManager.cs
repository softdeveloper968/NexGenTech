using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetRpaAssignedInsurances;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetById;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllByClientId;
using MediatR;
using MedHelpAuthorizations.Application.Responses.Audit;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using System;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class ClientInsurancesManager : IClientInsurancesManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ClientInsurancesManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{ClientInsurancesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<string> ExportToExcelAsync()
        {
            var response = await _tenantHttpClient.GetAsync(ClientInsurancesEndpoints.Export);
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }
        
        public async Task<IResult<List<GetAllClientInsurancesByClientIdResponse>>> GetAllClientInsurancesAsync()
        {
            var response = await _tenantHttpClient.GetAsync(ClientInsurancesEndpoints.GetAll());
            return await response.ToResult<List<GetAllClientInsurancesByClientIdResponse>>();
        }

        public async Task<PaginatedResult<GetAllPagedInsurancesResponse>> GetAllClientInsurancesPagedAsync(GetAllPagedInsurancesRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(ClientInsurancesEndpoints.GetAllPaged(request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<GetAllPagedInsurancesResponse>();
        }

        public async Task<IResult<List<GetRpaAssignedInsurancesResponse>>> GetRpaAssignedInsurancesAsync(GetRpaAssignedInsurancesQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(ClientInsurancesEndpoints.GetRpaAssigned);
            return await response.ToResult<List<GetRpaAssignedInsurancesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditInsuranceCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientInsurancesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<GetInsuranceByIdResponse>> GetInsuranceByIdAsync(GetInsuranceByIdQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(ClientInsurancesEndpoints.GetById(request.Id));
            return await response.ToResult<GetInsuranceByIdResponse>();
        }
        
        public async Task<IResult<List<GetClientInsurancesBySearchStringResponse>>> GetBySearchStringAsync(string searchString)
        {
            var response = await _tenantHttpClient.GetAsync(ClientInsurancesEndpoints.GetBySearchString(searchString));
            return await response.ToResult<List<GetClientInsurancesBySearchStringResponse>>();
        }

        /// <summary>
        /// Get location data by client Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IResult<List<GetAllClientInsurancesByClientIdResponse>>> GetInsuranceByClientIdAsync(GetAllClientInsuranceByClientIdQuery request)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(ClientInsurancesEndpoints.GetByClientId(request.ClientId));
                return await response.ToResult<List<GetAllClientInsurancesByClientIdResponse>>();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IResult<List<PayerProviderTotals>>> GetProviderTotalsByPayerAsync(GetProviderTotalsByPayerQuery request)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(ClientInsurancesEndpoints.GetProviderTotalsByPayer, request);
                return await response.ToResult<List<PayerProviderTotals>>();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<IResult<List<ClaimSummary>>> GetPaymentTotalsByPayerAsync(GetPaymentsByInsuranceQuery request) //EN-278
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(ClientInsurancesEndpoints.GetPaymentTotalsByPayer, request);
                return await response.ToResult<List<ClaimSummary>>();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<IResult<List<ClaimSummary>>> GetDenialTotalsByPayerAsync(GetDenialsByInsuranceQuery request)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(ClientInsurancesEndpoints.GetDenialTotalsByPayer, request);
                return await response.ToResult<List<ClaimSummary>>();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IResult<List<PayerTotalsByProvider>>> GetPayerTotalsAsync(GetPayerTotalsQuery request)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(ClientInsurancesEndpoints.GetPayerTotal, request);
                return await response.ToResult<List<PayerTotalsByProvider>>();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}

