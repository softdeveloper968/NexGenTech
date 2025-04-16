using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData
{
    public class ClaimWorkstationDetailsQuery : ClaimWorkstationDetailsQueryBase, IRequest<PaginatedResult<ClaimWorkstationDetailsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public ClaimWorkstationSearchOptions? ClaimWorkstationSearchOptions { get; set; } = null;
        //public string ClaimStatusCategory { get; set; } = null;
        //public List<int> ClaimsStatus { get; set; } = null;

        public ClaimWorkstationDetailsQuery(int pageNumber, int pageSize, ClaimWorkstationSearchOptions? claimWorkstationSearchOptions)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            ClaimWorkstationSearchOptions = claimWorkstationSearchOptions;
        }
    }

    public class ClaimWorkstationDetailsQueryHandler : IRequestHandler<ClaimWorkstationDetailsQuery, PaginatedResult<ClaimWorkstationDetailsResponse>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;

        public ClaimWorkstationDetailsQueryHandler(IClaimStatusQueryService claimStatusQueryService)
        {
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<PaginatedResult<ClaimWorkstationDetailsResponse>> Handle(ClaimWorkstationDetailsQuery query, CancellationToken cancellationToken)
        {
            return await _claimStatusQueryService.GetClaimsWorkstationDetailsAsync(query, query.PageNumber, query.PageSize, query.ClaimWorkstationSearchOptions);
        }
    }
}
