using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.IntegratedServices.RpaInsurances.Queries.GetAll;
using System.Collections.Generic;
using MedHelpAuthorizations.Application.Features.IntegratedServices.RpaInsurances.Commands.Update;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.RpaInsurances
{
    public interface IRpaInsurancesManager : IManager
    {
        Task<PaginatedResult<GetAllRpaInsurancesResponse>> GetRpaInsurancesPagedAsync(GetAllRpaInsurancesQuery request);
        Task<IResult<List<GetAllRpaInsurancesResponse>>> GetRpaInsurancesAsync(GetAllRpaInsurancesQuery request);
        Task<IResult<int>> UpdateInactivatedOn(UpdateRpaInsuranceInactivatedOnCommand command);

        //Task<IResult<int>> DeleteAsync(int id);

        //Task<string> ExportToExcelAsync();

        //Task<IResult<GetAllRpaInsurancesResponse>> GetInsuranceByIdAsync(GetRpaInsuranceByIdQuery request);
    }
}