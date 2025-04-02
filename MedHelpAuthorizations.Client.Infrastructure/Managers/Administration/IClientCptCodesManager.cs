using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IClientCptCodesManager : IManager
    {
        Task<PaginatedResult<GetAllPagedClientCptCodesResponse>> GetClientCptCodesAsync(GetAllPagedClientCptCodesRequest request);

        Task<IResult<int>> SaveAsync(AddEditClientCptCodeCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<string> ExportToExcelAsync();
        Task<IResult<GetClientCptCodeByIdResponse>> GetClientCptCodeByIdAsync(GetClientCptCodeByIdQuery request);
        Task<IResult<List<GetClientCptCodeByIdResponse>>> GetClientCptCodeByClientIdAsync();
        Task<IResult<GetClientCptCodeByIdResponse>> GetClientCptCodeByCodeAsync(string code);
        Task<IResult<GetClientCptCodeByIdResponse>> CheckMatchCpt(int id);

        Task<IResult<List<GetClientCptCodeByIdResponse>>> GetCptCodeBySearch(string searchString); //EN-258
        Task<IResult<List<ProviderTotalsByProcedure>>> GetProvidersByProcedureAsync(GetProviderByProcedureQuery criteria); //EN-334
        Task<IResult<List<ClaimSummary>>> GetInsuranceByProcedureAsync(GetInsuranceByProcedureCodeQuery criteria); //EN-334
        Task<IResult<List<ClaimSummary>>> GetDenialReasonsByProcedureAsync(GetDenialReasonsByProcedureCodeQuery criteria); //EN-334
        Task<IResult<List<ClaimSummary>>> GetPayerReimbursementByProcedureAsync(GetPayerReimbursementByProcedureCodeQuery criteria); //EN-334
        Task<IResult<List<ClaimSummary>>> GetProviderReimbursementByProcedureCodeAsync(GetProviderReimbursementByProcedureCodeQuery criteria); //EN-334
        Task<IResult<List<ClaimSummary>>> GetReimbursementByProcedureCode(ReimbursementByProcedureCodeQuery query); //EN-334
        Task<IResult<List<ChargesTotalsByProcedureCode>>> GetChargesByProcedureCodeQuery(ChargesByProcedureCodeQuery query); //EN-334
        Task<IResult<List<AverageDaysByProcedureCode>>> GetAverageDaysToPayByProcedureCodeQuery(GetAverageDaysToPayByProcedureCodeQuery query); //EN-334
        Task<IResult<List<ProviderTotals>>> GetProviderTotalsByProcedureCodeAsync(GetProviderTotalsByProcedureCodeQuery query);
    }
}