using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Net.Http;
using System.Threading.Tasks;
using MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices;
using MedHelpAuthorizations.Application.Features.IntegratedServices.RpaInsurances.Queries.GetAll;
using System.Collections.Generic;
using MediatR;
using System.Net.Http.Json;
using MedHelpAuthorizations.Application.Features.IntegratedServices.RpaInsurances.Commands.Update;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Commands.Update;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.RpaInsurances
{
    public class RpaInsurancesManager : IRpaInsurancesManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public RpaInsurancesManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<List<GetAllRpaInsurancesResponse>>> GetRpaInsurancesAsync(GetAllRpaInsurancesQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(RpaInsurancesEndpoints.GetAll(request.SearchString));
            return await response.ToResult<List<GetAllRpaInsurancesResponse>>();            
        }

        public Task<PaginatedResult<GetAllRpaInsurancesResponse>> GetRpaInsurancesPagedAsync(GetAllRpaInsurancesQuery request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IResult<int>> UpdateInactivatedOn(UpdateRpaInsuranceInactivatedOnCommand command)
        {
            var response = await _tenantHttpClient.PutAsJsonAsync(RpaInsurancesEndpoints.Save, command);
            return await response.ToResult<int>();
        }

        //public async Task<IResult<int>> SaveAsync(AddEditRpaInsuranceCommand request)
        //{
        //    var response = await _tenantHttpClient.PostAsJsonAsync(RpaInsurancesEndpoints.Save, request);
        //    return await response.ToResult<int>();
        //}

        //public async Task<IResult<GetRpaInsuranceByIdResponse>> GetRpaInsuranceByIdAsync(GetRpaInsuranceByIdQuery request)
        //{
        //    var response = await _tenantHttpClient.GetAsync(RpaInsurancesEndpoints.GetById(request.Id));
        //    retur

        //public async Task<IResult<int>> DeleteAsync(int id)
        //{
        //    var response = await _tenantHttpClient.DeleteAsync($"{RpaInsurancesEndpoints.Delete}/{id}");
        //    return await response.ToResult<int>();
        //}

        //public async Task<string> ExportToExcelAsync()
        //{
        //    var response = await _tenantHttpClient.GetAsync(RpaInsurancesEndpoints.Export);
        //    var data = await response.Content.ReadAsStringAsync();
        //    return data;
        //}
    }
}