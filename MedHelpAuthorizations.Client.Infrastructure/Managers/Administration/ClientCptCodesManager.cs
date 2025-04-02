using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
	public class ClientCptCodesManager : IClientCptCodesManager
	{
		private readonly ITenantHttpClient _tenantHttpClient;

		public ClientCptCodesManager(ITenantHttpClient tenantHttpClient)
		{
			_tenantHttpClient = tenantHttpClient;
		}

		public async Task<IResult<int>> DeleteAsync(int id)
		{
			var response = await _tenantHttpClient.DeleteAsync($"{ClientCptCodesEndpoints.Delete}/{id}");
			return await response.ToResult<int>();
		}

		public async Task<string> ExportToExcelAsync()
		{
			var response = await _tenantHttpClient.GetAsync(ClientCptCodesEndpoints.Export);
			var data = await response.Content.ReadAsStringAsync();
			return data;
		}

		public async Task<PaginatedResult<GetAllPagedClientCptCodesResponse>> GetClientCptCodesAsync(GetAllPagedClientCptCodesRequest request)
		{
			var response = await _tenantHttpClient.GetAsync(ClientCptCodesEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString));
			return await response.ToPaginatedResult<GetAllPagedClientCptCodesResponse>();
		}

		public async Task<IResult<int>> SaveAsync(AddEditClientCptCodeCommand request)
		{
			var response = await _tenantHttpClient.PostAsJsonAsync(ClientCptCodesEndpoints.Save, request);
			return await response.ToResult<int>();
		}

		public async Task<IResult<GetClientCptCodeByIdResponse>> GetClientCptCodeByIdAsync(GetClientCptCodeByIdQuery request)
		{
			var response = await _tenantHttpClient.GetAsync(ClientCptCodesEndpoints.GetById(request.Id));
			return await response.ToResult<GetClientCptCodeByIdResponse>();
		}

		public async Task<IResult<List<GetClientCptCodeByIdResponse>>> GetClientCptCodeByClientIdAsync()
		{
			var response = await _tenantHttpClient.GetAsync(ClientCptCodesEndpoints.GetCptByClientId());
			return await response.ToResult<List<GetClientCptCodeByIdResponse>>();
		}

		//EN-155
		public async Task<IResult<GetClientCptCodeByIdResponse>> GetClientCptCodeByCodeAsync(string code)
		{
			var response = await _tenantHttpClient.GetAsync(ClientCptCodesEndpoints.GetByCode(code));
			return await response.ToResult<GetClientCptCodeByIdResponse>();
		}

		public async Task<IResult<GetClientCptCodeByIdResponse>> CheckMatchCpt(int id)
		{
			var response = await _tenantHttpClient.GetAsync(ClientCptCodesEndpoints.CheckMatchCpt(id));
			return await response.ToResult<GetClientCptCodeByIdResponse>();
		}

		public async Task<IResult<List<GetClientCptCodeByIdResponse>>> GetCptCodeBySearch(string searchString)
		{
			var response = await _tenantHttpClient.GetAsync(ClientCptCodesEndpoints.CptCodeBySearch(searchString));
			return await response.ToResult<List<GetClientCptCodeByIdResponse>>();
		}

		//EN-334
        public async Task<IResult<List<ProviderTotalsByProcedure>>> GetProvidersByProcedureAsync(GetProviderByProcedureQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(ClientCptCodesEndpoints.ProvidersByProcedure, criteria);
                var data = await response.ToResult<List<ProviderTotalsByProcedure>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ClaimSummary>>> GetInsuranceByProcedureAsync(GetInsuranceByProcedureCodeQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(ClientCptCodesEndpoints.InsuranceByProcedure, criteria);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ClaimSummary>>> GetDenialReasonsByProcedureAsync(GetDenialReasonsByProcedureCodeQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(ClientCptCodesEndpoints.DenialReasonsByProcedure, criteria);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ClaimSummary>>> GetPayerReimbursementByProcedureAsync(GetPayerReimbursementByProcedureCodeQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(ClientCptCodesEndpoints.PayerReimbursementByProcedure, criteria);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ClaimSummary>>> GetProviderReimbursementByProcedureCodeAsync(GetProviderReimbursementByProcedureCodeQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(ClientCptCodesEndpoints.ProviderReimbursementByProcedureCode, criteria);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ClaimSummary>>> GetReimbursementByProcedureCode(ReimbursementByProcedureCodeQuery query)  //EN-334
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(ClientCptCodesEndpoints.ReimbursementByProcedureCode, query);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ChargesTotalsByProcedureCode>>> GetChargesByProcedureCodeQuery(ChargesByProcedureCodeQuery query)  //EN-334
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(ClientCptCodesEndpoints.ChargesByProcedureCode, query);
                var data = await response.ToResult<List<ChargesTotalsByProcedureCode>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<AverageDaysByProcedureCode>>> GetAverageDaysToPayByProcedureCodeQuery(GetAverageDaysToPayByProcedureCodeQuery query)  //EN-334
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(ClientCptCodesEndpoints.AverageDaysToPayByProcedureCodeQuery, query);
                var data = await response.ToResult<List<AverageDaysByProcedureCode>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ProviderTotals>>> GetProviderTotalsByProcedureCodeAsync(GetProviderTotalsByProcedureCodeQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(ClientCptCodesEndpoints.ProviderTotalsByProcedureCode, query);
                var data = await response.ToResult<List<ProviderTotals>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}