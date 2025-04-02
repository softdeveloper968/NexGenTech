
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Features.Providers.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Providers.GetByCriteria;
using MedHelpAuthorizations.Application.Features.Providers.Queries.GetAllProviders;
using MedHelpAuthorizations.Application.Features.Providers.Queries.GetProviderById;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Provider
{
    public class ProviderManager : IProviderManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ProviderManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<List<GetAllProvidersResponse>>> GetAllClientProvidersAsync()
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ProviderEndPoints.GetAllClientProviders);
            return await response.ToResult<List<GetAllProvidersResponse>>();
        }

        public async Task<PaginatedResult<GetAllProvidersResponse>> GetAllPagedAsync(GetAllPagedClientProvidersRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ProviderEndPoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString));
            return await response.ToPaginatedResult<GetAllProvidersResponse>();
        }

        public async Task<IResult<GetProviderByIdResponse>> GetByIdAsync(int id)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ProviderEndPoints.GetById(id));
            return await response.ToResult<GetProviderByIdResponse>();
        }

        public async Task<PaginatedResult<GetProvidersByCriteriaResponse>> GetByCriteriaAsync(GetProvidersByCriteriaQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ProviderEndPoints.GetByCriteria(request));
            return await response.ToPaginatedResult<GetProvidersByCriteriaResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditProviderCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ProviderEndPoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{Routes.ProviderEndPoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<AverageDaysByProvider>>> GetAverageDaysByProviderAsync(GetAverageDaysToPayByProviderQuery criteria) //EN-190
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ProviderEndPoints.GetAverageDaysByProviders, criteria);
                var data = await response.ToResult<List<AverageDaysByProvider>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ChargesTotalsByProvider>>> GetChargesByProviderAsync(ChargesByProviderQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ProviderEndPoints.ChargesByProvider, criteria);
                var data = await response.ToResult<List<ChargesTotalsByProvider>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        public async Task<IResult<List<ProcedureTotalsByProvider>>> GetProceduresByProviderAsync(GetProceduresByProviderQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ProviderEndPoints.ProceduresByProvider, criteria);
                var data = await response.ToResult<List<ProcedureTotalsByProvider>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        public async Task<IResult<List<ClaimSummary>>> GetInsurancesByProviderQueryAsync(GetInsurancesByProviderQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ProviderEndPoints.InsurancesByProvider, criteria);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        public async Task<IResult<List<DenialReasonsTotalsByProvider>>> GetDenialReasonsByProviderQueryAsync(GetDenialReasonsByProviderQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ProviderEndPoints.DenialReasonsByProvider, criteria);
                var data = await response.ToResult<List<DenialReasonsTotalsByProvider>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        public async Task<IResult<List<ClaimSummary>>> GetProcedureReimbursementByProviderQueryAsync(GetProcedureReimbursementByProviderQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ProviderEndPoints.GetProcedureReimbursementByProvider, criteria);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<IResult<List<ClaimSummary>>> GetPayerReimbursementByProviderQueryAsync(GetPayerReimbursementByProviderQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ProviderEndPoints.GetPayerReimbursementByProvider, criteria);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ProviderProcedureTotal>>> GetProviderProcedureTotalQueryAsync(GetProviderProcedureTotalQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ProviderEndPoints.GetProviderProcedureTotal, query);
                var data = await response.ToResult<List<ProviderProcedureTotal>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<List<ProviderDenialReasonTotal>>> GetDenialReasonTotalsByProviderIdQueryAsync(GetDenialReasonTotalsByProviderIdQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ProviderEndPoints.GetDenialReasonTotalsByProviderId, query);
                var data = await response.ToResult<List<ProviderDenialReasonTotal>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
