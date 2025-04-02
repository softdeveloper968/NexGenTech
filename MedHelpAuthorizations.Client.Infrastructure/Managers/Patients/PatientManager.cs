using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Cardholders.Queries.GetBySearchString;
using MedHelpAuthorizations.Application.Features.Patients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Patients.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Patients.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Patients.Queries.GetPatientsByCriteria;
using MedHelpAuthorizations.Application.Requests.Patients;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Patient
{
    public class PatientManager : IPatientManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public PatientManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{Routes.PatientsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<string> ExportToExcelAsync()
        {
            var response = await _tenantHttpClient.GetAsync(Routes.PatientsEndpoints.Export);
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<IResult<string>> GetPatientImageAsync(int id)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.PatientsEndpoints.GetPatientImage(id));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedPatientsResponse>> GetPatientsAsync(GetAllPagedPatientsRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.PatientsEndpoints.GetAllPaged(request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<GetAllPagedPatientsResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditPatientCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.PatientsEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<PaginatedResult<GetPatientsByCriteriaResponse>> GetByCriteriaAsync(GetPatientsByCriteriaQuery query)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.PatientsEndpoints.GetByCriteriaPaged(query));
            return await response.ToPaginatedResult<GetPatientsByCriteriaResponse>();
        }

        public async Task<IResult<GetPatientByIdResponse>> GetPatientByIdAsync(int id)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.PatientsEndpoints.GetById(id));
            return await response.ToResult<GetPatientByIdResponse>();
        }

        public async Task<IResult<List<GetPatientsBySearchStringResponse>>> GetBySearchStringAsync(string searchString)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.PatientsEndpoints.GetBySearchString(searchString));
            return await response.ToResult<List<GetPatientsBySearchStringResponse>>();
        }

    }
}