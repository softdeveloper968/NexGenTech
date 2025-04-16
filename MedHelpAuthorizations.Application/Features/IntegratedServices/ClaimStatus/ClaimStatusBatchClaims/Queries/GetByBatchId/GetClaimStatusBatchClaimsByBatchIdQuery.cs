using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetByBatchId
{
    public class GetClaimStatusBatchClaimsByBatchIdQuery : IRequest<Result<List<GetClaimStatusBatchClaimsByBatchIdResponse>>>
    {
        //public int PageNumber { get; set; } = 1;// AA-321
        //public int PageSize { get; set; } = 100000;// AA-321
        public int ClaimStatusBatchId { get; set; }
        public int ReturnQuantityCap { get; set; } = 5000;
    }

    public class GetClaimStatusBatchClaimsByBatchIdQueryHandler : IRequestHandler<GetClaimStatusBatchClaimsByBatchIdQuery, Result<List<GetClaimStatusBatchClaimsByBatchIdResponse>>>
    {
        private readonly IClaimStatusBatchClaimsRepository _claimStatusBatchClaimsRepository;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetClaimStatusBatchClaimsByBatchIdQueryHandler(IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository, IUnitOfWork<int> unitOFWork, IMapper mapper)
        {
            _claimStatusBatchClaimsRepository = claimStatusBatchClaimsRepository;
            _unitOfWork = unitOFWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetClaimStatusBatchClaimsByBatchIdResponse>>> Handle(GetClaimStatusBatchClaimsByBatchIdQuery query, CancellationToken cancellationToken)
        {
            //var claimStatusBatchClaimsList = await _claimStatusBatchClaimsRepository.GetInitialClaimStatusByBatchIdAsync(query.ClaimStatusBatchId);
            var batch = await _unitOfWork.Repository<ClaimStatusBatch>().Entities
                                .Include(x => x.Client)
                                .Include("ClientInsurance.RpaInsurance")
                                .Specify(new IsActiveClientsSpecification<ClaimStatusBatch>())
                                .Where(x => x.Id == query.ClaimStatusBatchId)
                                .FirstOrDefaultAsync();

            if (batch != null)
            {
                var rpaConfig = await _unitOfWork.Repository<ClientInsuranceRpaConfiguration>().Entities
                                .Where(x =>
                                        //x.RpaInsuranceId == batch.ClientInsurance.RpaInsuranceId //AA-23
                                        //&& 
                                        x.ClientInsuranceId == batch.ClientInsuranceId
                                        && x.AuthTypeId == batch.AuthTypeId
                                        && x.TransactionTypeId == Domain.Entities.Enums.TransactionTypeEnum.ClaimStatus
                                        && !x.IsDeleted)
                                .FirstOrDefaultAsync();

                if (rpaConfig != null && rpaConfig.DailyClaimLimit > 0)
                {
                    query.ReturnQuantityCap = rpaConfig.DailyClaimLimit - rpaConfig.CurrentDayClaimCount;
                }
            }

            var claimStatusBatchClaimsList = await _claimStatusBatchClaimsRepository.GetUnresolvedByBatchIdAsync(query.ClaimStatusBatchId, query.ReturnQuantityCap);//, query.PageNumber, query.PageSize);
            return await Result<List<GetClaimStatusBatchClaimsByBatchIdResponse>>.SuccessAsync(claimStatusBatchClaimsList);
        }
    }
}
