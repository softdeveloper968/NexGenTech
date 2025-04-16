using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllByProviderId;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetLocationDataByClientId;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IClientLocationsManager: IManager
    {
        Task<IResult<List<GetClientLocationsByClientIdResponse>>> GetAllClientLocationsAsync();
        Task<IResult<List<GetClientLocationsByProviderIdResponse>>> GetAllClientLocationsByProviderIdAsync(int providerId);

        Task<PaginatedResult<GetAllClientLocationsResponse>> GetAllClientLocationsPagedAsync(GetAllPagedLocationsRequest request);

        //Task<IResult<List<GetClientInsurancesBySearchStringResponse>>> GetBySearchStringAsync(string searchString);

        Task<IResult<int>> SaveAsync(AddEditClientLocationCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<string> ExportToExcelAsync();

        Task<IResult<GetClientLocationByIdResponse>> GetLocaationByIdAsync(GetClientLocationByIdQuery request);
        Task<IResult<List<GetClientLocationsByClientIdResponse>>> GetLocationByClientIdAsync(GetLocationDataByClientIdQuery request);

        /// <summary>
        /// Get procedure code totals by locations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IResult<List<ClaimSummary>>> GetProcedureTotalsByLocationAsync(GetProcedureTotalsByLocationQuery request); //EN-312

        /// <summary>
        /// Get procedure code totals by locations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IResult<List<ClaimSummary>>> GetInsuranceTotalsByLocationAsync(GetInsuranceTotalsByLocationQuery request); //EN-312

        /// <summary>
        /// Get denial totals by locations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IResult<List<ClaimSummary>>> GetDenialReasonsByLocationsAsync(GetDenialReasonsByLocationsQuery request); //EN-312

        /// <summary>
        /// Get procedure reimbursement by locations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IResult<List<ClaimSummary>>> GetProcedureReimbursementByLocationAsync(GetProcedureReimbursementByLocationQuery request); //EN-312

        /// <summary>
        /// Get payer reimbursement by locations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IResult<List<ClaimSummary>>> GetPayerReimbursementByLocationAsync(GetPayerReimbursementByLocationQuery request); //EN-312

        /// <summary>
        /// Get avg days to pay by location
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IResult<List<AverageDaysByLocation>>> GetAverageDaysToPayByLocationAsync(GetAverageDaysToPayByLocationQuery request); //EN-312

        /// <summary>
        /// Get charges by location
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IResult<List<ChargesTotalsByLocation>>> GetChargesByLocationAsync(GetChargesByLocationQuery request); //EN-312
    }
}
