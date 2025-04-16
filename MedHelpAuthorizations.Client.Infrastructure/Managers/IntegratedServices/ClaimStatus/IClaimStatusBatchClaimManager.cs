﻿using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetByBatchId;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetById;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetProcedureCodes;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.ClaimStatus
{
    public interface IClaimStatusBatchClaimManager : IManager
    {
        Task<IResult<List<GetAllClaimStatusBatchClaimsResponse>>> GetAllAsync();

        Task<IResult<GetClaimStatusBatchClaimByIdResponse>> GetByIdAsync(int id);

        Task<IResult<List<GetClaimStatusBatchClaimsByBatchIdResponse>>> GetByBatchId(int claimStatusBatchId);

        Task<IResult<List<GetClaimStatusClientProcedureCodeResponse>>> GetClientClaimStatusProcedureCodesAsync();
    }
}
