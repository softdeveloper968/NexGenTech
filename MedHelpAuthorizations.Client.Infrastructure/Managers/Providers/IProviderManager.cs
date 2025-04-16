using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Features.Providers.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Providers.GetByCriteria;
using MedHelpAuthorizations.Application.Features.Providers.Queries.GetAllProviders;
using MedHelpAuthorizations.Application.Features.Providers.Queries.GetProviderById;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Provider
{
    public interface IProviderManager : IManager
    {
        Task<IResult<List<GetAllProvidersResponse>>> GetAllClientProvidersAsync();
        Task<PaginatedResult<GetAllProvidersResponse>> GetAllPagedAsync(GetAllPagedClientProvidersRequest request);
        Task<PaginatedResult<GetProvidersByCriteriaResponse>> GetByCriteriaAsync(GetProvidersByCriteriaQuery request);
        Task<IResult<GetProviderByIdResponse>> GetByIdAsync(int id);
        Task<IResult<int>> SaveAsync(AddEditProviderCommand request);
        Task<IResult<int>> DeleteAsync(int id);
        /// <summary>
        /// To Get averaged days to pay by provider data
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<IResult<List<AverageDaysByProvider>>> GetAverageDaysByProviderAsync(GetAverageDaysToPayByProviderQuery criteria); //EN-190

        Task<IResult<List<ChargesTotalsByProvider>>> GetChargesByProviderAsync(ChargesByProviderQuery criteria);
        Task<IResult<List<ProcedureTotalsByProvider>>> GetProceduresByProviderAsync(GetProceduresByProviderQuery criteria); //EN-241
        Task<IResult<List<ClaimSummary>>> GetInsurancesByProviderQueryAsync(GetInsurancesByProviderQuery criteria); //EN-250
        Task<IResult<List<DenialReasonsTotalsByProvider>>> GetDenialReasonsByProviderQueryAsync(GetDenialReasonsByProviderQuery criteria); //EN-252
        Task<IResult<List<ClaimSummary>>> GetProcedureReimbursementByProviderQueryAsync(GetProcedureReimbursementByProviderQuery criteria); //EN-254
        Task<IResult<List<ClaimSummary>>> GetPayerReimbursementByProviderQueryAsync(GetPayerReimbursementByProviderQuery criteria); //EN-257
        Task<IResult<List<ProviderProcedureTotal>>> GetProviderProcedureTotalQueryAsync(GetProviderProcedureTotalQuery query);
        Task<IResult<List<ProviderDenialReasonTotal>>> GetDenialReasonTotalsByProviderIdQueryAsync(GetDenialReasonTotalsByProviderIdQuery query);
    }
}