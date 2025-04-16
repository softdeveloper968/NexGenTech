using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Questionnaires.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Questionnaires.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Questionnaires.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Questionnaires.Queries.GetPatientQuestionnaire;
using MedHelpAuthorizations.Application.Requests.Questionnaires;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Questionnaire
{
    public interface IQuestionnaireManager : IManager
    {

        Task<IResult<GetClientQuestionnaireByIdResponse>> GetClientQuestionnaireByIdAsync(int id);
        Task<IResult<GetPatientQuestionnaireByIdResponse>> GetPatientQuestionnaireByIdAsync(int id);
        Task<PaginatedResult<GetAllPagedPatientQuestionnairesResponse>> GetPatientQuestionnairesAsync(GetAllPatientQuestionnairesQuery request);

        Task<PaginatedResult<GetAllPagedClientQuestionnairesResponse>> GetClientQuestionnairesAsync(GetAllPagedClientQuestionnairesRequest request);
        Task<IResult<int>> SavePatientQuestionnaireAsync(AddEditPatientQuestionnaireCommand request);
        Task<IResult<int>> SaveClientQuestionnaireAsync(AddEditClientQuestionnaireCommand request);
        Task<IResult<int>> DeletePatientQuestionnaireAsync(int id);
        Task<IResult<int>> DeleteClientQuestionnaireAsync(int id);
        Task<string> ExportToExcelAsync();
    }
}
