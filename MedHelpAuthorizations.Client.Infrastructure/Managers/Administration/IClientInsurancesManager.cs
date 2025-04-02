using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetRpaAssignedInsurances;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetById;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllByClientId;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IClientInsurancesManager : IManager
    {
        Task<IResult<List<GetAllClientInsurancesByClientIdResponse>>> GetAllClientInsurancesAsync();

        Task<PaginatedResult<GetAllPagedInsurancesResponse>> GetAllClientInsurancesPagedAsync(GetAllPagedInsurancesRequest request);

        Task<IResult<List<GetRpaAssignedInsurancesResponse>>> GetRpaAssignedInsurancesAsync(GetRpaAssignedInsurancesQuery request);
        
        Task<IResult<List<GetClientInsurancesBySearchStringResponse>>> GetBySearchStringAsync(string searchString);
        
        Task<IResult<int>> SaveAsync(AddEditInsuranceCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<string> ExportToExcelAsync();
        
        Task<IResult<GetInsuranceByIdResponse>> GetInsuranceByIdAsync(GetInsuranceByIdQuery request);
        Task<IResult<List<GetAllClientInsurancesByClientIdResponse>>> GetInsuranceByClientIdAsync(GetAllClientInsuranceByClientIdQuery request);
        Task<IResult<List<PayerProviderTotals>>> GetProviderTotalsByPayerAsync(GetProviderTotalsByPayerQuery request); //EN-278
        Task<IResult<List<ClaimSummary>>> GetPaymentTotalsByPayerAsync(GetPaymentsByInsuranceQuery request); //EN-278
        Task<IResult<List<ClaimSummary>>> GetDenialTotalsByPayerAsync(GetDenialsByInsuranceQuery request); //EN-278
        Task<IResult<List<PayerTotalsByProvider>>> GetPayerTotalsAsync(GetPayerTotalsQuery request); //EN-556
    }
}