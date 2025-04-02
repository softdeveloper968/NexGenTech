using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Cardholders.Queries.GetBySearchString;
using MedHelpAuthorizations.Application.Features.Patients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Patients.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Patients.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Patients.Queries.GetPatientsByCriteria;
using MedHelpAuthorizations.Application.Requests.Patients;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Patient
{
    public interface IPatientManager : IManager
    {
        Task<PaginatedResult<GetAllPagedPatientsResponse>> GetPatientsAsync(GetAllPagedPatientsRequest request);

        Task<IResult<string>> GetPatientImageAsync(int id);
        Task<IResult<GetPatientByIdResponse>> GetPatientByIdAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditPatientCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<string> ExportToExcelAsync();

        Task<PaginatedResult<GetPatientsByCriteriaResponse>> GetByCriteriaAsync(GetPatientsByCriteriaQuery query);

        public Task<IResult<List<GetPatientsBySearchStringResponse>>> GetBySearchStringAsync(string searchString);
    }
}