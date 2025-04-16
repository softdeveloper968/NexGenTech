using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ExceptionReason.Queries
{
    public class GetOtherExceptionReasonCategoryQuery : IRequest<Result<List<ExceptionReasonResponse>>>
    {
    }

    public class GetOtherExceptionReasonCategoryQueryHandler : IRequestHandler<GetOtherExceptionReasonCategoryQuery, Result<List<ExceptionReasonResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IClaimStatusTransactionRepository _claimStatusTransactionRepository;

        public GetOtherExceptionReasonCategoryQueryHandler(IUnitOfWork<int> unitOfWork, IClaimStatusTransactionRepository claimStatusTransactionRepository)
        {
            _unitOfWork = unitOfWork;
            _claimStatusTransactionRepository = claimStatusTransactionRepository;
        }

        public async Task<Result<List<ExceptionReasonResponse>>> Handle(GetOtherExceptionReasonCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = await _unitOfWork.Repository<ClaimStatusTransaction>().Entities.Where(tx => tx.ClaimStatusExceptionReasonCategoryId == 0)
                              .GroupBy(tx => tx.ExceptionReason)
                              .Select(group => new ExceptionReasonResponse
                              {
                                  ExceptionReason = group.Key,
                                  Quantity = group.Count()
                              }).ToListAsync();

                

                return await Result<List<ExceptionReasonResponse>>.SuccessAsync(query);
            }
            catch (Exception ex)
            {

                string errorMessage = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}{Environment.NewLine}{ex.InnerException?.InnerException?.Message}";
                return await Result<List<ExceptionReasonResponse>>.FailAsync(errorMessage);
            }

        }
    }
}
