using MedHelpAuthorizations.Application.Features.Authorizations.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetByCriteriaPaged;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetByPatientId;
using MedHelpAuthorizations.Application.Features.Reports.Queries.GetExpiringAuthorizations;
using MedHelpAuthorizations.Application.Requests.Authorizations;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Authorizations
{
    public class AuthorizationManager : IAuthorizationManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public AuthorizationManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{Routes.AuthorizationsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<string> ExportToExcelAsync()
        {
            var response = await _tenantHttpClient.GetAsync(Routes.AuthorizationsEndpoints.Export);
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }
        //public async Task<string> ExportExpiringAuthorizationsToExcelAsync()
        //{
        //    var response = await _tenantHttpClient.GetAsync(Routes.ReportEndpoints.ExportExpiringAuthorizations);
        //    var data = await response.Content.ReadAsStringAsync();
        //    return data;
        //}

        public async Task<PaginatedResult<GetAllPagedAuthorizationsResponse>> GetAuthorizationsAsync(GetAllPagedAuthorizationsRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.AuthorizationsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString));
            return await response.ToPaginatedResult<GetAllPagedAuthorizationsResponse>();
        }

        public async Task<PaginatedResult<GetByCriteriaPagedAuthorizationsResponse>> GetAuthorizationsByCriteriaAsync(GetByCriteriaPagedAuthorizationsQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.AuthorizationsEndpoints.GetByCriteriaPaged(request));
            return await response.ToPaginatedResult<GetByCriteriaPagedAuthorizationsResponse>();
        }

        public async Task<IResult<GetAuthorizationByIdResponse>> GetAuthorizationByIdAsync(int id)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.AuthorizationsEndpoints.GetById(id));
            return await response.ToResult<GetAuthorizationByIdResponse>();
        }

        public async Task<PaginatedResult<GetAllPagedAuthorizationsResponse>> GetAuthorizationsByPatientIdAsync(GetAuthorizationByPatientIdQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.AuthorizationsEndpoints.GetByPatientIdPaged(request.PatientId, request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<GetAllPagedAuthorizationsResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditAuthorizationCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.AuthorizationsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
        //public async Task<PaginatedResult<GetAllPagedAuthorizationsResponse>> GetExpiringAuthorizationsAsync(GetPagedExpiringAuthorizationsQuery request)
        //{
        //    var response = await _tenantHttpClient.GetAsync(Routes.ReportEndpoints.GetExpiringAuthorizations(request));
        //    return await response.ToPaginatedResult<GetAllPagedAuthorizationsResponse>();
        //}
    }
}