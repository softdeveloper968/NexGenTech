using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Questionnaires.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Questionnaires.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Questionnaires.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Questionnaires.Queries.GetPatientQuestionnaire;
using MedHelpAuthorizations.Application.Requests.Questionnaires;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Questionnaire
{
    public class QuestionnaireManager : IQuestionnaireManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public QuestionnaireManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public Task<IResult<GetClientQuestionnaireByIdResponse>> GetClientQuestionnaireByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<GetPatientQuestionnaireByIdResponse>> GetPatientQuestionnaireByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginatedResult<GetAllPagedClientQuestionnairesResponse>> GetClientQuestionnairesAsync(GetAllPagedClientQuestionnairesRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(QuestionnairesEndpoints.GetAllPaged(request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<GetAllPagedClientQuestionnairesResponse>();
        }

        public async Task<PaginatedResult<GetAllPagedPatientQuestionnairesResponse>> GetPatientQuestionnairesAsync(GetAllPatientQuestionnairesQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(QuestionnairesEndpoints.GetAllPagedForPatient(request.PageNumber, request.PageSize, request.PatientId));
            return await response.ToPaginatedResult<GetAllPagedPatientQuestionnairesResponse>();
        }
        public async Task<IResult<int>> DeleteClientQuestionnaireAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{QuestionnairesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeletePatientQuestionnaireAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{QuestionnairesEndpoints.DeletePatientQuestionnaire}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<string> ExportToExcelAsync()
        {
            var response = await _tenantHttpClient.GetAsync(QuestionnairesEndpoints.Export);
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<PaginatedResult<GetAllPagedPatientQuestionnairesResponse>> GetPatientQuestionnairesAsync(int pageNumber, int pageSize, GetAllPagedPatientQuestionnairesRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(QuestionnairesEndpoints.GetAllPagedForPatient(pageNumber, pageSize, request.patientId));
            return await response.ToPaginatedResult<GetAllPagedPatientQuestionnairesResponse>();
        }

        public async Task<PaginatedResult<GetAllPagedClientQuestionnairesResponse>> GetQuestionnairesAsync(GetAllClientQuestionnairesQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(QuestionnairesEndpoints.GetAllPaged(request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<GetAllPagedClientQuestionnairesResponse>();
        }

        public async Task<IResult<int>> SavePatientQuestionnaireAsync(AddEditPatientQuestionnaireCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(QuestionnairesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> SaveClientQuestionnaireAsync(AddEditClientQuestionnaireCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(QuestionnairesEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
