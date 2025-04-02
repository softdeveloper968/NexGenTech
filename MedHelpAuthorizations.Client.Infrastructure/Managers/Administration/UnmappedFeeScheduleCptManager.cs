using MedHelpAuthorizations.Application.Features.Administration.UnMappedClientFeeSchedule.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class UnmappedFeeScheduleCptManager : IUnmappedFeeScheduleCptManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public UnmappedFeeScheduleCptManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<PaginatedResult<GetAllUnmappedFeeScheduleResponse>> GetAllUnmappedFeeSchduleCptPagedAsync(GetAllUnmappedFeeScheduleCptRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(UnmappedFeeScheduleEndPoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString));
            return await response.ToPaginatedResult<GetAllUnmappedFeeScheduleResponse>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{UnmappedFeeScheduleEndPoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> CleanUpUnmappedFeeScheduleCptAsync()
        {
            var response = await _tenantHttpClient.DeleteAsync(UnmappedFeeScheduleEndPoints.CleanUpUnmappedFeeScheduleCpt);
            return await response.ToResult<int>();
        }
    }
}
