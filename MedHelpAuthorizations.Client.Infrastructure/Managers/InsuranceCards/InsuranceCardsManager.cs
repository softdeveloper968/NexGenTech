using MedHelpAuthorizations.Application.Features.InsuranceCards.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetByCardholderId;
using MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetById;
using MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetByPatientId;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetByCriteria;
using System;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.InsuranceCards
{
    public class InsuranceCardsManager : IInsuranceCardsManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public InsuranceCardsManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<PaginatedResult<GetAllPagedInsuranceCardsResponse>> GetAllPagedAsync(GetAllPagedInsuranceCardsQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.InsuranceCardEndPoints.GetAll());
            return await response.ToPaginatedResult<GetAllPagedInsuranceCardsResponse>();
        }

        public async Task<IResult<GetInsuranceCardByIdResponse>> GetByIdAsync(int id)
        {
            
            var response = await _tenantHttpClient.GetAsync(Routes.InsuranceCardEndPoints.GetById(id));
            var result = await response.ToResult<GetInsuranceCardByIdResponse>();
            return result;
        }

        public async Task<PaginatedResult<GetInsuranceCardsByCriteriaResponse>> GetByCriteriaAsync(GetInsuranceCardsByCriteriaPagedQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.InsuranceCardEndPoints.GetByCriteria(request));
            return await response.ToPaginatedResult<GetInsuranceCardsByCriteriaResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditInsuranceCardCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.InsuranceCardEndPoints.Save(), request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetInsuranceCardsByPatientIdResponse>>> GetInsuranceCardsByPatientIdAsync(int patientId)
        {
            try
            {
                var endpoint = Routes.InsuranceCardEndPoints.GetByPatientId(patientId);
                var response = await _tenantHttpClient.GetAsync(endpoint);
                var result = await response.ToResult<List<GetInsuranceCardsByPatientIdResponse>>();
                return result;
            }
            catch (Exception ex)
            {
                throw; 
            }          
        }

        public async Task<IResult<List<GetInsuranceCardsByCardholderIdResponse>>> GetInsuranceCardsByCardholderIdAsync(int cardholderId)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.InsuranceCardEndPoints.GetById(cardholderId));
            return await response.ToResult<List<GetInsuranceCardsByCardholderIdResponse>>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{InsuranceCardEndPoints.Delete}/{id}");
            return await response.ToResult<int>();
        }
    }
}
