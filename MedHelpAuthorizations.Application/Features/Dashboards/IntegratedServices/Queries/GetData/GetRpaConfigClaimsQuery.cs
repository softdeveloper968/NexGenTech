using AutoMapper;
using MedHelpAuthorizations.Application.Features.RpaConfigClaims;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class RpaConfigClaimsQuery : IRequest<Result<List<RpaConfigClaimsDto>>>
    {
        public int RpaConfigId { get; set; } = 0;
        public RpaConfigClaimsQuery(int rpaConfigId = 0)
        {
            RpaConfigId = rpaConfigId;
        }
    }

    public class RpaConfigClaimsQueryHandler : IRequestHandler<RpaConfigClaimsQuery, Result<List<RpaConfigClaimsDto>>>
    {
        private readonly IRpaConfigClaimService _configClaimService;
        private readonly IMapper _mapper;

        public RpaConfigClaimsQueryHandler(IRpaConfigClaimService configClaimService)
        {
            _configClaimService = configClaimService;
        }

        public async Task<Result<List<RpaConfigClaimsDto>>> Handle(RpaConfigClaimsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.RpaConfigId == 0)
                {
                    var rpaConfigList = await _configClaimService.GetRpaConfigClaimsListAsync();
                    if (rpaConfigList == null)
                    {
                        return Result<List<RpaConfigClaimsDto>>.Fail("RPA configuration not found!");
                    }
                    return Result<List<RpaConfigClaimsDto>>.Success(rpaConfigList);
                }
                else
                {
                    var rpaConfigClaimsList = new List<RpaConfigClaimsDto>();
                    var rpaConfigClaims = await _configClaimService.ProcessClientClaimsAsync(cancellationToken, request.RpaConfigId);
                    if (rpaConfigClaims == null)
                    {
                        return Result<List<RpaConfigClaimsDto>>.Fail("RPA configuration not found!");
                    }
                    rpaConfigClaimsList.Add(rpaConfigClaims);
                    return Result<List<RpaConfigClaimsDto>>.Success(rpaConfigClaimsList);
                }
            }
            catch (Exception ex)
            {
                // Catch any exceptions and return a failure result
                return Result<List<RpaConfigClaimsDto>>.Fail("An error occurred while processing the claims.");
            }

        }
    }
}
