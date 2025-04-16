using MedHelpAuthorizations.Application.Features.Administration.UnmatchedReimbursements.Queries;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class UnmatchedReimbursementManager : IUnmatchedReimbursementManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public UnmatchedReimbursementManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<PaginatedResult<GetAllUnmatchedReimbursementResponse>> GetAllUnmatchedReimbursementPagedAsync(GetAllUnmatchedReimbursementRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(UnmatchedReimbursementEndPoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString));
            return await response.ToPaginatedResult<GetAllUnmatchedReimbursementResponse>();
        }
    }
}
